using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.DataAccess.Repository.IRepository.TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.Models;
using TeaDemo.Models.ViewModels;
using TeaDemo.Utility;

namespace _114skills.Areas.Customer.Controllers
{
    // 此控制器屬於 "Customer" 區域，僅限授權使用者存取
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; // 資料操作的單位工作模式
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; } // 綁定到 HTTP 請求模型的屬性

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

            // 初始化購物車視圖模型
            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(
                    u => u.ApplicationUserId == userId, // 僅獲取當前使用者的購物車項目
                    includeProperties: "Product" // 包括產品的相關資料
                ),
                OrderHeader = new()
            };

            // 計算購物車總金額
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Product.Price * cart.Count);
            }

            return View(ShoppingCartVM); // 返回購物車視圖
        }

        // 增加購物車中某項產品的數量
        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            cartFromDb.Count += 1; // 增加數量
            _unitOfWork.ShoppingCart.Update(cartFromDb); // 更新資料庫
            _unitOfWork.Save(); // 保存變更

            return RedirectToAction(nameof(Index)); // 返回購物車頁面
        }

        // 減少購物車中某項產品的數量
        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            if (cartFromDb.Count <= 1)
            {
                // 如果數量為 1，則從購物車中移除此項產品
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1; // 減少數量
                _unitOfWork.ShoppingCart.Update(cartFromDb); // 更新資料庫
            }

            _unitOfWork.Save(); // 保存變更

            return RedirectToAction(nameof(Index)); // 返回購物車頁面
        }

        // 從購物車中移除某項產品
        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            _unitOfWork.ShoppingCart.Remove(cartFromDb); // 從資料庫移除此項目
            _unitOfWork.Save(); // 保存變更

            return RedirectToAction(nameof(Index)); // 返回購物車頁面
        }

        // 顯示訂單摘要頁面
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            // 填充訂購人資訊
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.ApplicationUser.Address;

            // 計算總金額
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Product.Price * cart.Count);
            }

            return View(ShoppingCartVM); // 返回摘要頁面
        }

        // 提交訂單處理
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST(ShoppingCartVM shoppingCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");

            // 設定訂單資訊
            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            // 計算總金額
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Product.Price * cart.Count);
            }

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader); // 新增訂單頭部資訊
            _unitOfWork.Save();

            // 新增每個購物車項目的訂單詳情
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Ice = cart.Ice,
                    sweetness = cart.Sweetness,
                    Price = cart.Product.Price,
                    Count = cart.Count
                };

                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            // 重定向到訂單確認頁面
            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
        }

        // 訂單確認處理
        public IActionResult OrderConfirmation(int id)
        {
            // 根據訂單 ID 獲取訂單資訊
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");

            // 更新訂單狀態為 "Pending"
            _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusPending);

            // 清空當前使用者的購物車
            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(id); // 返回訂單確認頁面
        }
    }
}
