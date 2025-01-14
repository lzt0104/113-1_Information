using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.Models;

namespace TeaDemo.DataAccess.Repository
{
    // ShoppingCartRepository 類別實作 IShoppingCartRepository 定義的功能。
    // 此 Repository 專門處理 ShoppingCart 模型，並負責相關的資料庫操作。
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        // 應用程式的資料庫上下文 (Database Context) 引用。
        private ApplicationDbContext _db;

        // 建構子，初始化 Repository 並傳入資料庫上下文。
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        // Update 方法用於修改資料庫中的現有 ShoppingCart 物件。
        // 它使用 Entity Framework 的 Update 方法來追蹤物件的狀態並儲存更改。
        public void Update(ShoppingCart obj)
        {
            // 在資料庫上下文中更新 ShoppingCart 物件。
            // 確保在呼叫此方法之前，物件已被 DbContext 所追蹤。
            _db.ShoppingCarts.Update(obj);
        }
    }
}
