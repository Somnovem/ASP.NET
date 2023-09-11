using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_20.Models;
using Practice_20.ViewModels;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace Practice_20.Controllers
{
    public class AccountController : Controller
    {

        private readonly ApplicationContext _context;
        private readonly ILogger<AccountController> _logger;
        public AccountController(ApplicationContext db, ILogger<AccountController> logger)
        {
            _logger = logger;
            _context = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Email) == null)
            {
                return View();
            }

            return RedirectToAction("LoggedIn");
        }
    
        [HttpGet]
        public IActionResult SignUp()
        {
            if (HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Email) == null)
            {
                return View();
            }

            return RedirectToAction("Login");
        }
        
        [NonAction]
        private async Task<int> ApplyClaims(string email,DateTime birthday, Role role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.DateOfBirth, birthday.ToString()),
                new Claim(ClaimTypes.Role, role.Name)
            };
            var identity = new ClaimsIdentity(claims, "PracticeAuthScheme");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("PracticeAuthScheme", principal);
            return 0;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string encryptedPassword = Encrypter.CalculateSHA256(model.Password);
            User userDb = _context.Users.AsNoTracking().Where(user => user.Email == model.Email && user.Password == encryptedPassword).Include(x => x.Role).FirstOrDefault();
            if (userDb == null)
            {
                ModelState.AddModelError("Email", "No such email found or password not valid");
                return View(model);
            }
            
            await ApplyClaims(model.Email, userDb.BirthDate,userDb.Role);
            
            return RedirectToAction("LoggedIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterModel model)
        {
            User userDb = _context.Users.FirstOrDefault(user => user.Email == model.Email);
            userDb = _context.Users.FirstOrDefault(user => user.Email == model.Email);
            if (userDb != null)
            {
                ModelState.AddModelError("Email", "Email already in use");
                return View(model);
            }

            User newUser = new User() { Email = model.Email, Password = Encrypter.CalculateSHA256(model.Password), BirthDate = model.Birthday,RoleId = 2};
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            await ApplyClaims(model.Email, model.Birthday,new Role(){Id = 2, Name = "user"});

            return RedirectToAction("LoggedIn");
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public IActionResult LoggedIn()
        {
            return View(new LoggedInModel()
            {
                Email = HttpContext.User.FindFirst(c=>c.Type == ClaimTypes.Email)!.Value,
                Birthday = DateTime.Parse(HttpContext.User.FindFirst(c=>c.Type == ClaimTypes.DateOfBirth)!.Value)
            });
        }
        
        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public IActionResult LoggedIn(LoggedInModel model)
        {
            if (model.LogOut)
            {
                HttpContext.SignOutAsync();
                return RedirectToAction("Login");
            }

            return View(model);
        }
        
        
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Users()
        {
            UsersViewModel model = new UsersViewModel()
            {
                Users = _context.Users.AsNoTracking().ToList(),
                Roles = _context.Roles.AsNoTracking().ToList()
            };
            return View(model);
        }

        [NonAction]
        private bool InputHasErrors(User user)
        {
            bool hasErrors = false;
            string emailRegexScheme ="^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";
            Regex emailRegex = new Regex(emailRegexScheme);
            if (!emailRegex.IsMatch(user.Email))
            {
                ModelState.AddModelError("User.Email","Invalid email format");
                hasErrors = true;
            }

            if (user.Password.Length < 8 || user.Password.Length > 16)
            {
                ModelState.AddModelError("User.Password","Invalid password format: 8-16 length, 1 Uppercase, 1 Lowercase, 1 digit, 1 of [+-_]");
                hasErrors = true;
            }
            else
            {
                bool hasUppercase = false;
                bool hasLowercase = false;
                bool hasNumber = false;
                bool hasSpecial = false;
                Regex uppercaseRegex = new Regex("[A-Z]");
                Regex lowercaseRegex = new Regex("[a-z]");
                Regex numbersRegex = new Regex("[\\d]");
                Regex specialsRegex = new Regex("[+-_]");
                for (int i = 0; i < user.Password.Length; i++) {
                    string symbol = user.Password.Substring(i,1);
                    if (uppercaseRegex.IsMatch(symbol)) {
                        hasUppercase = true;
                    } else if (lowercaseRegex.IsMatch(symbol)) {
                        hasLowercase = true;
                    } else if (numbersRegex.IsMatch(symbol)) {
                        hasNumber = true;
                    } else if (specialsRegex.IsMatch(symbol)) {
                        hasSpecial = true;
                    }
                }

                if (!(hasUppercase && hasLowercase && hasNumber && hasSpecial))
                {
                    ModelState.AddModelError("User.Password","Invalid password format: 8-16 length, 1 Uppercase, 1 Lowercase, 1 digit, 1 of [+-_]");
                    hasErrors = true;
                }
            }
           

            return hasErrors;
        }
        
        [NonAction]
        private UsersViewModel GenerateNewUsersViewModel(User user)
        {
            return new UsersViewModel()
            {
                User = user,
                Users = _context.Users.AsNoTracking().ToList(),
                Roles = _context.Roles.AsNoTracking().ToList()
            };
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Users(UsersViewModel model)
        {
            switch (model.Action) 
            {
                case FormAction.Create:
                    if (InputHasErrors(model.User)) return View(GenerateNewUsersViewModel(model.User));
                    User? userTemp = _context.Users.AsNoTracking().FirstOrDefault(x => x.Email == model.User.Email);
                    if (userTemp != null)
                    {
                        ModelState.AddModelError("Email","Email already in use");
                        return View(GenerateNewUsersViewModel(model.User));
                    }

                    _context.Users.Add(new User()
                    {
                        Email = model.User.Email,
                        Password = Encrypter.CalculateSHA256(model.User.Password),
                        BirthDate = model.User.BirthDate,
                        RoleId = model.User.RoleId
                    });
                    _context.SaveChanges();
                    break;
                case FormAction.Update:
                    if (InputHasErrors(model.User)) return View(model);
                    User? userSameEmail = _context.Users.AsNoTracking().FirstOrDefault(x => x.Email == model.User.Email && x.Id != model.User.Id);
                    if (userSameEmail != null)
                    {
                        ModelState.AddModelError("User.Email","Email already in use");
                        return View(GenerateNewUsersViewModel(model.User));
                    }
                    User userToChange = _context.Users.First(x => x.Id == model.User.Id);
                    userToChange.Email = model.User.Email;
                    userToChange.Password = Encrypter.CalculateSHA256(model.User.Password);
                    userToChange.BirthDate = model.User.BirthDate;
                    userToChange.RoleId = model.User.RoleId;
                    _context.SaveChanges();
                    break;
                case FormAction.Delete:
                    User userToDelete = _context.Users.First(x => x.Id == model.User.Id);
                    _context.Users.Remove(userToDelete);
                    _context.SaveChanges();
                    break;
            }
            
            return View(GenerateNewUsersViewModel(model.User));
        }
    }
}
