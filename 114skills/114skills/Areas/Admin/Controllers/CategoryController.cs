using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.DataAccess.Repository.IRepository.TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.Models;
using TeaDemo.Utility;

namespace _114skills.Areas.Admin.Controllers
{
    // 指定此 Controller 屬於 Admin 區域
    [Area("Admin")]
    // 限制只有擁有 Admin 角色的使用者可以存取此 Controller
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // 透過 Dependency Injection 傳入 IUnitOfWork，用於管理資料庫操作
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 顯示分類列表的頁面
        public IActionResult Index()
        {
            // 取得所有分類資料並轉換為 List
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            // 將分類列表傳遞至 View 並顯示
            return View(objCategoryList);
        }

        // 顯示新增分類的頁面
        public IActionResult Create()
        {
            return View();
        }

        // 新增分類的操作方法
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            // 驗證規則：類別名稱不能與顯示順序相同
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                // 在 ModelState 中新增自訂錯誤訊息
                ModelState.AddModelError("name", "類別名稱不能跟顯示順序一致");
            }

            // 如果 Model 驗證通過，執行新增分類操作
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj); // 新增分類資料
                _unitOfWork.Save(); // 保存變更至資料庫
                TempData["success"] = "類別新增成功"; // 設定成功訊息
                return RedirectToAction("Index"); // 重定向至分類列表頁面
            }
            // 如果驗證失敗，重新返回新增頁面
            return View();
        }

        // 顯示編輯分類的頁面
        public IActionResult Edit(int? id)
        {
            // 驗證 ID 是否有效，若無效返回 404 頁面
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // 根據 ID 從資料庫中取得對應的分類資料
            Category? categoryFromdb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromdb == null)
            {
                return NotFound(); // 如果資料不存在，返回 404 頁面
            }
            // 將分類資料傳遞至 View 並顯示
            return View(categoryFromdb);
        }

        // 編輯分類的操作方法
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            // 如果 Model 驗證通過，執行更新操作
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj); // 更新分類資料
                _unitOfWork.Save(); // 保存變更至資料庫
                TempData["success"] = "類別更新成功"; // 設定成功訊息
                return RedirectToAction("Index"); // 重定向至分類列表頁面
            }
            // 如果驗證失敗，重新返回編輯頁面
            return View();
        }

        // 顯示刪除分類的確認頁面
        public IActionResult Delete(int? id)
        {
            // 驗證 ID 是否有效，若無效返回 404 頁面
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // 根據 ID 從資料庫中取得對應的分類資料
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound(); // 如果資料不存在，返回 404 頁面
            }
            // 將分類資料傳遞至 View 並顯示
            return View(categoryFromDb);
        }

        // 刪除分類的操作方法
        [HttpPost, ActionName("Delete")] // 指定此方法處理 Delete 的 POST 請求
        public IActionResult DeletePOST(int? id)
        {
            // 根據 ID 從資料庫中取得對應的分類資料
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound(); // 如果資料不存在，返回 404 頁面
            }

            _unitOfWork.Category.Remove(obj); // 刪除分類資料
            _unitOfWork.Save(); // 保存變更至資料庫
            TempData["success"] = "類別刪除成功"; // 設定成功訊息
            return RedirectToAction("Index"); // 重定向至分類列表頁面
        }
    }
}
