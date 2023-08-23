﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using Task_1.Models;
using Task_1.ViewModels;

namespace Task_1.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext context;
        public HomeController(ApplicationContext db)
        {
            context = db;
        }

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("About")]
        [Route("Info")]
        public IActionResult About()
        {
            return View();
        }

        [Route("Details/{id:int}")]
        public IActionResult Details(int id)
        {
            return View(context.Products.AsNoTracking().Where(product => product.Id == id).ToList()[0]);
        }

        [Route("Store")]
        [Route("Buy")]
        public async Task<IActionResult> Store()
        {
            StoreViewModel model = new StoreViewModel();
            model.Products = context.Products.AsNoTracking().ToList();
            if (model.Products.Count == 0)
            {
                var apiUrl = $"https://dummyjson.com/products";
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(apiUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        return BadRequest("Error fetching data from the API.");
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var searchResults = JsonConvert.DeserializeObject<ProductAPIResponse>(json);
                    if (searchResults?.products != null)
                    {
                        foreach (var item in searchResults.products)
                        {
                            model.Products.Add(new Product()
                            {
                                Title = item.Title,
                                Price = item.Price,
                                Rating = item.Rating,
                                Stock = item.Stock,
                                Brand = item.Brand,
                                Category = item.Category,
                                Description = item.Description,
                                DiscountPercentage = item.DiscountPercentage,
                                Thumbnail = item.Thumbnail
                            });
                        }
                        context.Products.AddRange(model.Products);
                        context.SaveChanges();
                    }
                }
            }
            model.Products.ForEach(p => {
                if (!model.Categories.Contains(p.Category)) model.Categories.Add(p.Category);
            });
            return View(model);
        }

        [HttpGet]
        [Route("Reviews")]
        public IActionResult Reviews()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                ReviewsViewModel model = new ReviewsViewModel();
                model.Username = identity.FindFirst(ClaimTypes.Name)?.Value;
                bool.TryParse(Request.Cookies["IsManager"], out bool isManagerValue);
                model.IsManager = isManagerValue;
                if (model.IsManager)
                {
                    model.UserMessages = context.UserMessages.AsNoTracking().ToList();
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Auth");
            }
        }

        [HttpPost]
        [Route("Reviews")]
        public IActionResult Reviews(string username, string message)
        {
            try
            {
                context.UserMessages.Add(new UserMessage
                {
                    UserId = context.Users.Where(user => user.Username == username).ToList()[0].Id,
                    MessageText = message,
                    DateSent = DateTime.Now
                });
                context.SaveChanges();
                return RedirectToAction("Store");
            }
            catch
            {
                return View();
            }
        }


        [Route("Auth")]
        [Route("Authorisation")]
        [HttpGet]
        public IActionResult Auth()
        {
            AuthViewModel model = new AuthViewModel();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            model.IsLoggedIn = (identity != null && identity.IsAuthenticated);
            model.Username = identity.FindFirst(ClaimTypes.Name)?.Value;
            return View(model);
        }

        [Route("Auth")]
        [Route("Authorisation")]
        [HttpPost]
        public async Task<IActionResult> Auth(bool isLogin,string username,string password, string? email, string? gender,DateTime? birthday)
        {
            if (isLogin)
            {
                var user = context.Users.AsNoTracking().Where(user => user.Username == username && user.Password == Encrypter.CalculateSHA256(password)).ToList();
                if (user.Count == 0)
                {
                    var manager = context.Managers.AsNoTracking().Where(manager => manager.Username == username && manager.Password == Encrypter.CalculateSHA256(password)).ToList();
                    if (manager.Count != 0)
                    {
                        Response.Cookies.Append("IsManager", "True");
                    }
                    else
                    {
                        Response.Cookies.Append("IsManager", "False");
                        return View();
                    }
                }
            }
            else
            {
                var user = context.Users.Where(user => user.Username == username || user.Email == email).ToList();
                if (user.Count != 0) return View();

                User newUser = new User() { Username = username, Email = email, Password = Encrypter.CalculateSHA256(password), Gender = gender[0], BirthDate = birthday ?? DateTime.Now};
                context.Users.Add(newUser);
                context.SaveChanges();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };
            var identity = new ClaimsIdentity(claims, "LoremIpsumAuthScheme");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("LoremIpsumAuthScheme", principal);
            return RedirectToAction("Reviews");
        }

        public IActionResult StoreItems()
        {
            var jsonData = JsonConvert.SerializeObject(context.Products.AsNoTracking().ToList());
            return new JsonResult(jsonData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}