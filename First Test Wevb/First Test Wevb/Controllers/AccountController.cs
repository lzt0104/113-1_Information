using First_Test_Wevb.Data;
using First_Test_Wevb.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace First_Test_Wevb.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Register Page
        public IActionResult Register()
        {
            return View(); // �T�O��^ View�A�ץ����~�� View() �եΡC
        }

        // POST: Register Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Members member, IFormFile StudentCard)
        {
            if (ModelState.IsValid)
            {
                // �B�z�ɮפW��
                if (StudentCard != null && StudentCard.Length > 0)
                {
                    // �T�O�ؿ��s�b
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    // �ɮ׸��|
                    var filePath = Path.Combine(uploadDir, StudentCard.FileName);

                    // �x�s�ɮ�
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await StudentCard.CopyToAsync(stream);
                    }

                    // �x�s�ɮ׬۹���|���Ʈw
                    member.StudentCardPath = "/uploads/" + StudentCard.FileName;
                }

                // �x�s������T���Ʈw
                _context.Add(member);
                await _context.SaveChangesAsync();

                // ���\��ɦV�����Ψ�L����
                return RedirectToAction("Index", "Home");
            }

            // �Y������ҥ��ѡA��^��l���U����
            return View(member);
        }
    }
}
