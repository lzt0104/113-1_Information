using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.DataAccess.Repository.IRepository.TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.Models;

namespace TeaTimeDemo.Areas.Customer.Controllers
{
    [Area("Customer")] // 指定此控制器屬於 "Customer" 區域
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // 用於記錄日誌的 Logger
        private readonly IUnitOfWork _unitOfWork; // 使用 UnitOfWork 管理 Repository 的操作

        // 建構子，注入 Logger 和 UnitOfWork
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // 首頁，顯示所有產品列表
        public IActionResult Index()
        {
            // 從資料庫中取得所有產品，並包含 Category 資料
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList);
        }

        // 顯示產品詳細資訊
        public IActionResult Details(int productId)
        {
            // 初始化購物車物件，包含產品資訊和初始數量
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = productId
            };
            return View(cart);
        }

        [HttpPost]
        [Authorize] // 限制此方法只能由已授權的使用者呼叫
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            // 取得目前使用者的 ID
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 將使用者 ID 設置到購物車物件中
            shoppingCart.ApplicationUserId = userId;

            // 檢查資料庫中是否已存在相同的購物車項目
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUser.Id == userId && u.ProductId == shoppingCart.ProductId && u.Ice == shoppingCart.Ice && u.Sweetness == shoppingCart.Sweetness);
            if (cartFromDb != null)
            {
                // 如果已存在，更新數量
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            else
            {
                // 如果不存在，新增購物車項目
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }

            // 顯示成功訊息
            TempData["success"] = "加入購物車成功！";
            _unitOfWork.Save(); // 儲存所有更改
            return RedirectToAction(nameof(Index)); // 返回首頁
        }

        // 隱私政策頁面
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // 錯誤頁面顯示
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
