using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Models;
using System.Text.RegularExpressions;

namespace Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        ApplicationContext db;
        public UsersController(ApplicationContext context)
        {
            db = context;
            if (!db.Users.Any())
            {
                db.Users.Add(new User { Firstname = "Tom", Lastname = "Brady", Birthday = DateTime.Parse("01.01.1990") });
                db.Users.Add(new User { Firstname = "Elvis", Lastname = "Prestley", Birthday = DateTime.Parse("03.06.1994") });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var temp =  await db.Users.AsNoTracking().ToListAsync();
            return temp;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            User user = await db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return user == null ? NotFound() : new ObjectResult(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            if (DateTime.Now.Year - user.Birthday.Year < 18)
                ModelState.AddModelError("Birthday", "Must be at least 18 years old");

            if (Regex.IsMatch(user.Firstname, "^[a-zA-Z]$"))
            {
                ModelState.AddModelError("Firstname", "Must contain only english letters");
            }

            if (Regex.IsMatch(user.Lastname, "^[a-zA-Z]$"))
            {
                ModelState.AddModelError("Lastname", "Must contain only english letters");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult<User>> Put(User user)
        {
            if (db.Users.AsNoTracking().FirstOrDefault(x => x.Id == user.Id) == null)
            {
                return NotFound();
            }

            db.Update(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            User user = db.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
    }
}