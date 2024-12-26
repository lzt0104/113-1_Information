using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaDemo.Utility
{
    // 自定義的電子郵件發送器類，實現 IEmailSender 接口
    public class EmailSender : IEmailSender
    {
        // 實現 IEmailSender 的 SendEmailAsync 方法，用於發送電子郵件
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // 模擬電子郵件發送，實際上只是將郵件內容輸出到控制台
            Console.WriteLine($"Sending email to {email} with subject {subject}");

            // 完成異步操作的空任務，模擬成功發送
            return Task.CompletedTask;
        }
    }
}
