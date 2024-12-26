using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeaDemo.Models;

namespace TeaDemo.DataAccess
{
    // 繼承 IdentityDbContext，為應用程式提供身份驗證和授權的功能
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        // 通過建構子傳遞 DbContext 配置選項
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // 定義資料庫表對應的 DbSet
        public DbSet<Category> Categories { get; set; } // 分類表
        public DbSet<Product> Products { get; set; } // 產品表
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } // 自定義的使用者表（擴展 IdentityUser）

        // 配置模型行為，例如資料的種子（預設值）和關係
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 必須呼叫基類的 OnModelCreating，確保 Identity 的模型配置被正確應用
            base.OnModelCreating(modelBuilder);

            // 為 Category 表定義種子數據（預設值）
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "茶飲", DisplayOrder = 1 },
                new Category { Id = 2, Name = "水果茶", DisplayOrder = 2 },
                new Category { Id = 3, Name = "咖啡", DisplayOrder = 3 });

            /*
            如果需要初始化 Product 表的種子數據，以下是範例（目前被註解）：
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Name = "水果茶", // 產品名稱
                    Size = "大杯", // 產品大小
                    Description = "台灣在地水果茶", // 產品描述
                    Price = 60, // 價格
                    CategoryId = 1, // 對應的分類 ID
                    ImageUrl = "" // 圖片路徑
                },
                new Product
                {
                    Name = "鐵觀音",
                    Size = "中杯",
                    Description = "人生的味道",
                    Price = 35,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Name = "美式咖啡",
                    Size = "中杯",
                    Description = "休閒時光",
                    Price = 60,
                    CategoryId = 3,
                    ImageUrl = ""
                });
            */
        }
    }
}
