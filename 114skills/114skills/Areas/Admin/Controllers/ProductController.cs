using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Reflection.Emit;
using TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.Models;
using TeaDemo.Models.ViewModels;
using TeaDemo.Utility;

namespace _114skills.Areas.Admin.Controllers
{
    [Area("Admin")] // 指定此 Controller 屬於 Admin 區域
    [Authorize(Roles = SD.Role_Admin)] // 只有具備 Admin 角色的使用者可以存取此 Controller
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // 建構子，注入資料存取單位 (UnitOfWork) 和網頁主機環境
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // 產品列表頁面
        public IActionResult Index()
        {
            // 獲取所有未被刪除的產品，並按名稱排序
            List<Product> objCategoryList = _unitOfWork.Product.GetAll(includeProperties: "Category")
                .Where(p => !p.IsDeleted)
                .OrderBy(p => p.Name)
                .ToList();

            return View(objCategoryList);
        }

        // 新增或編輯產品頁面
        public IActionResult Upsert(int? id)
        {
            // 建立產品視圖模型
            ProductVM productVM = new()
            {
                // 從分類表獲取所有分類並轉換為下拉選單
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product() // 預設為新產品
            };

            if (id == null || id == 0)
            {
                // 新增產品
                return View(productVM);
            }
            else
            {
                // 編輯產品，根據 ID 從資料庫載入資料
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }

        // 新增或編輯產品處理
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath; // 網站根目錄路徑

                // 處理圖片檔案上傳
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // 生成唯一檔名
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    // 如果已有圖片，則刪除舊圖片
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // 儲存新圖片
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName; // 設定圖片 URL
                }

                // 新增或更新產品
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product); // 新增產品
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product); // 更新產品
                }

                _unitOfWork.Save(); // 保存變更
                TempData["success"] = "產品新增成功"; // 設定成功訊息
                return RedirectToAction("Index");
            }
            else
            {
                // 如果驗證失敗，重新加載分類清單並返回頁面
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            }

            return View(productVM); // 返回 Upsert 頁面
        }

        #region API CALLS

        // API: 獲取所有產品（包含分類資訊）
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        // API: 刪除產品
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            // 根據 ID 獲取產品資料
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "刪除失敗" });
            }

            // 刪除產品圖片
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            // 刪除產品資料
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "刪除成功" });
        }

        #endregion

        // 搜尋功能 (範例代碼)
        // public IActionResult IndexSearching(string searchString)
        // {
        //     var products = _unitOfWork.Product.GetAll(); // 獲取所有產品
        //
        //     if (!string.IsNullOrEmpty(searchString))
        //     {
        //         products = products.Where(p => p.Name.ToUpper().Contains(searchString.ToUpper())); // 根據名稱搜尋產品
        //     }
        //
        //     return View("Index", products.ToList());
        // }

        // 軟刪除功能 (範例代碼)
        // [HttpPost, ActionName("Deleted")]
        // public IActionResult IsDeleted(int? id)
        // {
        //     if (id == null || id == 0)
        //     {
        //         return NotFound();
        //     }
        //
        //     var obj = _unitOfWork.Product.Get(u => u.Id == id);
        //     if (obj == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     // 設置 IsDeleted 為 true
        //     obj.IsDeleted = true;
        //
        //     // 更新資料庫
        //     _unitOfWork.Product.Update(obj);
        //     _unitOfWork.Save();
        //
        //     TempData["success"] = "產品已軟刪除";
        //     return RedirectToAction("Index");
        // }
    }
}
