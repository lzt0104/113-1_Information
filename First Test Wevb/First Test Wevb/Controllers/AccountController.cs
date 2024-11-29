using First_Test_Wevb.Data;
using First_Test_Wevb.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace First_Test_Wevb.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Register Page
        public IActionResult Register()
        {
            return View(); // 確保返回 View，修正錯誤的 View() 調用。
        }

        // POST: Register Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Members member, IFormFile StudentCard)
        {
            if (ModelState.IsValid)
            {
                // 處理檔案上傳
                if (StudentCard != null && StudentCard.Length > 0)
                {
                    // 確保目錄存在
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    // 檔案路徑
                    var filePath = Path.Combine(uploadDir, StudentCard.FileName);

                    // 儲存檔案
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await StudentCard.CopyToAsync(stream);
                    }

                    // 儲存檔案相對路徑到資料庫
                    member.StudentCardPath = "/uploads/" + StudentCard.FileName;
                }

                // 儲存成員資訊到資料庫
                _context.Add(member);
                await _context.SaveChangesAsync();

                // 成功後導向首頁或其他頁面
                return RedirectToAction("Index", "Home");
            }

            // 若資料驗證失敗，返回原始註冊頁面
            return View(member);
        }
    }
}
