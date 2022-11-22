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
            //Taikhoannguoidung taikhoannguoidung = new Taikhoannguoidung();
            //if(ModelState.IsValid)
            //{
            //    Console.WriteLine("first account: " + db.Taikhoannguoidungs.Count());
            //}   
            //else
            //{
            //    Console.WriteLine("Model not valid");
            //}
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            string emailTest = "test@gmail.com";
            string passwordTest = "test1234";
            Console.WriteLine("emailfaill");
            _loginSuccessCallback = LoginSuccessCall;
            using(db)
            {
                //var checkAccount = db.Taikhoannguoidungs.Where(acc => acc.Email.Equals(email.Trim())).FirstOrDefault();
                //if (checkAccount == null)
                //{
                //    return View();
                //}
                if(!emailTest.Equals(email.Trim()))
                {
                    Console.WriteLine("emailfaill");
                    return View();
                }

                //if(password.Trim().Equals(checkAccount.MatKhau.Trim()))
                //{
                //    _loginSuccessCallback.Invoke(true);
                //}
                if (passwordTest.Equals(password.Trim()))
                {
                    Console.WriteLine("true");
                    return _loginSuccessCallback.Invoke(true);
                }
                //else
                //{
                //    return View();
                //}
            }

            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        private IActionResult LoginSuccessCall(bool status)
        {
            Console.WriteLine("login success");
            return RedirectToAction("Index", "Home");
        }
        
    }
}
