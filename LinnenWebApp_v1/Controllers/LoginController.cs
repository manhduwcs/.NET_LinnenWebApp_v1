using System.Net;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LinnenWebApp_v1.Models;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace LinnenWebApp_v1.Controllers
{
    public class LoginController : Controller
    {
        private string notificationString = "";
        // GET product
        public IActionResult Index()
        {
            ViewBag.notification = notificationString;
            return View();
        }

        [HttpPost]
        public IActionResult Login(User us)
        {
            if (us.Username == "manhduc" && us.Password == "manh")
            {
                notificationString = $"User : {us.Username} logged in successfully !";
                return RedirectToAction("Index");
            }

            notificationString = $"User : {us.Username} logged in failed !";
            return RedirectToAction("Index");
        }
    }
}