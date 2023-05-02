﻿// <auto-generated />
using System;
using Data_Access_Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data_Access_Entity.Migrations
{
    [DbContext(typeof(RestaurantContext))]
    partial class RestaurantContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Data_Access_Entity.Entities.Admin", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Admins");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Login = "Admin",
                            Password = "1234"
                        });
                });

            modelBuilder.Entity("Data_Access_Entity.Entities.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "Entrees"
                        },
                        new
                        {
                            ID = 2,
                            Name = "Soup"
                        },
                        new
                        {
                            ID = 3,
                            Name = "Salad"
                        },
                        new
                        {
                            ID = 4,
                            Name = "Pizza"
                        },
                        new
                        {
                            ID = 5,
                            Name = "Sushi"
                        },
                        new
                        {
                            ID = 6,
                            Name = "Burgers"
                        },
                        new
                        {
                            ID = 7,
                            Name = "Desserts"
                        },
                        new
                        {
                            ID = 8,
                            Name = "Drinks"
                        });
                });

            modelBuilder.Entity("Data_Access_Entity.Entities.Order", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TableId")
                        .HasColumnType("int");

                    b.Property<int>("WaiterId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("WaiterId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Data_Access_Entity.Entities.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("ID");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            CategoryId = 1,
                            Name = "Butter Chicken",
                            Price = 15.300000000000001
                        },
                        new
                        {
                            ID = 2,
                            CategoryId = 1,
                            Name = "Palak Paneer",
                            Price = 12.800000000000001
                        },
                        new
                        {
                            ID = 3,
                            CategoryId = 1,
                            Name = "Potato With Chicken",
                            Price = 12.4
                        },
                        new
                        {
                            ID = 4,
                            CategoryId = 1,
                            Name = "Jamaican Jerk Chicken",
                            Price = 17.5
                        },
                        new
                        {
                            ID = 5,
                            CategoryId = 1,
                            Name = "Salmon Chowder",
                            Price = 22.0
                        },
                        new
                        {
                            ID = 6,
                            CategoryId = 2,
                            Name = "Gazpacho Soup",
                            Price = 12.25
                        },
                        new
                        {
                            ID = 7,
                            CategoryId = 2,
                            Name = "Harira Soup",
                            Price = 8.3000000000000007
                        },
                        new
                        {
                            ID = 8,
                            CategoryId = 2,
                            Name = "Minestrone Soup",
                            Price = 15.5
                        },
                        new
                        {
                            ID = 9,
                            CategoryId = 2,
                            Name = "Tom Yum Soup",
                            Price = 19.600000000000001
                        },
                        new
                        {
                            ID = 10,
                            CategoryId = 2,
                            Name = "Chupe de Marisco Soup",
                            Price = 26.399999999999999
                        },
                        new
                        {
                            ID = 11,
                            CategoryId = 3,
                            Name = "Caprese Salad",
                            Price = 12.199999999999999
                        },
                        new
                        {
                            ID = 12,
                            CategoryId = 3,
                            Name = "Greek Salad",
                            Price = 15.0
                        },
                        new
                        {
                            ID = 13,
                            CategoryId = 3,
                            Name = "Waldorf Salad",
                            Price = 11.4
                        },
                        new
                        {
                            ID = 14,
                            CategoryId = 3,
                            Name = "Caprese Salad",
                            Price = 15.5
                        },
                        new
                        {
                            ID = 15,
                            CategoryId = 3,
                            Name = "Olivier Salad",
                            Price = 11.199999999999999
                        },
                        new
                        {
                            ID = 16,
                            CategoryId = 4,
                            Name = "Diablo Pizza",
                            Price = 9.5999999999999996
                        },
                        new
                        {
                            ID = 17,
                            CategoryId = 4,
                            Name = "Pepperoni Pizza",
                            Price = 9.6999999999999993
                        },
                        new
                        {
                            ID = 18,
                            CategoryId = 4,
                            Name = "Margherita Pizza",
                            Price = 8.5999999999999996
                        },
                        new
                        {
                            ID = 19,
                            CategoryId = 4,
                            Name = "Carbonaro Pizza",
                            Price = 8.9000000000000004
                        },
                        new
                        {
                            ID = 20,
                            CategoryId = 4,
                            Name = "Beef & Chips Pizza",
                            Price = 9.3499999999999996
                        },
                        new
                        {
                            ID = 21,
                            CategoryId = 5,
                            Name = "Philadelphia With Salmon",
                            Price = 3.9900000000000002
                        },
                        new
                        {
                            ID = 22,
                            CategoryId = 5,
                            Name = "Philadelphia With Tuna",
                            Price = 4.9900000000000002
                        },
                        new
                        {
                            ID = 23,
                            CategoryId = 5,
                            Name = "Philadelphia Classic",
                            Price = 3.0899999999999999
                        },
                        new
                        {
                            ID = 24,
                            CategoryId = 5,
                            Name = "Itachi",
                            Price = 3.5899999999999999
                        },
                        new
                        {
                            ID = 25,
                            CategoryId = 5,
                            Name = "Yamamoto",
                            Price = 3.9900000000000002
                        },
                        new
                        {
                            ID = 26,
                            CategoryId = 6,
                            Name = "California Club",
                            Price = 12.99
                        },
                        new
                        {
                            ID = 27,
                            CategoryId = 6,
                            Name = "Steak Sandwich",
                            Price = 18.989999999999998
                        },
                        new
                        {
                            ID = 28,
                            CategoryId = 6,
                            Name = "Sandwich Combo",
                            Price = 11.99
                        },
                        new
                        {
                            ID = 29,
                            CategoryId = 6,
                            Name = "Beef Sandwich",
                            Price = 14.59
                        },
                        new
                        {
                            ID = 30,
                            CategoryId = 6,
                            Name = "Philly Cheese Steak",
                            Price = 14.99
                        },
                        new
                        {
                            ID = 31,
                            CategoryId = 7,
                            Name = "Ice Cream Sundae",
                            Price = 5.9900000000000002
                        },
                        new
                        {
                            ID = 32,
                            CategoryId = 7,
                            Name = "Ice Cream By The Scoop",
                            Price = 2.8900000000000001
                        },
                        new
                        {
                            ID = 33,
                            CategoryId = 7,
                            Name = "Bread Pudding",
                            Price = 5.25
                        },
                        new
                        {
                            ID = 34,
                            CategoryId = 7,
                            Name = "Rootbeer Float",
                            Price = 5.9900000000000002
                        },
                        new
                        {
                            ID = 35,
                            CategoryId = 7,
                            Name = "Banana Split",
                            Price = 10.99
                        },
                        new
                        {
                            ID = 36,
                            CategoryId = 8,
                            Name = "Pepsi Cola",
                            Price = 3.3500000000000001
                        },
                        new
                        {
                            ID = 37,
                            CategoryId = 8,
                            Name = "Coca Cola",
                            Price = 3.0499999999999998
                        },
                        new
                        {
                            ID = 38,
                            CategoryId = 8,
                            Name = "Coffee",
                            Price = 3.79
                        },
                        new
                        {
                            ID = 39,
                            CategoryId = 8,
                            Name = "Iced Tea",
                            Price = 3.79
                        },
                        new
                        {
                            ID = 40,
                            CategoryId = 8,
                            Name = "Orange Juice",
                            Price = 5.5499999999999998
                        });
                });

            modelBuilder.Entity("Data_Access_Entity.Entities.ProductOrder", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductsOrders");
                });

            modelBuilder.Entity("Data_Access_Entity.Entities.Table", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("WaiterId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("WaiterId");

                    b.ToTable("Tables");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Active = true,
                            WaiterId = 1
                        },
                        new
                        {
                            ID = 2,
                            Active = true,
                            WaiterId = 1
                        },
                        new
                        {
                            ID = 3,
                            Active = true,
                            WaiterId = 1
                        },
                        new
                        {
                            ID = 4,
                            Active = true,
                            WaiterId = 2
                        },
                        new
                        {
                            ID = 5,
                            Active = true,
                            WaiterId = 2
                        },
                        new
                        {
                            ID = 6,
                            Active = true,
                            WaiterId = 3
                        },
                        new
                        {
                            ID = 7,
                            Active = true,
                            WaiterId = 3
                        },
                        new
                        {
                            ID = 8,
                            Active = true,
                            WaiterId = 4
                        },
                        new
                        {
                            ID = 9,
                            Active = true,
                            WaiterId = 4
                        },
                        new
                        {
                            ID = 10,
                            Active = true,
                            WaiterId = 5
                        });
                });

            modelBuilder.Entity("Data_Access_Entity.Entities.Waiter", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Raiting")
                        .HasColumnType("int");

                    b.Property<double>("Salary")
                        .HasColumnType("float");

                    b.Property<DateTime>("StartWorkingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SurName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Waiters");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            BirthDate = new DateTime(1978, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Sarah",
                            Raiting = 0,
                            Salary = 7000.0,
                            StartWorkingDate = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SurName = "Larson"
                        },
                        new
                        {
                            ID = 2,
                            BirthDate = new DateTime(1982, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Adrianne",
                            Raiting = 0,
                            Salary = 3000.0,
                            StartWorkingDate = new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SurName = "Curry"
                        },
                        new
                        {
                            ID = 3,
                            BirthDate = new DateTime(1981, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Courtney",
                            Raiting = 0,
                            Salary = 3000.0,
                            StartWorkingDate = new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SurName = "Yates"
                        },
                        new
                        {
                            ID = 4,
                            BirthDate = new DateTime(1981, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Lindsey",
                            Raiting = 0,
                            Salary = 3000.0,
                            StartWorkingDate = new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SurName = "Vuolo"
                        },
                        new
                        {
                            ID = 5,
                            BirthDate = new DateTime(1977, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Jenna",
                            Raiting = 0,
                            Salary = 3000.0,
                            StartWorkingDate = new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SurName = "Lewis"
                        });
                });

            modelBuilder.Entity("Data_Access_Entity.Entities.Order", b =>
                {
                    b.HasOne("Data_Access_Entity.Entities.Waiter", "Waiter")
                        .WithMany("Orders")
                        .HasForeignKey("WaiterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Waiter");
                });

            modelBuilder.Entity("Data_Access_Entity.Entities.Product", b =>
                {
                    b.HasOne("Data_Access_Entity.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Data_Access_Entity.Entities.ProductOrder", b =>
                {
                    b.HasOne("Data_Access_Entity.Entities.Order", "Order")
                        .WithMany("ProductsOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data_Access_Entity.Entities.Product", "Product")
                        .WithMany("ProductsOrders")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Data_Access_Entity.Entities.Table", b =>
                {
                    b.HasOne("Data_Access_Entity.Entities.Waiter", "Waiter")
                        .WithMany("Tables")
                        .HasForeignKey("WaiterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Waiter");
                });

            modelBuilder.Entity("Data_Access_Entity.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Data_Access_Entity.Entities.Order", b =>
                {
                    b.Navigation("ProductsOrders");
                });

            modelBuilder.Entity("Data_Access_Entity.Entities.Product", b =>
                {
                    b.Navigation("ProductsOrders");
                });

            modelBuilder.Entity("Data_Access_Entity.Entities.Waiter", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Tables");
                });
#pragma warning restore 612, 618
        }
    }
}
