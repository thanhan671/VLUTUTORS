using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VLUTUTORS.Areas.Tutors.Controllers
{
    public class ManageWalletController : Controller
    {
        [Area("Tutors")]

        public IActionResult Index()
        {
            //string user = HttpContext.Session.GetString("LoginId");
            //if (user == null)
            //{
            //    return RedirectToAction("Login", "Accounts", new { area = "default" });
            //}
            return View();
        }
    }
}
