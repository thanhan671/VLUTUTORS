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

        public IActionResult Login()
        {
            Taikhoannguoidung _taikhoannguoidung = new Taikhoannguoidung();
            return View(_taikhoannguoidung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind(include:"Email, MatKhau")] Taikhoannguoidung taikhoannguoidung)
        {
            string email = taikhoannguoidung.Email;
            string password = taikhoannguoidung.MatKhau;
            _loginSuccessCallback = LoginSuccessCall;

            if (ModelState.IsValid)
            {
                var checkAccount = new Taikhoannguoidung();
                try
                {
                    checkAccount = db.Taikhoannguoidungs.Where(acc => acc.Email.Equals(email.Trim())).FirstOrDefault();
                    Console.WriteLine(checkAccount.Email);
                }
                catch (Exception ex)
                {
                    return View();
                }

                if (checkAccount.MatKhau.Equals(password.Trim()))
                {
                    return _loginSuccessCallback.Invoke(true);
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
            // add session info here
            //HttpContext.Session.
            Console.WriteLine("login success");
            return RedirectToAction("Index", "Home");
        }
        
    }
}
