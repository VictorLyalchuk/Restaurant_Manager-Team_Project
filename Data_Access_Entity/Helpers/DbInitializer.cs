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
                    Name = "Pizza",
                },
                new Category()
                {
                    ID = 5,
                    Name = "Sushi",
                },
                new Category()
                {
                    ID = 6,
                    Name = "Burgers",
                },
                new Category()
                {
                    ID = 7,
                    Name = "Desserts",
                },
                new Category()
                {
                    ID = 8,
                    Name = "Drinks",
                },
            });
        }
        public static void SeedWaiters(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Waiter>().HasData(new Waiter[]
            {
                new Waiter()
                {
                    ID= 1,
                    FirstName = "Sarah",
                    SurName = "Larson",
                    BirthDate = new DateTime(1978,12,20),
                    StartWorkingDate = new DateTime(2023,01,01),
                    Salary = 7000,
                },
                new Waiter()
                {
                    ID= 2,
                    FirstName = "Adrianne",
                    SurName = "Curry",
                    BirthDate = new DateTime(1982,08,06),
                    StartWorkingDate = new DateTime(2023,02,01),
                    Salary = 3000,
                },
                new Waiter()
                {
                    ID= 3,
                    FirstName = "Courtney",
                    SurName = "Yates",
                    BirthDate = new DateTime(1981,03,26),
                    StartWorkingDate = new DateTime(2023,03,01),
                    Salary = 3000,
                },
                new Waiter()
                {
                    ID= 4,
                    FirstName = "Lindsey",
                    SurName = "Vuolo",
                    BirthDate = new DateTime(1981,10,19),
                    StartWorkingDate = new DateTime(2023,03,01),
                    Salary = 3000,
                },
                new Waiter()
                {
                    ID= 5,
                    FirstName = "Jenna",
                    SurName = "Lewis",
                    BirthDate = new DateTime(1977,07,16),
                    StartWorkingDate = new DateTime(2023,03,01),
                    Salary = 3000,
                },
            });
        }
        public static void SeedTables(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Table>().HasData(new Table[]
            {
                new Table()
                {
                    ID = 1,
                    WaiterId = 1
                    
                },
                new Table()
                {
                    ID = 2,
                    WaiterId = 1
                },
                new Table()
                {
                    ID = 3,
                    WaiterId = 1
                },
                new Table()
                {
                    ID = 4,
                    WaiterId = 2
                },
                new Table()
                {
                    ID = 5,
                    WaiterId = 2
                },
                new Table()
                {
                    ID = 6,
                    WaiterId = 3
                },
                new Table()
                {
                    ID = 7,
                    WaiterId = 3
                },
                new Table()
                {
                    ID = 8,
                    WaiterId = 4
                },
                new Table()
                {
                    ID = 9,
                    WaiterId = 4
                },
                new Table()
                {
                    ID = 10,
                    WaiterId = 5
                },
            });
        }
        public static void SeedProducts(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(new Product[]
            {
                new Product()
                {
                    ID = 1,
                    CategoryId = 1,
                    Name = "Butter Chicken",
                    Price = 15.3
                },
                new Product()
                {
                    ID = 2,
                    CategoryId = 1,
                    Name = "Palak Paneer",
                    Price = 12.8
                },
                new Product()
                {
                    ID = 3,
                    CategoryId = 1,
                    Name = "Potato With Chicken",
                    Price = 12.4
                },
                new Product()
                {
                    ID = 4,
                    CategoryId = 1,
                    Name = "Jamaican Jerk Chicken",
                    Price = 17.5
                },
                new Product()
                {
                    ID = 5,
                    CategoryId = 1,
                    Name = "Salmon Chowder",
                    Price = 22.0
                },
                new Product()
                {
                    ID = 6,
                    CategoryId = 2,
                    Name = "Gazpacho Soup",
                    Price = 12.25
                },
                new Product()
                {
                    ID = 7,
                    CategoryId = 2,
                    Name = "Harira Soup",
                    Price = 8.3
                },
                new Product()
                {
                    ID = 8,
                    CategoryId = 2,
                    Name = "Minestrone Soup",
                    Price = 15.5
                },
                new Product()
                {
                    ID = 9,
                    CategoryId = 2,
                    Name = "Tom Yum Soup",
                    Price = 19.6
                },
                new Product()
                {
                    ID = 10,
                    CategoryId = 2,
                    Name = "Chupe de Marisco Soup",
                    Price = 26.4
                },
                new Product()
                {
                    ID = 11,
                    CategoryId = 3,
                    Name = "Caprese Salad",
                    Price = 12.2

                },
                new Product()
                {
                    ID = 12,
                    CategoryId = 3,
                    Name = "Greek Salad",
                    Price = 15
                },
                new Product()
                {
                    ID = 13,
                    CategoryId = 3,
                    Name = "Waldorf Salad",
                    Price = 11.4
                },
                new Product()
                {
                    ID = 14,
                    CategoryId = 3,
                    Name = "Caprese Salad",
                    Price = 15.5
                },
                new Product()
                {
                    ID = 15,
                    CategoryId = 3,
                    Name = "Olivier Salad",
                    Price = 11.2
                },
                new Product()
                {
                    ID = 16,
                    CategoryId = 4,
                    Name = "Diablo Pizza",
                    Price = 9.6
                },
                new Product()
                {
                    ID = 17,
                    CategoryId = 4,
                    Name = "Pepperoni Pizza",
                    Price = 9.7
                },
                new Product()
                {
                    ID = 18,
                    CategoryId = 4,
                    Name = "Margherita Pizza",
                    Price = 8.6
                },
                new Product()
                {
                    ID = 19,
                    CategoryId = 4,
                    Name = "Carbonaro Pizza",
                    Price = 8.9
                },
                new Product()
                {
                    ID = 20,
                    CategoryId = 4,
                    Name = "Beef & Chips Pizza",
                    Price = 9.35
                },
                new Product()
                {
                    ID = 21,
                    CategoryId = 5,
                    Name = "Philadelphia With Salmon",
                    Price = 3.99
                },
                new Product()
                {
                    ID = 22,
                    CategoryId = 5,
                    Name = "Philadelphia With Tuna",
                    Price = 4.99
                },
                new Product()
                {
                    ID = 23,
                    CategoryId = 5,
                    Name = "Philadelphia Classic",
                    Price = 3.09
                },
                new Product()
                {
                    ID = 24,
                    CategoryId = 5,
                    Name = "Itachi",
                    Price = 3.59
                },
                new Product()
                {
                    ID = 25,
                    CategoryId = 5,
                    Name = "Yamamoto",
                    Price = 3.99
                },
                new Product()
                {
                    ID = 26,
                    CategoryId = 6,
                    Name = "California Club",
                    Price = 12.99
                },
                new Product()
                {
                    ID = 27,
                    CategoryId = 6,
                    Name = "Steak Sandwich",
                    Price = 18.99
                },
                new Product()
                {
                    ID = 28,
                    CategoryId = 6,
                    Name = "Sandwich Combo",
                    Price = 11.99
                },
                new Product()
                {
                    ID = 29,
                    CategoryId = 6,
                    Name = "Beef Sandwich",
                    Price = 14.59
                },
                new Product()
                {
                    ID = 30,
                    CategoryId = 6,
                    Name = "Philly Cheese Steak",
                    Price = 14.99
                },
                new Product()
                {
                    ID = 31,
                    CategoryId = 7,
                    Name = "Ice Cream Sundae",
                    Price = 5.99
                },
                new Product()
                {
                    ID = 32,
                    CategoryId = 7,
                    Name = "Ice Cream By The Scoop",
                    Price = 2.89
                },
                new Product()
                {
                    ID = 33,
                    CategoryId = 7,
                    Name = "Bread Pudding",
                    Price = 5.25
                },
                new Product()
                {
                    ID = 34,
                    CategoryId = 7,
                    Name = "Rootbeer Float",
                    Price = 5.99
                },
                new Product()
                {
                    ID = 35,
                    CategoryId = 7,
                    Name = "Banana Split",
                    Price = 10.99
                },
                new Product()
                {
                    ID = 36,
                    CategoryId = 8,
                    Name = "Pepsi Cola",
                    Price = 3.35
                },
                new Product()
                {
                    ID = 37,
                    CategoryId = 8,
                    Name = "Coca Cola",
                    Price = 3.05
                },
                new Product()
                {
                    ID = 38,
                    CategoryId = 8,
                    Name = "Coffee",
                    Price = 3.79
                },
                new Product()
                {
                    ID = 39,
                    CategoryId = 8,
                    Name = "Iced Tea",
                    Price = 3.79
                },
                new Product()
                {
                    ID = 40,
                    CategoryId = 8,
                    Name = "Orange Juice",
                    Price = 5.55
                },
            });
        }
    }
}