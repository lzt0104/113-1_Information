using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeaDemo.Models
{
    // 表示分類的模型，用於存儲和處理分類的相關數據
    public class Category
    {
        // 主鍵，對應資料庫中的主鍵列
        [Key]
        public int Id { get; set; }

        // 類別名稱，必填，且最大長度為30
        [Required] // 指定此屬性為必填項
        [MaxLength(30)] // 限制最大長度為 30
        [DisplayName("類別名稱")] // 指定顯示名稱，用於前端顯示的標籤
        public string Name { get; set; }

        // 顯示順序，用於排序分類
        [DisplayName("顯示順序")] // 指定顯示名稱
        [Range(1, 100, ErrorMessage = "輸入範圍應該要在1-100之間")] // 限制值的範圍在 1 到 100 之間
        public int DisplayOrder { get; set; }
    }
}
