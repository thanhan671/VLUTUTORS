using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VLUTUTORS.Areas.Tutors.Controllers
{
    [Area("Tutors")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            int idGiaSu = (int)HttpContext.Session.GetInt32("IdGiaSu");
            string user = HttpContext.Session.GetString("LoginId");
            if (user == null || idGiaSu != 6)
            {
                return RedirectToAction("Login", "Accounts", new { area = "default" });
            }
            return View();
        }
    }
}
