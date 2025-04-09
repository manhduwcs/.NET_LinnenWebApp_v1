using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LinnenWebApp_v1.Models;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using LinnenWebApp_v1.Controllers.utilities;

namespace LinnenWebApp_v1.Controllers;

public class HomeController : Controller
{
    // The starter page of the whole website
    private string connectionString;
    public HomeController(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    public IActionResult Index()
    {
        LoginUserCookieChecker cookieChecker = new LoginUserCookieChecker(connectionString);
        User loggedinUser = cookieChecker.CheckLoginUserViaCookie(HttpContext);

        if (loggedinUser.UserID != -1)
        {
            UserController.loggedInUser = loggedinUser;
        }

        ViewData["studentList"] = new string[]
        {
        "😀 Jack",
        "😄 Johnson",
        "😅 Alice",
        "😂 Bob",
        "😜 Charlie",
        "😎 Diana",  // New student
        "🤓 Eve",     // New student
        "😇 Frank"    // New student
        };
        return View();
    }

    [HttpPost]
    public IActionResult SubmitStudent(IFormCollection fr)
    {
        string studentName = fr["studentName"];
        TempData["studentName"] = studentName;
        return RedirectToAction("Index");
    }

    public IActionResult About(string name)
    {
        ViewData["name"] = name;
        TempData["shortName"] = "DM";
        ViewData["currentTime"] = DateTime.Now;
        ViewBag.currentTime = "fjksdlfjflsfj";
        return View("~/Views/About/About.cshtml");
    }
}
