using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.Models;
using TeaTimeDemo.DataAccess.Repository.IRepository;
using TeaTimeDemo.DataAccess.Repository;
using TeaDemo.DataAccess.Repository.IRepository.TeaDemo.DataAccess.Repository.IRepository;

namespace TeaDemo.DataAccess.Repository
{
    // UnitOfWork 設計模式的實現，統一管理多個 Repository 的生命週期和資料庫操作
    public class UnitOfWork : IUnitOfWork
    {
        // 應用程式的資料庫上下文 (Database Context) 引用
        private ApplicationDbContext _db;

        // 每個具體的 Repository，透過屬性公開
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }

        // 建構子，初始化所有 Repository 並注入資料庫上下文
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db); // 類別 Repository 的初始化
            Product = new ProductRepository(_db);   // 產品 Repository 的初始化
            ShoppingCart = new ShoppingCartRepository(_db); // 購物車 Repository 的初始化
            ApplicationUser = new ApplicationUserRepository(_db); // 使用者 Repository 的初始化
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
        }

        // Save 方法，統一保存所有更改到資料庫
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
