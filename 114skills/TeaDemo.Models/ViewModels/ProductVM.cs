using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaDemo.Models.ViewModels
{
    // ViewModel (視圖模型) 用於在產品相關視圖中傳遞數據
    public class ProductVM
    {
        public Product Product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        // 建構子，用於初始化默認值
        public ProductVM()
        {
            Product = new Product(); // 確保 Product 不為 null
            CategoryList = new List<SelectListItem>(); // 初始化為空清單，避免 NullReferenceException
        }
    }

}
