using First_Test_Wevb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace First_Test_Wevb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Team()
        {
            return View();
        }

        public IActionResult after()
        {
            return View();
        }

        public IActionResult LINECHAT()
        {
            return Redirect("https://line.me/ti/g2/StIeyzM_9apJ3yugTlInMPfhlxheD2X2-_xwyg?utm_source=invitation&utm_medium=link_copy&utm_campaign=default");
        }

        public IActionResult LowestStandard111()
        {
            return Redirect("https://onedrive.live.com/?authkey=%21AD8o8BNWBer%5FWDI&id=F73E5B38AE39F58D%2151153&cid=F73E5B38AE39F58D");
        }

        public IActionResult Opendata112()
        {
            return Redirect("https://onedrive.live.com/?authkey=%21AOLnw6JAI8NLKhA&id=F73E5B38AE39F58D%2151277&cid=F73E5B38AE39F58D");
        }

        public IActionResult Year111()
        {
            return View();
        }

        public IActionResult Year112()
        {
            return View();
        }

        public IActionResult Year113()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
