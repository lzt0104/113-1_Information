using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.DataAccess.Repository.IRepository.TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.Models;
using TeaDemo.Utility;

namespace _114skills.Areas.Admin.Controllers
{
    [Area("Admin")] // 指定此 Controller 屬於 Admin 區域
    [Authorize(Roles = SD.Role_Admin)] // 只有具備 Admin 角色的使用者可以存取此 Controller
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // 透過 Dependency Injection 傳入 IUnitOfWork，方便進行資料存取操作
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 列表頁面，顯示所有的分類資料
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        // 新增分類頁面
        public IActionResult Create()
        {
            return View();
        }

        // 新增分類操作
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            // 驗證規則：名稱不能與顯示順序相同
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "類別名稱不能跟顯示順序一致");
            }
            // 如果驗證通過，則新增分類
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj); // 新增資料至資料庫
                _unitOfWork.Save(); // 保存變更
                TempData["success"] = "類別新增成功"; // 用於顯示成功訊息
                return RedirectToAction("Index"); // 返回分類列表頁面
            }
            return View(); // 如果驗證失敗，返回新增頁面
        }

        // 編輯分類頁面
        public IActionResult Edit(int? id)
        {
            // 如果 ID 無效或為空，返回 404
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // 根據 ID 從資料庫取得對應的分類資料
            Category? categoryFromdb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromdb == null)
            {
                return NotFound(); // 如果分類不存在，返回 404
            }
            return View(categoryFromdb); // 返回編輯頁面
        }

        // 編輯分類操作
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            // 如果驗證通過，則更新分類
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj); // 更新資料庫中的分類資料
                _unitOfWork.Save(); // 保存變更
                return RedirectToAction("Index"); // 返回分類列表頁面
            }
            return View(); // 如果驗證失敗，返回編輯頁面
        }

        // 刪除分類頁面
        public IActionResult Delete(int? id)
        {
            // 如果 ID 無效或為空，返回 404
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // 根據 ID 從資料庫取得對應的分類資料
            Category categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound(); // 如果分類不存在，返回 404
            }
            return View(categoryFromDb); // 返回刪除確認頁面
        }

        // 刪除分類操作
        [HttpPost, ActionName("Delete")] // 指定此方法處理 Delete 請求
        public IActionResult DeletePOST(int? id)
        {
            // 根據 ID 從資料庫取得對應的分類資料
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound(); // 如果分類不存在，返回 404
            }
            _unitOfWork.Category.Remove(obj); // 刪除分類
            _unitOfWork.Save(); // 保存變更
            TempData["success"] = "類別刪除成功"; // 用於顯示成功訊息
            return RedirectToAction("Index"); // 返回分類列表頁面
        }
    }
}
