using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TeaDemo.DataAccess.Repository.IRepository;

namespace TeaDemo.DataAccess.Repository
{
    /// <summary>
    /// 泛型 Repository 類別，用於提供資料存取的通用方法
    /// 適用於任何類型的實體 T
    /// </summary>
    /// <typeparam name="T">資料模型類型</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db; // 資料庫上下文
        internal DbSet<T> dbSet; // 內部的 DbSet，用於操作資料


        /// 建構函式，初始化資料庫上下文並設定 DbSet
        /// <param name="db">注入的 ApplicationDbContext</param>
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();

            // 預先載入 Products 表中的 Category 和 CategoryId 屬性（僅適用於 Product）
            // 若有其他實體需要 Include，應在具體 Repository 中覆寫
            _db.Products.Include(u => u.Category).Include(u => u.CategoryId);
        }


        /// 新增一個實體到資料庫

        /// <param name="entity">要新增的實體</param>
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }


        /// 根據條件獲取單一實體，並可選擇包含關聯屬性

        /// <param name="filter">條件篩選表達式</param>
        /// <param name="includeProperties">以逗號分隔的關聯屬性名稱</param>
        /// <returns>匹配條件的第一個實體，若無則為 null</returns>
        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            // 根據篩選條件篩選資料
            query = query.Where(filter);

            // 動態加入指定的關聯屬性
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault(); // 返回第一個匹配項目
        }


        /// 獲取符合條件的所有實體，並可選擇包含關聯屬性

        /// <param name="filter">條件篩選表達式（可選）</param>
        /// <param name="includeProperties">以逗號分隔的關聯屬性名稱（可選）</param>
        /// <returns>符合條件的實體集合</returns>
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            // 根據篩選條件篩選資料（若有）
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // 動態加入指定的關聯屬性
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.ToList(); // 返回結果作為列表
        }


        /// 刪除一個實體

        /// <param name="entity">要刪除的實體</param>
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }


        /// 刪除一個實體集合

        /// <param name="entity">要刪除的實體集合</param>
        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
