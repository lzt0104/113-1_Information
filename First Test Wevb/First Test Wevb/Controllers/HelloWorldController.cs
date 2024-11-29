using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using First_Test_Wevb.Models;
using System.Diagnostics;

namespace MvcMovie.Controllers;

public class HelloWorldController : Controller
{

    //public IActionResult Index()
    //{
    //    return View();
    //}

    public string HelloWrold()
    {
        return "This is my default action...";
    }

    //public IActionResult HelloWorld()
    //{
    //    return View();
    //}


    // 
    // GET: /HelloWorld/Welcome/ 
    // Requires using System.Text.Encodings.Web;
    public string Welcome(string name, int ID = 1)
    {
        return HtmlEncoder.Default.Encode($"Hello {name}, ID: {ID}");
    }

}

