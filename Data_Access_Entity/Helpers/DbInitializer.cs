using Data_Access_Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Data_Access_Entity.Helpers
{
    internal static class DbInitializer
    {
        public static void SeedAdmins(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(new Admin[]
            {
                new Admin()
                {
                    ID= 1,
                    Login = "Admin",
                    Password = "1234"
                },
            });
        }
        public static void SeedCategories(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(new Category[]
            {
                new Category()
                {
                    ID = 1,
                    Name = "Entrees",
                },
                new Category()
                {
                    ID = 2,
                    Name = "Soup",
                },
                new Category()
                {
                    ID = 3,
                    Name = "Salad",
                },
                new Category()
                {
                    ID = 4,
                    Name = "Dishes",
                },
                new Category()
                {
                    ID = 5,
                    Name = "Pizza",
                },
                new Category()
                {
                    ID = 6,
                    Name = "Drinks",
                },
                new Category()
                {
                    ID = 7,
                    Name = "Desserts",
                },
                new Category()
                {
                    ID = 8,
                    Name = "Ice Cream",
                },
            });
        }
    }
}