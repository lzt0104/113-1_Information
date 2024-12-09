using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeaDemo.DataAccess.Repository.IRepository;
using TeaDemo.Models;

namespace _114skills.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> objCategoryList = _unitOfWork.Product.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "產品新增成功";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromdb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromdb == null)
            {
                return NotFound();
            }
            return View(productFromdb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "產品編輯成功";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "產品刪除成功";
            return RedirectToAction("Index");
        }

        public IActionResult IndexSearching(string searchString)
        {
            var products = _unitOfWork.Product.GetAll(); // 獲取所有產品

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.ToUpper().Contains(searchString.ToUpper())); // 根據名稱搜尋產品
            }

            return View("Index", products.ToList());
        }

    }
}
