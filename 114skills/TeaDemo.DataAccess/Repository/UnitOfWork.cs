using Microsoft.EntityFrameworkCore;
using System;
using TeaDemo.DataAccess.Repository.IRepository;

namespace TeaDemo.DataAccess.Repository
{
    // UnitOfWork 設計模式的實現，統一管理多個 Repository 的生命週期和資料庫操作
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db; // 資料庫上下文

        // 屬性，提供對不同實體的 Repository 訪問
        public ICategoryRepository Category { get; private set; } // 分類相關的 Repository
        public IProductRepository Product { get; private set; } // 產品相關的 Repository

        // 建構子，初始化資料庫上下文和各個 Repository
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            // 初始化各個 Repository，並傳入同一個資料庫上下文
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
        }

        // 保存所有更改到資料庫
        public void Save()
        {
            try
            {
                _db.SaveChanges(); // 提交所有更改
            }
            catch (DbUpdateException ex)
            {
                // 記錄異常資訊（假設有日誌系統）
                Console.WriteLine($"Error saving changes: {ex.Message}");
                throw; // 向上拋出異常
            }
        }

        // 如果需要事務支持，可以考慮加入以下方法：
        public void BeginTransaction()
        {
            _db.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _db.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _db.Database.RollbackTransaction();
        }

    }
}
