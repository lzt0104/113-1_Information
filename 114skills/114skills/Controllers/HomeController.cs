using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TeaDemo.Models;

namespace _114skills.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // 用於記錄應用程式運行時的日誌

        // 建構子，注入 Logger，方便記錄日誌資訊
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // 首頁 (Index Action)
        public IActionResult Index()
        {
            // 返回首頁視圖
            return View();
        }

        // 分類頁面 (Category Action)
        public IActionResult Category()
        {
            // 返回分類頁面的視圖
            return View();
        }

        // 隱私政策頁面 (Privacy Action)
        public IActionResult Privacy()
        {
            // 返回隱私政策頁面的視圖
            return View();
        }

        // 錯誤處理頁面 (Error Action)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // 禁止對錯誤頁面進行緩存
        public IActionResult Error()
        {
            // 返回錯誤頁面，並包含錯誤的請求 ID（如果存在）
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
