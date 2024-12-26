using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaDemo.Models
{
    // 自定義應用程式用戶類，繼承自 ASP.NET Core 的 IdentityUser
    public class ApplicationUser : IdentityUser
    {
        // 使用者姓名，為必填欄位
        [Required] // DataAnnotations，指定此屬性為必填項
        public string Name { get; set; }

        // 使用者地址，為可選欄位
        public string Address { get; set; }
    }
}
