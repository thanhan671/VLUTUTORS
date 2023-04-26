using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VLUTUTORS.Models;

namespace VLUTUTORS.Areas.Tutors.Controllers
{
    [Area("Tutors")]
    public class ManageWalletController : Controller
    {

        public async Task<IActionResult> Index()
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
