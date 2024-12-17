using Microsoft.EntityFrameworkCore;
using TeaDemo.Models;

namespace TeaDemo.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "茶飲", DisplayOrder = 1 },
                new Category { Id = 2, Name = "水果茶", DisplayOrder = 2 },
                new Category { Id = 3, Name = "咖啡", DisplayOrder = 3 });
            //modelBuilder.Entity<Product>().HasData(
            //    new Product
            //    {
            //        Name = "水果茶",
            //        Size = "大杯",
            //        Description = "台灣在地水果茶",
            //        Price = 60,
            //        CategoryId = 1,
            //        ImageUrl = ""
            //    },
            //    new Product
            //    {
            //        Name = "鐵觀音",
            //        Size = "中杯",
            //        Description = "人生的味道",
            //        Price = 35,
            //        CategoryId = 2,
            //        ImageUrl = ""
            //    },
            //    new Product
            //    {
            //        Name = "美式咖啡",
            //        Size = "中杯",
            //        Description = "休閒時光",
            //        Price = 60,
            //        CategoryId = 3,
            //        ImageUrl = ""
            //    }
            //    );
        }
        
    }
}
