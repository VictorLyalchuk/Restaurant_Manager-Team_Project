using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Access_Entity.Migrations
{
    public partial class _08052023_Iniz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Waiters",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<double>(type: "float", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartWorkingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Raiting = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waiters", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WaiterId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    TableId = table.Column<int>(type: "int", nullable: false),
                    TotalSum = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Orders_Waiters_WaiterId",
                        column: x => x.WaiterId,
                        principalTable: "Waiters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WaiterId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tables_Waiters_WaiterId",
                        column: x => x.WaiterId,
                        principalTable: "Waiters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsOrders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsOrders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductsOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsOrders_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "ID", "Login", "Password" },
                values: new object[] { 1, "Admin", "1234" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Entrees" },
                    { 2, "Soup" },
                    { 3, "Salad" },
                    { 4, "Pizza" },
                    { 5, "Sushi" },
                    { 6, "Burgers" },
                    { 7, "Desserts" },
                    { 8, "Drinks" }
                });

            migrationBuilder.InsertData(
                table: "Waiters",
                columns: new[] { "ID", "BirthDate", "FirstName", "Raiting", "Salary", "StartWorkingDate", "SurName" },
                values: new object[,]
                {
                    { 1, new DateTime(1978, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sarah", 0, 7000.0, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Larson" },
                    { 2, new DateTime(1982, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adrianne", 0, 3000.0, new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Curry" },
                    { 3, new DateTime(1981, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Courtney", 0, 3000.0, new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yates" },
                    { 4, new DateTime(1981, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lindsey", 0, 3000.0, new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vuolo" },
                    { 5, new DateTime(1977, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jenna", 0, 3000.0, new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lewis" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "CategoryId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Butter Chicken", 15.300000000000001 },
                    { 2, 1, "Palak Paneer", 12.800000000000001 },
                    { 3, 1, "Potato With Chicken", 12.4 },
                    { 4, 1, "Jamaican Jerk Chicken", 17.5 },
                    { 5, 1, "Salmon Chowder", 22.0 },
                    { 6, 2, "Gazpacho Soup", 12.25 },
                    { 7, 2, "Harira Soup", 8.3000000000000007 },
                    { 8, 2, "Minestrone Soup", 15.5 },
                    { 9, 2, "Tom Yum Soup", 19.600000000000001 },
                    { 10, 2, "Chupe de Marisco Soup", 26.399999999999999 },
                    { 11, 3, "Caesar Salad", 12.199999999999999 },
                    { 12, 3, "Greek Salad", 15.0 },
                    { 13, 3, "Waldorf Salad", 11.4 },
                    { 14, 3, "Caprese Salad", 15.5 },
                    { 15, 3, "Olivier Salad", 11.199999999999999 },
                    { 16, 4, "Diablo Pizza", 9.5999999999999996 },
                    { 17, 4, "Pepperoni Pizza", 9.6999999999999993 },
                    { 18, 4, "Margherita Pizza", 8.5999999999999996 },
                    { 19, 4, "Carbonaro Pizza", 8.9000000000000004 },
                    { 20, 4, "Beef & Chips Pizza", 9.3499999999999996 },
                    { 21, 5, "Philadelphia With Salmon", 3.9900000000000002 },
                    { 22, 5, "Philadelphia With Tuna", 4.9900000000000002 },
                    { 23, 5, "Philadelphia Classic", 3.0899999999999999 },
                    { 24, 5, "Itachi", 3.5899999999999999 },
                    { 25, 5, "Yamamoto", 3.9900000000000002 },
                    { 26, 6, "California Club", 12.99 },
                    { 27, 6, "Steak Sandwich", 18.989999999999998 },
                    { 28, 6, "Sandwich Combo", 11.99 },
                    { 29, 6, "Beef Sandwich", 14.59 },
                    { 30, 6, "Philly Cheese Steak", 14.99 },
                    { 31, 7, "Ice Cream Sundae", 5.9900000000000002 },
                    { 32, 7, "Ice Cream By The Scoop", 2.8900000000000001 },
                    { 33, 7, "Bread Pudding", 5.25 },
                    { 34, 7, "Rootbeer Float", 5.9900000000000002 },
                    { 35, 7, "Banana Split", 10.99 },
                    { 36, 8, "Pepsi Cola", 3.3500000000000001 },
                    { 37, 8, "Coca Cola", 3.0499999999999998 },
                    { 38, 8, "Coffee", 3.79 },
                    { 39, 8, "Iced Tea", 3.79 },
                    { 40, 8, "Orange Juice", 5.5499999999999998 }
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "ID", "Active", "WaiterId" },
                values: new object[,]
                {
                    { 1, true, 1 },
                    { 2, true, 1 }
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "ID", "Active", "WaiterId" },
                values: new object[,]
                {
                    { 3, true, 1 },
                    { 4, true, 2 },
                    { 5, true, 2 },
                    { 6, true, 3 },
                    { 7, true, 3 },
                    { 8, true, 4 },
                    { 9, true, 4 },
                    { 10, true, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WaiterId",
                table: "Orders",
                column: "WaiterId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOrders_OrderId",
                table: "ProductsOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOrders_ProductId",
                table: "ProductsOrders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_WaiterId",
                table: "Tables",
                column: "WaiterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "ProductsOrders");

            migrationBuilder.DropTable(
                name: "Tables");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Waiters");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
