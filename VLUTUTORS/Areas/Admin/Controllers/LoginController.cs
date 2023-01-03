using System.Threading.Tasks;
using VLUTUTORS.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using VLUTUTORS.Security;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        UserManager _userManager;
        private CP25Team01Context db = new CP25Team01Context();

        public LoginController(UserManager userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            Taikhoanadmin taikhoanadmin = new Taikhoanadmin();
            return View(taikhoanadmin);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind(include: "TaiKhoan, MatKhau")] Taikhoanadmin taikhoanadmin)
        {
            string taiKhoan = taikhoanadmin.TaiKhoan;
            string password = taikhoanadmin.MatKhau;
            try
            {
                _userManager.SignIn(this.HttpContext, taiKhoan, password);

                var admin = db.Taikhoanadmins.Where(acc => acc.TaiKhoan.Equals(taiKhoan.Trim())).FirstOrDefault();
                if (admin != null && admin.Id > 0)
                {
                    HttpContext.Session.SetInt32("LoginId", admin.Id);
                    HttpContext.Session.SetInt32("IdQuyen", admin.IdQuyen);
                    HttpContext.Session.SetString("loginName", admin.TaiKhoan);
                    HttpContext.Session.SetString("SessionInfo", JsonConvert.SerializeObject(admin));
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View();
            }

        }
        public IActionResult Logout()
        {
            try
            {
                _userManager.SignOut(this.HttpContext);
                HttpContext.Session.Clear();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
