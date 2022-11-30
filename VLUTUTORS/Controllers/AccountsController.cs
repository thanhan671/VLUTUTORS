using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using MimeKit;
using MailKit.Net.Smtp;

namespace VLUTUTORS.Controllers
{
    public class AccountsController : Controller
    {
        private CP25Team01Context db = new CP25Team01Context();
        private Func<Taikhoannguoidung, IActionResult> _loginSuccessCallback;

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
            

            if (ModelState.IsValid)
            {
                var checkAccount = new Taikhoannguoidung();
                try
                {
                    checkAccount = db.Taikhoannguoidungs.Where(acc => acc.Email.Equals(email.Trim())).FirstOrDefault();
                    _loginSuccessCallback = LoginSuccessCall;
                }
                catch (Exception ex)
                {
                    return View();
                }

                if (checkAccount.MatKhau.Equals(password.Trim()))
                {
                    return _loginSuccessCallback.Invoke(checkAccount);
                }
            }

            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string HoTen, string Email, string MatKhau)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var taiKhoan = db.Taikhoannguoidungs.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());

                    if (taiKhoan != null)
                    {
                        return RedirectToAction("Login", "Accounts");
                    }
                    else
                    {
                        Taikhoannguoidung taiKhoanNguoiDung = new Taikhoannguoidung
                        {
                            HoTen = HoTen,
                            Email = Email,
                            MatKhau = MatKhau,
                            TrangThaiTaiKhoan = true
                        };
                        try
                        {
                            db.Add(taiKhoanNguoiDung);
                            await db.SaveChangesAsync();
                        }
                        catch
                        {
                            return RedirectToAction("Login", "Accounts");
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Accounts");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Accounts");

            }
            return RedirectToAction("Login", "Accounts");
        }

        private IActionResult LoginSuccessCall(Taikhoannguoidung taikhoannguoidung)
        {
            // add session info here
            //HttpContext.Session.
            HttpContext.Session.SetInt32("LoginId", taikhoannguoidung.Id);
            //HttpContext.Session.SetString("LoginName", taikhoannguoidung.HoTen);
            HttpContext.Session.SetString("SessionInfo", JsonConvert.SerializeObject(taikhoannguoidung));

            Console.WriteLine("login success");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ForGotPass(string Email)
        {
            Random pass = new Random();
            int newPass = pass.Next(100000, 999999);


            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Gia Sư Văn Lang", "giasuvanlang@gmail.com"));
            message.To.Add(new MailboxAddress("Thanh An", "thanhannguyen67@gmail.com"));
            message.Subject = "Khôi phục mật khẩu Gia Sư Văn Lang";
            message.Body = new TextPart("plain")
            {
                Text = newPass.ToString()
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("thanhannguyen671@gmail.com", "thanhannguyen672001");

                client.Send(message);

                client.Disconnect(true);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}