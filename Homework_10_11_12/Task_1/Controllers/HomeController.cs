using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using Task_1.Models;
using Task_1.ViewModels;
using Task_1.Filters;
using Task_1.Sessions;

namespace Task_1.Controllers
{
    [KMeleonFilter]
    [RequestTimeFilter]
    public class HomeController : Controller
    {
        ApplicationContext _context;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ApplicationContext db,ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = db;
        }

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel()
            {
                TopViewedProducts = _context.Products.AsNoTracking().OrderByDescending(x=>x.Views).Take(3).ToList()
            };
            return View(model);
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
            
            IQueryable<Review> reviews = _context.Reviews.AsNoTracking().Where(review => review.ProductId == id).Include(x => x.User);

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
            var product = _context.Products.AsNoTracking().Where(product => product.Id == id).Include(x => x.Reviews).ThenInclude(review => review.User).ToList()[0];
            DetailsViewModel model = new DetailsViewModel(
                product,
                reviews,
                new PageViewModel(_context.Reviews.AsNoTracking().Count(), page, reviewCount),
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
            if (!HttpContext.Session.Keys.Contains("ViewedProductIds"))
            {
                HttpContext.Session.Set<List<int>>("ViewedProductIds", new List<int>());
            }
            if (User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name) != null && !HttpContext.Session.Get<List<int>>("ViewedProductIds")!.Contains(id))
            {
                Product productViewed = _context.Products.First(product => product.Id == id);
                ++productViewed.Views;
                _context.SaveChanges();
                List<int> newViewedProductIds = HttpContext.Session.Get<List<int>>("ViewedProductIds")!;
                newViewedProductIds.Add(id);
                HttpContext.Session.Set<List<int>>("ViewedProductIds", newViewedProductIds);
            }
            return FormDetailsPage(id, name, rating, page, sortOrder);
        }

        
        [Route("Details/{id:int}")]
        [HttpPost]
        public IActionResult Details(int id,string username, int rating, string message)
        {
            int userId = _context.Users.AsNoTracking().First(x => x.Username == username).Id;
            _context.Reviews.Add(new Review()
            {
                Message = message,
                UserId = userId,
                Rating = rating,
                ProductId = id
            });
            _context.SaveChanges();
            return FormDetailsPage(id, "", 0, 1, SortState.NameAsc);
        }

        [Route("Store")]
        [Route("Buy")]
        [LastVisitResourceFilter]
        public async Task<IActionResult> Store()
        {
            StoreViewModel model = new StoreViewModel();
            model.Products = _context.Products.AsNoTracking().ToList();
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
                        _context.Products.AddRange(model.Products);
                        _context.SaveChanges();
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
                    model.UserMessages = _context.UserMessages.AsNoTracking().ToList();
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Auth","Account");
            }
        }

        [HttpPost]
        [Route("Reviews")]
        public IActionResult Reviews(string username, string message)
        {
            try
            {
                _context.UserMessages.Add(new UserMessage
                {
                    UserId = _context.Users.Where(user => user.Username == username).ToList()[0].Id,
                    MessageText = message,
                    DateSent = DateTime.Now
                });
                _context.SaveChanges();
                return RedirectToAction("Store");
            }
            catch
            {
                return View();
            }
        }

		public IActionResult StoreItems()
        {
            var jsonData = JsonConvert.SerializeObject(_context.Products.AsNoTracking().ToList());
            return new JsonResult(jsonData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}