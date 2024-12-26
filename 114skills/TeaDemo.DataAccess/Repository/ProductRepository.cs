using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.Models;

namespace TeaDemo.DataAccess.Repository
{
    // 繼承通用 Repository 並實現產品特定的 Repository 接口
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db; // 資料庫上下文

        // 通過建構子將 ApplicationDbContext 傳遞給基類
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db; // 保存上下文，用於執行特定的操作
        }

        // 更新產品資訊
        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Size = obj.Size;
                objFromDb.Price = obj.Price;
                objFromDb.Description = obj.Description;
                objFromDb.Category = obj.Category;

                // 更新圖片 URL，並記錄日誌
                if (obj.ImageUrl != null && obj.ImageUrl != objFromDb.ImageUrl)
                {
                    Console.WriteLine($"Updating ImageUrl for ProductId {obj.Id}");
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
            else
            {
                Console.WriteLine($"Product with Id {obj.Id} not found.");
                throw new InvalidOperationException($"Cannot update non-existent product with Id {obj.Id}");
            }
        }
    }
}
