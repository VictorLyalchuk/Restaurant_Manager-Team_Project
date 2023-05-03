using Data_Access_Entity.Entities;
using Data_Access_Entity.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Entity
{
    public class RestaurantContext : DbContext
    {
        public RestaurantContext()
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOrder> ProductsOrders { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Waiter> Waiters { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer($@"workstation id=Restaurant.mssql.somee.com;packet size=4096;user id=Restaurant_Team_SQLLogin_1;pwd=hkn78544lf;data source=Restaurant.mssql.somee.com;persist security info=False;initial catalog=Restaurant");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.SeedAdmins();
            modelBuilder.SeedCategories();
            modelBuilder.SeedProducts();
            modelBuilder.SeedWaiters();
            modelBuilder.SeedTables();
            modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);
            modelBuilder.Entity<ProductOrder>().HasKey(po => new {po.OrderId, po.ProductId});
            modelBuilder.Entity<ProductOrder>().HasOne(p => p.Product).WithMany(p => p.ProductsOrders).HasForeignKey(p => p.ProductId);
            modelBuilder.Entity<ProductOrder>().HasOne(o => o.Order).WithMany(o => o.ProductsOrders).HasForeignKey(o => o.OrderId);
            modelBuilder.Entity<Order>().HasOne(o => o.Waiter).WithMany(w => w.Orders).HasForeignKey(o => o.WaiterId);
            modelBuilder.Entity<Table>().HasOne(t => t.Waiter).WithMany(w => w.Tables).HasForeignKey(t => t.WaiterId);
        }
    }
}