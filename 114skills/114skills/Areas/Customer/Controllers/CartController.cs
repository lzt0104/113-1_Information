using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.DataAccess.Repository.IRepository.TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.Models.ViewModels;

namespace _114skills.Areas.Customer.Controllers
{
    [Area("Customer")] // 指定此控制器屬於 "Customer" 區域
    [Authorize] // 要求使用者登入授權才能存取此控制器
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; // 依賴注入，存取資料層的單位工作模式
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; } // 綁定到 HTTP 請求的模型屬性

        // 建構函式，注入 IUnitOfWork 依賴
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 顯示購物車頁面
        public IActionResult Index()
        {
            // 獲取當前使用者的 ID
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 初始化 ShoppingCartVM 並從資料庫獲取購物車項目
            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(
                    u => u.ApplicationUserId == userId, // 篩選條件：目前使用者的購物車
                    includeProperties: "Product" // 包括相關的 Product 資料
                ),
                // 註解掉 OrderHeader 初始化，待未來需求啟用
                // OrderHeader = new()
            };

            // 計算購物車的總金額
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                ShoppingCartVM.OrderTotal += (cart.Product.Price * cart.Count);
            }

            return View(ShoppingCartVM); // 傳遞購物車資料給視圖
        }

        // 增加購物車中某項產品的數量
        public IActionResult Plus(int cartId)
        {
            // 從資料庫獲取購物車項目
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            // 增加產品數量
            cartFromDb.Count += 1;

            // 更新資料庫
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();

            // 重定向回 Index 頁面
            return RedirectToAction(nameof(Index));
        }

        // 減少購物車中某項產品的數量
        public IActionResult Minus(int cartId)
        {
            // 從資料庫獲取購物車項目
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            if (cartFromDb.Count <= 1)
            {
                // 如果數量為 1 或更少，從購物車中刪除此項目
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                // 減少產品數量
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            // 保存變更
            _unitOfWork.Save();

            // 重定向回 Index 頁面
            return RedirectToAction(nameof(Index));
        }

        // 從購物車中移除某項產品
        public IActionResult Remove(int cartId)
        {
            // 從資料庫獲取購物車項目
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            // 從資料庫中刪除此項目
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();

            // 重定向回 Index 頁面
            return RedirectToAction(nameof(Index));
        }
    }
}
