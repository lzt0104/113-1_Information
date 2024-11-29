using System.ComponentModel.DataAnnotations;

namespace First_Test_Wevb.Models
{
    public class Members
    {
        [Key]
        public int Id { get; set; } // 主鍵

        [Required]
        [MaxLength(100)]
        public string Account { get; set; } // 帳號

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } // 姓名

        [Required]
        [MaxLength(100)]
        public string School { get; set; } // 學校

        [Required]
        public string Group { get; set; } // 群別

        [Required]
        public string Department { get; set; } // 科別

        [Required]
        [EmailAddress]
        public string Email { get; set; } // 電子郵件

        [Required]
        [MaxLength(100)]
        public string Password { get; set; } // 密碼

        [MaxLength(100)]
        public string Qualification { get; set; } // 資格

        [MaxLength(100)]
        public string OtherQualification { get; set; } // 其他資格

        public string StudentCardPath { get; set; } // 學生證檔案路徑
    }
}
