﻿using Microsoft.AspNetCore.Mvc;
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
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using VLUTUTORS.Security;
using System.Net.Http;

namespace VLUTUTORS.Controllers
{
    public class AccountsController : Controller
    {
        UserManager _userManager;
        private CP25Team01Context db = new CP25Team01Context();

        public AccountsController(UserManager userManager)
        {
            _userManager = userManager;
        }


        public IActionResult Login()
        {
            Taikhoannguoidung _taikhoannguoidung = new Taikhoannguoidung();
            return View(_taikhoannguoidung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind(include: "Email, MatKhau")] Taikhoannguoidung taikhoannguoidung)
        {
            string email = taikhoannguoidung.Email;
            string password = taikhoannguoidung.MatKhau;
            try
            {
                _userManager.SignIn(this.HttpContext, email, password);
                var checkAccount = db.Taikhoannguoidungs.Where(acc => acc.Email.Equals(email.Trim())).FirstOrDefault();
                if (checkAccount != null && checkAccount.Id > 0)
                {
                    HttpContext.Session.SetInt32("LoginId", checkAccount.Id);
                    HttpContext.Session.SetString("loginName", checkAccount.HoTen);
                    HttpContext.Session.SetString("SessionInfo", JsonConvert.SerializeObject(checkAccount));
                }
                else
                {
                    var admin = db.Taikhoanadmins.Where(acc => acc.TaiKhoan.Equals(email.Trim())).FirstOrDefault();
                    if (admin != null && admin.Id > 0)
                    {
                        HttpContext.Session.SetInt32("LoginId", admin.Id);
                        HttpContext.Session.SetString("loginName", admin.TaiKhoan);
                        HttpContext.Session.SetString("SessionInfo", JsonConvert.SerializeObject(admin));
                    }
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public async Task<IActionResult> Details(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await db.Taikhoannguoidungs.FirstOrDefaultAsync(m => m.Id == id);
            string newString = taiKhoan.AnhDaiDien.TrimStart('[','"');
            ViewData["image"] = newString.TrimEnd('"',']').ToString();
            var gioiTinhs = await db.Gioitinhs.ToListAsync();
            SelectList ddlStatus = new SelectList(gioiTinhs, "IdgioiTinh", "GioiTinh1");
            taiKhoan.GioiTinhs = ddlStatus;
            return View(taiKhoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, [FromForm] int IdgioiTinh, [FromForm] DateTime NgaySinh, [FromForm] string Sdt, [FromForm] string MatKhau, [FromForm] string ReMatKhau)
        {
            var dbTaikhoannguoidung = await db.Taikhoannguoidungs.FindAsync(id);
            if (dbTaikhoannguoidung == null || (dbTaikhoannguoidung != null && id != dbTaikhoannguoidung.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbTaikhoannguoidung.IdgioiTinh = IdgioiTinh;
                    dbTaikhoannguoidung.NgaySinh = NgaySinh;
                    dbTaikhoannguoidung.Sdt = Sdt;
                    if (!string.IsNullOrEmpty(MatKhau))
                        dbTaikhoannguoidung.MatKhau = MatKhau;

                    db.Update(dbTaikhoannguoidung);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Details", new { id });
                }
                return RedirectToAction("Details", new { id });
            }

            return RedirectToAction("Details", new { id });

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

        public IActionResult LoginSuccessCall(Taikhoannguoidung taikhoannguoidung)
        {
            // add session info here
            //HttpContext.Session.
            HttpContext.Session.SetInt32("LoginId", taikhoannguoidung.Id);
            HttpContext.Session.SetString("loginName", taikhoannguoidung.HoTen);
            //HttpContext.Session.SetString("LoginName", taikhoannguoidung.HoTen);
            HttpContext.Session.SetString("SessionInfo", JsonConvert.SerializeObject(taikhoannguoidung));

            Console.WriteLine("login success");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForGotPass()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForGotPass(string Email)
        {
            Random pass = new Random();
            int newPass = pass.Next(100000, 999999);

            var sqlStringBuilder = new SqlConnectionStringBuilder();
            sqlStringBuilder["Server"] = "tuleap.vanlanguni.edu.vn,18082";
            sqlStringBuilder["Database"] = "CP25Team01";
            sqlStringBuilder["UID"] = "CP25Team01";
            sqlStringBuilder["PWD"] = "Cap25t01";

            var sqlStringConnection = sqlStringBuilder.ToString();

            using var connection = new SqlConnection(sqlStringConnection);

            connection.Open();

            using var command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "UPDATE TAIKHOANNGUOIDUNG SET MatKhau = @MatKhau WHERE Email = @Email";

            command.Parameters.AddWithValue("@MatKhau", newPass);
            command.Parameters.AddWithValue("@Email", Email);

            command.ExecuteNonQuery();

            connection.Close();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Gia Sư Văn Lang", "giasuvanlang@gmail.com"));
            message.To.Add(new MailboxAddress("Gia Sư Văn Lang", Email));
            message.Subject = "Khôi phục mật khẩu Gia Sư Văn Lang";
            message.Body = new TextPart("plain")
            {
                Text = "Mật khẩu mới của bạn là: " + newPass.ToString() + " Vui lòng đăng nhập với mật khẩu mới để đặt lại mật khẩu."
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("thanhannguyen67@gmail.com", "zepyqmhzacjzgsid");

                client.Send(message);

                client.Disconnect(true);

                client.Dispose();
            }

            return RedirectToAction("Login", "Accounts");
        }
    }
}