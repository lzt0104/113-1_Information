using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.DataAccess.Repository.IRepository.TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.Models;
using TeaDemo.Models.ViewModels;
using TeaDemo.Utility;

namespace _114skills.Areas.Admin.Controllers
{
    // 指定此 Controller 屬於 Admin 區域，並限制只有 Admin 角色的使用者可以存取
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // 透過建構子注入 UnitOfWork 和 WebHostEnvironment，用於資料操作和處理檔案上傳
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // 產品列表頁面
        public IActionResult Index()
        {
            // 取得所有產品資料（包含分類資訊），過濾已刪除的產品，並按名稱排序
            List<Product> objCategoryList = _unitOfWork.Product
                .GetAll(includeProperties: "Category")
                .Where(p => !p.IsDeleted)
                .OrderBy(p => p.Name)
                .ToList();

            return View(objCategoryList); // 傳遞產品資料至 View
        }

        // 新增或編輯產品頁面
        public IActionResult Upsert(int? id)
        {
            // 初始化產品視圖模型，包含產品資料和分類清單
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name, // 分類名稱
                    Value = u.Id.ToString() // 分類 ID
                }),
                Product = new Product() // 預設為新增產品
            };

            if (id == null || id == 0)
            {
                // 如果 ID 為空或 0，則為新增模式
                return View(productVM);
            }
            else
            {
                // 編輯模式：根據 ID 取得對應的產品資料
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                if (productVM.Product == null)
                {
                    return NotFound(); // 若找不到產品資料，返回 404
                }
                return View(productVM);
            }
        }

        // 新增或編輯產品處理
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath; // 取得網站根目錄路徑

                // 處理檔案上傳
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // 生成唯一檔案名稱
                    string productPath = Path.Combine(wwwRootPath, @"images\product"); // 定義圖片儲存路徑

                    // 刪除舊圖片（如果存在）
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // 儲存新圖片到伺服器
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName; // 更新圖片路徑
                }

                // 判斷是新增還是更新產品
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product); // 新增產品
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product); // 更新產品
                }

                _unitOfWork.Save(); // 保存變更至資料庫
                TempData["success"] = "產品新增成功"; // 設定成功訊息
                return RedirectToAction("Index"); // 返回產品列表頁面
            }
            else
            {
                // 如果驗證失敗，重新加載分類清單以顯示在下拉選單中
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            }

            return View(productVM); // 返回原本的 Upsert 頁面
        }

        #region API CALLS

        // API: 獲取所有產品資料，包含分類資訊
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product
                .GetAll(includeProperties: "Category")
                .ToList();

            return Json(new { data = objProductList }); // 以 JSON 格式返回資料
        }

        // API: 刪除產品
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "刪除失敗，無效的 ID" });
            }

            // 根據 ID 取得產品資料
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "刪除失敗，產品不存在" });
            }

            // 刪除圖片檔案（如果存在）
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            // 刪除產品資料
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "刪除成功" }); // 返回成功訊息
        }

        #endregion
    }
}
