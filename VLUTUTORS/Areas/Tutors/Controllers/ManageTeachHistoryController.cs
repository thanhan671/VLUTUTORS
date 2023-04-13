using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VLUTUTORS.Models;

namespace VLUTUTORS.Areas.Tutors.Controllers

{
    [Area("Tutors")]
    public class ManageTeachHistoryController : Controller
    {
        private CP25Team01Context _db = new CP25Team01Context();
        public IActionResult Index()
        {
            string user = HttpContext.Session.GetString("LoginId");
            if (user == null)
            {
                return RedirectToAction("Login", "Accounts", new { area = "default" });
            }
            int checkUser = (int)_db.Taikhoannguoidungs.Where(m => m.Id == HttpContext.Session.GetInt32("LoginId")).First().IdxetDuyet;
            if (checkUser == 5)
            {
                return View();
            }
            return RedirectToAction("Login", "Accounts", new { area = "default" });
        }
    }
}
