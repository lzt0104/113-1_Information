using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TeaDemo.DataAccess.Repository.IRepository;

namespace TeaDemo.DataAccess.Repository
{
    // 通用 Repository 實現，負責處理所有實體類型的通用數據操作
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db; // 資料庫上下文
        internal DbSet<T> dbSet; // Entity Framework Core 提供的 DbSet，用於操作資料表

        // 建構子，初始化資料庫上下文和 DbSet
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>(); // 獲取當前實體類型的 DbSet
            // 預載產品相關的資料（如果適用於 T = Product），確保相關的分類資訊被包括
            _db.Products.Include(u => u.Category).Include(u => u.CategoryId);
        }

        // 新增單一實體到資料庫
        public void Add(T entity)
        {
            dbSet.Add(entity); // 使用 EF Core 的 Add 方法新增資料
        }

        // 根據條件過濾並獲取單一實體
        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet; // 初始化查詢對象
            query = query.Where(filter); // 添加條件過濾
            if (!string.IsNullOrEmpty(includeProperties))
            {
                // 如果有 includeProperties，將相關實體包括進查詢中
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault(); // 返回第一個符合條件的實體或默認值
        }

        // 獲取所有實體的集合，可選包括相關的實體
        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet; // 初始化查詢對象
            if (!string.IsNullOrEmpty(includeProperties))
            {
                // 如果有 includeProperties，將相關實體包括進查詢中
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList(); // 返回所有符合條件的實體集合
        }

        // 刪除單一實體
        public void Remove(T entity)
        {
            dbSet.Remove(entity); // 使用 EF Core 的 Remove 方法移除資料
        }

        // 批量刪除實體
        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity); // 使用 EF Core 的 RemoveRange 方法移除多個資料
        }
    }
}
