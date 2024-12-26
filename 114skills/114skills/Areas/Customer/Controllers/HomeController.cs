using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.Models;

namespace TeaTimeDemo.Areas.Customer.Controllers
{
    [Area("Customer")] // 指定此 Controller 屬於 Customer 區域
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // 用於記錄應用程式日誌
        private readonly IUnitOfWork _unitOfWork; // 資料存取單元，負責與資料庫交互

        // 建構子，注入記錄器和 UnitOfWork
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // 首頁：顯示所有產品列表
        public IActionResult Index()
        {
            // 獲取所有產品，並包含分類資訊
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList); // 傳遞產品列表至 View
        }

        // 產品詳情頁
        public IActionResult Details(int productId)
        {
            // 根據產品 ID 獲取產品資訊，並包含分類資訊
            Product product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category");
            return View(product); // 傳遞產品資料至 View
        }

        // 下列為購物車相關功能（目前已註解），可根據需要啟用：
        /*
        // 產品詳情頁，包含購物車資訊
        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"), // 取得產品資訊
                Count = 1, // 預設數量為 1
                ProductId = productId // 設置產品 ID
            };
            return View(cart); // 傳遞購物車資訊至 View
        }

        // 新增至購物車功能（需要登入驗證）
        [HttpPost]
        [Authorize] // 此方法需要登入後才能存取
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            // 獲取當前登入使用者的 ID
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCart.ApplicationUserId = userId; // 將使用者 ID 設定至購物車

            // 檢查是否已有相同產品、冰量、甜度的購物車紀錄
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u =>
                u.ApplicationUser.Id == userId &&
                u.ProductId == shoppingCart.ProductId &&
                u.Ice == shoppingCart.Ice &&
                u.Sweetness == shoppingCart.Sweetness);

            if (cartFromDb != null)
            {
                // 如果購物車中已有相同的產品，則增加數量
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            else
            {
                // 如果購物車中沒有相同的產品，則新增紀錄
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }

            TempData["success"] = "加入購物車成功！"; // 顯示成功訊息
            _unitOfWork.Save(); // 保存變更至資料庫
            return RedirectToAction(nameof(Index)); // 返回首頁
        }
        */

        // 隱私權頁面
        public IActionResult Privacy()
        {
            return View(); // 返回隱私權頁面
        }

        // 錯誤頁面處理
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // 建立錯誤資訊模型，包含請求 ID
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
