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
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System.IO;
using VLUTUTORS.Support.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mail;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace VLUTUTORS.Controllers
{
    public class AccountsController : Controller
    {
        private CP25Team01Context db = new CP25Team01Context();
        private Func<Taikhoannguoidung, IActionResult> _loginSuccessCallback;
        private readonly ILogger<AccountsController> _logger;
        private IHostingEnvironment _environment;

        public AccountsController(ILogger<AccountsController> logger, IHostingEnvironment environment)
        {
            _logger = logger;
            this._environment = environment;
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

            if (ModelState["Email"].Errors.Count == 0 && ModelState["MatKhau"].Errors.Count == 0)
            {
                ModelState.Clear();
            }

            if (ModelState.IsValid)
            {
                Taikhoannguoidung checkAccount;
                checkAccount = db.Taikhoannguoidungs.Where(acc => acc.Email.Equals(email.Trim())).FirstOrDefault();
                if (checkAccount == null)
                {
                    ViewBag.Message = "Email chưa đúng, vui lòng kiểm tra lại";
                    return View();
                }
                else
                {
                    if (checkAccount.XacThuc==false)
                    {
                        ViewBag.Message = "Vui lòng kiểm tra email để xác thực tài khoản!";
                        return View();
                    }
                    else
                    {
                        if (checkAccount.TrangThaiTaiKhoan == true)
                        {
                            if (checkAccount != null)
                            {
                                _loginSuccessCallback = LoginSuccessCall;
                            }

                            if (checkAccount.MatKhau.Equals(password.Trim()))
                            {
                                return _loginSuccessCallback.Invoke(checkAccount);
                            }
                            ViewBag.Message = "Mật khẩu chưa đúng, vui lòng kiểm tra lại";
                        }
                        else
                        {
                            ViewBag.Message = "Tài khoản có Email đăng nhập là " + checkAccount.Email + " đã bị khóa, vui lòng liên hệ với chúng tôi để được giải quyết! Xin cảm ơn";
                        }
                    }
                }
            }

            return View();
        }


        public async Task<IActionResult> Details(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await db.Taikhoannguoidungs.FirstOrDefaultAsync(m => m.Id == id);
            if (taiKhoan.AnhDaiDien != null)
            {
                TempData["avt"] = "Yes";
            }
            else
            {
                TempData["avt"] = null;
            }

            var gioiTinhs = await db.Gioitinhs.ToListAsync();
            SelectList ddlStatus = new SelectList(gioiTinhs, "IdgioiTinh", "GioiTinh1");
            taiKhoan.GenderItems = ddlStatus;
            return View(taiKhoan);
        }

        [HttpGet]
        public async Task<IActionResult> EditLearnerAccounts(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await db.Taikhoannguoidungs.FirstOrDefaultAsync(m => m.Id == id);

            if (taiKhoan.AnhDaiDien != null)
            {
                TempData["avt"] = "Yes";
            }
            else
            {
                TempData["avt"] = null;
            }

            var gioiTinhs = await db.Gioitinhs.ToListAsync();
            SelectList ddlStatus = new SelectList(gioiTinhs, "IdgioiTinh", "GioiTinh1");
            taiKhoan.GenderItems = ddlStatus;
            return View(taiKhoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLearnerAccounts(int id, [FromForm] int IdgioiTinh, [FromForm] DateTime NgaySinh, [FromForm] string Sdt, [FromForm] string MatKhau, [FromForm] string ReMatKhau, List<IFormFile> avatar)
        {
            var dbTaikhoannguoidung = await db.Taikhoannguoidungs.FindAsync(id);
            string avatarPath = Path.Combine("avatars", dbTaikhoannguoidung.Id.ToString());

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
                    dbTaikhoannguoidung.AnhDaiDien = avatar.Count != 0 ? TutorServices.SaveAvatar(this._environment.WebRootPath, avatarPath, avatar) : dbTaikhoannguoidung.AnhDaiDien;

                    if (!string.IsNullOrEmpty(MatKhau))
                        dbTaikhoannguoidung.MatKhau = MatKhau;
                    TempData["message"] = "Cập nhật thành công!";
                    db.Entry(dbTaikhoannguoidung).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Details", new { id });
                }
            }

            return RedirectToAction("Details", new { id });

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] string HoTen, [FromForm] string Email, [FromForm] string MatKhau)
        {
            if (ModelState["Email"].Errors.Count == 0 && ModelState["MatKhau"].Errors.Count == 0)
            {
                ModelState.Clear();
            }

            if (ModelState.IsValid)
            {
                var taiKhoan = db.Taikhoannguoidungs.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());

                if (taiKhoan == null)
                {
                    Random verify = new Random();
                    int numVerify = verify.Next(100000, 999999);
                    Taikhoannguoidung taiKhoanNguoiDung = new Taikhoannguoidung
                    {
                        HoTen = HoTen,
                        Email = Email,
                        MatKhau = MatKhau,
                        TrangThaiTaiKhoan = true,
                        IdgioiTinh = 1,
                        Idkhoa = 1,
                        IdxetDuyet = 6,
                        XacThuc = false,
                        MaXacThuc = numVerify
                    };
                    try
                    {

                        db.Add(taiKhoanNguoiDung);
                        await db.SaveChangesAsync();
                        HttpContext.Session.SetString("email", Email);
                        return RedirectToAction("SendMail", "Accounts",
                        new { toEmail = Email, mailBody = "Mã xác thực của bạn là " + numVerify + "<br/>Vui lòng xác thực để sử dụng các tính năng của trang web! Hoặc truy cập vào đường dẫn sau để xác thực:" +
                        "https://cntttest.vanlanguni.edu.vn:18081/CP25Team01/Accounts/VerifyAccount"
                        });

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return RedirectToAction("Login", "Accounts");
                    }
                }
                else
                {
                    ViewBag.Message = "Email đã được đăng ký, vui lòng kiểm tra lại";
                    return RedirectToAction("Login", "Accounts");
                }
            }
            ViewBag.Message = "Đăng ký tài khoản thành công!";
            return RedirectToAction("VerifyAccount", "Accounts");
        }

        [HttpGet]
        public IActionResult VerifyAccount()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyAccount(string verifyCode, string email)
        {
            if (HttpContext.Session.GetString("email") != null)
            {
                email = HttpContext.Session.GetString("email");
                var dbTaikhoannguoidung = db.Taikhoannguoidungs.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == email.ToLower());

                if (dbTaikhoannguoidung != null)
                {
                    if (dbTaikhoannguoidung.MaXacThuc == int.Parse(verifyCode))
                    {
                        dbTaikhoannguoidung.XacThuc = true;
                        db.Update(dbTaikhoannguoidung);
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                var dbTaikhoannguoidung = db.Taikhoannguoidungs.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == email.ToLower());

                if (dbTaikhoannguoidung != null)
                {
                    if (dbTaikhoannguoidung.MaXacThuc == int.Parse(verifyCode))
                    {
                        dbTaikhoannguoidung.XacThuc = true;
                        db.Update(dbTaikhoannguoidung);
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            return RedirectToAction("Login", "Accounts");
        }

        public IActionResult LoginSuccessCall(Taikhoannguoidung taikhoannguoidung)
        {
            // add session info here
            //HttpContext.Session.
            HttpContext.Session.SetInt32("LoginId", taikhoannguoidung.Id);
            HttpContext.Session.SetInt32("IdGiaSu", taikhoannguoidung.IdxetDuyet == null ? 0 : (int)taikhoannguoidung.IdxetDuyet);
            HttpContext.Session.SetString("loginName", taikhoannguoidung.HoTen);
            HttpContext.Session.SetString("loginEmail", taikhoannguoidung.Email);
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
        public IActionResult ForGotPass(string Email)
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

            string mailTitle = "Gia Sư Văn Lang";
            string fromMail = "giasuvanlang.thongtin@gmail.com";
            string fromEmailPass = "wwxtjmqczzdgwqke";

            //Email and content
            MailMessage message = new MailMessage(new MailAddress(fromMail, mailTitle), new MailAddress(Email));
            message.Subject = "Khôi phục mật khẩu";
            message.Body = "<p>Mật khẩu mới của bạn là<p><br/> <b>" + newPass + "</b><br/><p>Vui lòng đăng nhập với mật khẩu mới để thay đổi mật khẩu!<p>";
            message.IsBodyHtml = true;

            //Server detail
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            //Credentials
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential();
            credential.UserName = fromMail;
            credential.Password = fromEmailPass;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credential;

            smtp.Send(message);

            return RedirectToAction("Login", "Accounts");
        }

        public IActionResult SendMail(string toEmail, string mailBody)
        {
            string mailTitle = "Gia Sư Văn Lang";
            string fromMail = "giasuvanlang.thongtin@gmail.com";
            string fromEmailPass = "wwxtjmqczzdgwqke";

            //Email and content
            MailMessage message = new MailMessage(new MailAddress(fromMail, mailTitle), new MailAddress(toEmail));
            message.Subject = "Xác thực Email cho Gia sư Văn Lang";
            message.Body = mailBody;
            message.IsBodyHtml = true;

            //Server detail
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            //Credentials
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential();
            credential.UserName = fromMail;
            credential.Password = fromEmailPass;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credential;

            smtp.Send(message);

            ViewBag.email = toEmail;

            return RedirectToAction("VerifyAccount", "Accounts");
        }
    }
}