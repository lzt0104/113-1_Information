using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TeaDemo.DataAccess.Repository.IRepository
{
    // 通用 Repository 介面，定義了對資料庫進行操作的基本方法
    // T 是類型參數，限定為類別型別
    public interface IRepository<T> where T : class
    {
        // 取得所有符合條件的資料集合
        // filter: 可選的條件篩選 (Expression)
        // includeProperties: 要包含的相關資料 (如外鍵) 的屬性名稱，使用逗號分隔
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        // 根據條件取得單一資料
        // filter: 條件篩選 (必填)
        // includeProperties: 要包含的相關資料 (如外鍵) 的屬性名稱，使用逗號分隔
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);

        // 新增資料至資料庫
        // entity: 要新增的實體物件
        void Add(T entity);

        // 刪除單一資料
        // entity: 要刪除的實體物件
        void Remove(T entity);

        // 批量刪除資料
        // entity: 要刪除的實體物件集合
        void RemoveRange(IEnumerable<T> entity);
    }
}
