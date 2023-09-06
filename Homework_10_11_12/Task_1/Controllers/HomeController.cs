using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using Task_1.Models;
using Task_1.ViewModels;
using Task_1.Filters;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Task_1.Controllers
{
    [KMeleonFilter]
    [RequestTimeFilter]
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

        [NonAction]
        private IActionResult FormDetailsPage(int id, string name, int rating, int page, SortState sortOrder)
        {
            int reviewCount = 3;
            
            IQueryable<Review> reviews = context.Reviews.AsNoTracking().Where(review => review.ProductId == id).Include(x => x.User);

            //Filtering
            if (!string.IsNullOrEmpty(name)) reviews = reviews.Where(review => review.User!.Username.Contains(name));
            if (rating != 0) reviews = reviews.Where(review => review.Rating == rating);


            //Sorting
            switch (sortOrder)
            {
                case SortState.NameDesc:
                    reviews = reviews.OrderByDescending(s => s.User!.Username);
                    break;
                case SortState.RatingAsc:
                    reviews = reviews.OrderBy(s => s.Rating);
                    break;
                case SortState.RatingDesc:
                    reviews = reviews.OrderByDescending(s => s.Rating);
                    break;
                default:
                    reviews = reviews.OrderBy(s => s.User!.Username);
                    break;
            }

            //Paginating
            reviews = reviews.Skip((page - 1) * reviewCount).Take(reviewCount);
            List<int> ratings = new List<int>() {1,2,3,4,5};
            var product = context.Products.AsNoTracking().Where(product => product.Id == id).Include(x => x.Reviews).ThenInclude(review => review.User).ToList()[0];
            DetailsViewModel model = new DetailsViewModel(
                product,
                reviews,
                new PageViewModel(context.Reviews.AsNoTracking().Count(), page, reviewCount),
                new FilterViewModel(ratings, rating, name),
                new SortViewModel(sortOrder)
            );
            return View(model);
        }

        [Route("Details/{id:int}")]
        [IdStabilizer]
        [HttpGet]
        public IActionResult Details(int id,string name, int rating = 0, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            return FormDetailsPage(id, name, rating, page, sortOrder);
        }

        
        [Route("Details/{id:int}")]
        [HttpPost]
        public IActionResult Details(int id,string username, int rating, string message)
        {
            int userId = context.Users.AsNoTracking().First(x => x.Username == username).Id;
            context.Reviews.Add(new Review()
            {
                Message = message,
                UserId = userId,
                Rating = rating,
                ProductId = id
            });
            context.SaveChanges();
            return View(context.Products.AsNoTracking().Where(product => product.Id == id).Include(x => x.Reviews).ThenInclude(review => review.User).ToList()[0]);
        }

        [Route("Store")]
        [Route("Buy")]
        [LastVisitResourceFilter]
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
            PersonLogin model = new PersonLogin();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            model.IsLoggedIn = (identity != null && identity.IsAuthenticated);
            model.Username = identity.FindFirst(ClaimTypes.Name)?.Value;
            bool.TryParse(Request.Cookies["IsManager"], out bool isManagerValue);
            model.IsManager = isManagerValue;
            return View(model);
        }

        [Route("Auth")]
        [Route("Authorisation")]
        [HttpPost]
        public async Task<IActionResult> Auth(PersonLogin person)
        {
            if (person.IsLogin)
            {
                string encryptedPassword = Encrypter.CalculateSHA256(person.Password);
                User user = context.Users.AsNoTracking().FirstOrDefault(user => user.Username == person.Username && user.Password == encryptedPassword);
                if (user == null)
                {
                    Manager manager = context.Managers.AsNoTracking().FirstOrDefault(manager => manager.Username == person.Username && manager.Password == encryptedPassword);
                    if (manager != null)
                    {
                        Response.Cookies.Append("IsManager", "True");
                    }
                    else
                    {
                        Response.Cookies.Append("IsManager", "False");
                        ModelState.AddModelError("", "No such user found or invalid password");
                        return View(person);
                    }
                }
            }
            else
            {
                User user = context.Users.FirstOrDefault(user => user.Username == person.Username);
                int errors = 0;
                if (user != null)
                {
                    ModelState.AddModelError("Username", "Username already in use");
                    ++errors;
                }
                user = context.Users.FirstOrDefault(user => user.Email == person.Email);
                if (user != null)
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    ++errors;
                }
                if (errors != 0)
                {
                    return View(person);
                }

                User newUser = new User() { Username = person.Username, Email = person.Email, Password = Encrypter.CalculateSHA256(person.Password), Gender = person.Gender[0], BirthDate = person.Birthday ?? DateTime.Now};
                context.Users.Add(newUser);
                context.SaveChanges();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, person.Username)
            };
            var identity = new ClaimsIdentity(claims, "LoremIpsumAuthScheme");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("LoremIpsumAuthScheme", principal);
            return RedirectToAction("Auth");
        }



		[Route("Mod")]
		[Route("Moder")]
		[Route("Moderation")]
		[HttpGet]
		public IActionResult Moderation()
		{
			var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
			{
				bool.TryParse(Request.Cookies["IsManager"], out bool isManagerValue);
                if (isManagerValue)
                {
                    ModerationModel model = new ModerationModel();
                    model.Users = context.Users.AsNoTracking().ToList();
                    return View(model);
                }
                else return RedirectToAction("Index");
			}
			else return RedirectToAction("Index");
		}

        [Route("Mod")]
        [Route("Moder")]
        [Route("Moderation")]
        [HttpPost]
        public IActionResult Moderation(ModerationModel model)
        {
            if (model.IsDeleting) //Deleting a user
            {
                User user = context.Users.Where(user => user.Id == model.Id).ToList()[0];
                context.Users.Remove(user);
                context.SaveChanges();
            }
            else
            {
                if (model.Id == 0) //New Account
                {
                    User user = context.Users.AsNoTracking().FirstOrDefault(user => user.Username == model.Username);
                    int errors = 0;
                    if (user != null)
                    {
                        ModelState.AddModelError("Username", "Username already in use");
                        ++errors;
                    }
                    user = context.Users.AsNoTracking().FirstOrDefault(user => user.Email == model.Email);
                    if (user != null)
                    {
                        ModelState.AddModelError("Email", "Email already in use");
                        ++errors;
                    }
                    else
                    {
                        Manager manager = context.Managers.AsNoTracking().FirstOrDefault(manager => manager.Email == model.Email);
                        if (manager != null)
                        {
                            ModelState.AddModelError("Email", "Email already in use");
                            ++errors;
                        }
                    }
                    if (errors != 0)
                    {
                        return View(model);
                    }
                    User newUser = new User() { Username = model.Username, Email = model.Email!, Password = Encrypter.CalculateSHA256(model.Password), Gender = model.Gender![0], BirthDate = model.Birthday ?? DateTime.Now };
                    context.Users.Add(newUser);
                    context.SaveChanges();
                }
                else //Changing existing
                {
                    int errors = 0;
                    User user = context.Users.AsNoTracking().FirstOrDefault(user => user.Username == model.Username && user.Id != model.Id);
                    if (user != null)
                    {
                        ModelState.AddModelError("Username", "Username already in use");
                        ++errors;
                    }
                    user = context.Users.AsNoTracking().FirstOrDefault(user => user.Email == model.Email && user.Id != model.Id);
                    if (user != null)
                    {
                        ModelState.AddModelError("Email", "Email already in use");
                        ++errors;
                    }
                    else
                    {
                        Manager manager = context.Managers.AsNoTracking().FirstOrDefault(manager => manager.Email == model.Email && manager.Id != model.Id);
                        if (manager != null)
                        {
                            ModelState.AddModelError("Email", "Email already in use");
                            ++errors;
                        }
                    }
                    if (errors != 0)
                    {
                        return View(model);
                    }
                    var userToChange = context.Users.Where(user => user.Id == model.Id).ToList()[0];
                    userToChange.Username = model.Username;
                    userToChange.Email = model.Email;
                    userToChange.Password = model.Password;
                    userToChange.BirthDate = model.Birthday ?? DateTime.Now; //Birthday will always be not null
                    userToChange.Gender = model.Gender[0];
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Moderation");
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