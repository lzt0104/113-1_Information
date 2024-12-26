using Microsoft.AspNetCore.Mvc;
using TeaDemo.DataAccess;
using TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.Models;

namespace _114skills.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        // 建構子注入 CategoryRepository，用於操作分類資料
        public CategoryController(ICategoryRepository db)
        {
            _categoryRepo = db;
        }

        // 首頁：顯示所有分類的列表
        public IActionResult Index()
        {
            // 從資料庫中獲取所有分類，並轉換為 List
            List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
            return View(objCategoryList); // 將分類列表傳遞到視圖
        }

        // 新增分類頁面
        public IActionResult Create()
        {
            return View(); // 返回空白的新增頁面
        }

        // 處理新增分類的 POST 請求
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            // 驗證：分類名稱不能與顯示順序相同
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "類別名稱不能跟顯示順序一致");
            }

            // 如果模型驗證成功，新增分類
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(obj); // 新增資料到資料庫
                _categoryRepo.Save(); // 保存變更
                TempData["success"] = "類別新增成功"; // 設置成功訊息
                return RedirectToAction("Index"); // 返回分類列表頁面
            }
            return View(); // 如果驗證失敗，返回新增頁面
        }

        // 編輯分類頁面
        public IActionResult Edit(int? id)
        {
            // 檢查 ID 是否為空或無效
            if (id == null || id == 0)
            {
                return NotFound(); // 返回 404
            }

            // 根據 ID 獲取分類資料
            Category? categoryFromdb = _categoryRepo.Get(u => u.Id == id);
            if (categoryFromdb == null)
            {
                return NotFound(); // 如果分類不存在，返回 404
            }

            return View(categoryFromdb); // 返回編輯頁面並傳遞分類資料
        }

        // 處理編輯分類的 POST 請求
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            // 如果模型驗證成功，更新分類
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj); // 更新資料庫中的分類資料
                _categoryRepo.Save(); // 保存變更
                return RedirectToAction("Index"); // 返回分類列表頁面
            }
            return View(); // 如果驗證失敗，返回編輯頁面
        }

        // 刪除分類頁面
        public IActionResult Delete(int? id)
        {
            // 檢查 ID 是否為空或無效
            if (id == null || id == 0)
            {
                return NotFound(); // 返回 404
            }

            // 根據 ID 獲取分類資料
            Category categoryFromDb = _categoryRepo.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound(); // 如果分類不存在，返回 404
            }

            return View(categoryFromDb); // 返回刪除確認頁面並傳遞分類資料
        }

        // 處理刪除分類的 POST 請求
        [HttpPost, ActionName("Delete")] // 指定處理 Delete 請求的方法
        public IActionResult DeletePOST(int? id)
        {
            // 根據 ID 獲取分類資料
            Category? obj = _categoryRepo.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound(); // 如果分類不存在，返回 404
            }

            _categoryRepo.Remove(obj); // 從資料庫刪除分類
            _categoryRepo.Save(); // 保存變更
            return RedirectToAction("Index"); // 返回分類列表頁面
        }
    }
}
