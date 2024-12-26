using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeaDemo.Models
{
    // 表示產品的模型，包含產品的屬性和相關關係
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "產品名稱為必填項")]
        public string Name { get; set; }

        [Required(ErrorMessage = "產品大小為必填項")]
        public string Size { get; set; }

        [Required(ErrorMessage = "產品價格為必填項")]
        [Range(1, 10000, ErrorMessage = "產品價格應在 1 到 10,000 之間")]
        public double Price { get; set; }

        [MaxLength(500, ErrorMessage = "描述不能超過 500 個字")]
        public string Description { get; set; }

        public bool IsDeleted { get; set; } // 軟刪除標記

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }
    }

}
