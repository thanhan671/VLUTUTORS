using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;
namespace VLUTUTORS.Controllers
{
    public class AccountsController : Controller
    {
        private CP25Team01Context db = new CP25Team01Context();
        private Func<bool, IActionResult> _loginSuccessCallback;

        public IActionResult Login(Taikhoannguoidung taikhoannguoidung)
        {
            string email = taikhoannguoidung.Email;
            string password = taikhoannguoidung.MatKhau;

            _loginSuccessCallback = LoginSuccessCall;
            using(db)
            {
                var checkAccount = db.Taikhoannguoidungs.Where(acc => acc.Email.Equals(email.Trim())).FirstOrDefault();
                if (checkAccount == null)
                {
                    return View();
                }

                if(password.Trim().Equals(checkAccount.MatKhau.Trim()))
                {
                    _loginSuccessCallback.Invoke(true);
                }
            }

            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        private IActionResult LoginSuccessCall(bool status)
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        
    }
}
