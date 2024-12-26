using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.Models;

namespace TeaDemo.DataAccess.Repository
{
    // 繼承通用 Repository 並實現分類特定的 Repository 接口
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db; // 資料庫上下文

        // 通過建構子將 ApplicationDbContext 傳遞給基類
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db; // 保存上下文，用於執行特定的操作
        }

        // 保存對資料庫的所有變更
        public void Save()
        {
            _db.SaveChanges(); // 使用 EF Core 提供的方法提交變更
        }

        // 更新分類
        public void Update(Category obj)
        {
            _db.Categories.Update(obj); // 使用 EF Core 的 Update 方法標記實體為更新狀態
        }
    }
}
