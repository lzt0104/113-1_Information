using System;
using System.Collections.Generic;

namespace TeaDemo.Models.ViewModels
{
    /// ViewModel 用於表示購物車相關的頁面數據
    public class ShoppingCartVM
    {
        /// 儲存購物車項目的集合
        /// 每個項目代表使用者加入購物車的產品及其數量
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }

        /// 購物車的總金額
        /// 計算方式為各項商品的價格乘以數量後的總和
        // public double OrderTotal { get; set; }
        public OrderHeader OrderHeader { get; set; }

        // 以下屬性為未來擴展的預留部分，當需要管理訂單標頭時可啟用
        // public required object OrderHeader { get; set; }
    }
}
