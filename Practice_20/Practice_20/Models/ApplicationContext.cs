using Microsoft.EntityFrameworkCore;
namespace Practice_20.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "adminadmin@gmail.com";
            string adminPassword = "�~|L�*�_ڸt����Z\u0006VI!��Hh\u0005�'��$W"; //Admin_123

            string userEmail = "useruser@gmail.com";
            string userPassword = "\u0017��:+?��.(��\u001e��I�]�\u0013SB�ğ�\u001a@����"; //User_1234

            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id, BirthDate = DateTime.Parse("01.01.2000")};
            User userUser = new User { Id = 2, Email = userEmail, Password = userPassword, RoleId = userRole.Id, BirthDate = DateTime.Parse("08.08.2005")};

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser, userUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}
