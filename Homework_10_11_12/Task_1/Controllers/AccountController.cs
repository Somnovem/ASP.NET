using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task_1.Models;
using Microsoft.AspNetCore.Authentication;

namespace Task_1.Controllers;

public class AccountController : Controller
{
      ApplicationContext _context;
      private readonly ILogger<AccountController> _logger;
      
      public AccountController(ApplicationContext db,ILogger<AccountController> logger)
      {
          _logger = logger;
          _context = db;
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
          return View(model);
      }

      [Route("Auth")]
      [Route("Authorisation")]
      [HttpPost]
      public async Task<IActionResult> Auth(PersonLogin person)
      {
          if (person.IsLoggingOut)
          {
              await HttpContext.SignOutAsync();
              return RedirectToAction("Auth");
          }
        bool isManager = false;
          if (person.IsLogin)
          {
              string encryptedPassword = Encrypter.CalculateSHA256(person.Password);
              User user = _context.Users.AsNoTracking().FirstOrDefault(user => user.Username == person.Username && user.Password == encryptedPassword);
              if (user == null)
              {
                  Manager manager = _context.Managers.AsNoTracking().FirstOrDefault(manager => manager.Username == person.Username && manager.Password == encryptedPassword);
                  if (manager != null)
                  {
                      Response.Cookies.Append("IsManager", "True");
                      isManager = true;
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
              User user = _context.Users.FirstOrDefault(user => user.Username == person.Username);
              if (user != null)
              {
                  ModelState.AddModelError("Username", "Username already in use");
              }
              user = _context.Users.FirstOrDefault(user => user.Email == person.Email);
              if (user != null)
              {
                  ModelState.AddModelError("Email", "Email already in use");
              }
              if (!ModelState.IsValid)
              {
                  return View(person);
              }

              User newUser = new User() { Id = Guid.NewGuid(), Username = person.Username, Email = person.Email, Password = Encrypter.CalculateSHA256(person.Password), Gender = person.Gender[0], BirthDate = person.Birthday ?? DateTime.Now};
              _context.Users.Add(newUser);
              _context.SaveChanges();
          }
          var claims = new List<Claim>
          {
              new Claim(ClaimTypes.Name, person.Username)
          };
          if (isManager) claims.Add(new Claim(ClaimTypes.Role, "manager"));
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
                        model.Users = _context.Users.AsNoTracking().ToList();
                        return View(model);
                    }
                    else return RedirectToAction("Index","Home");
      	}
      	else return RedirectToAction("Index","Home");
      }

      [Route("Mod")]
      [Route("Moder")]
      [Route("Moderation")]
      [HttpPost]
      public IActionResult Moderation(ModerationModel model)
      {
          if (model.IsDeleting) //Deleting a user
          {
              User user = _context.Users.Where(user => user.Id == model.Id).ToList()[0];
              _context.Users.Remove(user);
              _context.SaveChanges();
          }
          else
          {
              if (string.IsNullOrEmpty(model.Id.ToString())) //New Account
              {
                  User user = _context.Users.AsNoTracking().FirstOrDefault(user => user.Username == model.Username);
                  if (user != null)
                  {
                      ModelState.AddModelError("Username", "Username already in use");
                  }
                  user = _context.Users.AsNoTracking().FirstOrDefault(user => user.Email == model.Email);
                  if (user != null)
                  {
                      ModelState.AddModelError("Email", "Email already in use");
                  }
                  else
                  {
                      Manager manager = _context.Managers.AsNoTracking().FirstOrDefault(manager => manager.Email == model.Email);
                      if (manager != null)
                      {
                          ModelState.AddModelError("Email", "Email already in use");
                      }
                  }
                  if (!ModelState.IsValid)
                  {
                      return View(model);
                  }
                  User newUser = new User() { Id = Guid.NewGuid(),Username = model.Username, Email = model.Email!, Password = Encrypter.CalculateSHA256(model.Password), Gender = model.Gender![0], BirthDate = model.Birthday ?? DateTime.Now };
                  _context.Users.Add(newUser);
                  _context.SaveChanges();
              }
              else //Changing existing
              {
                  User user = _context.Users.AsNoTracking().FirstOrDefault(user => user.Username == model.Username && user.Id != model.Id);
                  if (user != null)
                  {
                      ModelState.AddModelError("Username", "Username already in use");
                  }
                  user = _context.Users.AsNoTracking().FirstOrDefault(user => user.Email == model.Email && user.Id != model.Id);
                  if (user != null)
                  {
                      ModelState.AddModelError("Email", "Email already in use");
                  }
                  else
                  {
                      Manager manager = _context.Managers.AsNoTracking().FirstOrDefault(manager => manager.Email == model.Email && manager.Id != model.Id);
                      if (manager != null)
                      {
                          ModelState.AddModelError("Email", "Email already in use");
                      }
                  }
                  if (!ModelState.IsValid)
                  {
                      return View(model);
                  }
                  var userToChange = _context.Users.Where(user => user.Id == model.Id).ToList()[0];
                  userToChange.Username = model.Username;
                  userToChange.Email = model.Email;
                  userToChange.Password = model.Password;
                  userToChange.BirthDate = model.Birthday ?? DateTime.Now; //Birthday will always be not null
                  userToChange.Gender = model.Gender[0];
                  _context.SaveChanges();
              }
          }
          return RedirectToAction("Moderation");
      }
}