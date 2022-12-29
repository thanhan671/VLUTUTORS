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

            Taikhoannguoidung checkAccount;
            checkAccount = db.Taikhoannguoidungs.Where(acc => acc.Email.Equals(email.Trim())).FirstOrDefault();
            if (checkAccount.TrangThaiTaiKhoan == true)
            {
                if (checkAccount != null)
                {
                    _loginSuccessCallback = LoginSuccessCall;
                }
                else
                {
                    ViewBag.Message = "Email chưa đúng, vui lòng kiểm tra lại";
                    return View();
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
            return View();
        }

        
        public async Task<IActionResult> Details(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }
            var noiDung = await db.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);
            ViewData["Slogan"] = noiDung.Slogan;
            ViewData["gtChanTrang"] = noiDung.GioiThieuChanTrang;
            ViewData["diaChi"] = noiDung.DiaChi;
            ViewData["Sdt"] = noiDung.Sdt;
            ViewData["Email"] = noiDung.Email;
            ViewData["Fb"] = noiDung.Facebook;
            ViewData["gioiThieu"] = noiDung.GioiThieu;
            var taiKhoan = await db.Taikhoannguoidungs.FirstOrDefaultAsync(m => m.Id == id);
            if (taiKhoan.AnhDaiDien != null)
            {
                TempData["avt"] = "Yes";
                string newString = taiKhoan.AnhDaiDien.TrimStart('[', '"');
                ViewData["image"] = newString.TrimEnd('"', ']').ToString();
            }
            else
            {
                ViewData["image"] = "https://cdn.icon-icons.com/icons2/1378/PNG/512/avatardefault_92824.png";
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
            var noiDung = await db.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);
            ViewData["Slogan"] = noiDung.Slogan;
            ViewData["gtChanTrang"] = noiDung.GioiThieuChanTrang;
            ViewData["diaChi"] = noiDung.DiaChi;
            ViewData["Sdt"] = noiDung.Sdt;
            ViewData["Email"] = noiDung.Email;
            ViewData["Fb"] = noiDung.Facebook;
            ViewData["gioiThieu"] = noiDung.GioiThieu;
            var taiKhoan = await db.Taikhoannguoidungs.FirstOrDefaultAsync(m => m.Id == id);
            if (taiKhoan.AnhDaiDien != null)
            {
                TempData["avt"] = "Yes";
                string newString = taiKhoan.AnhDaiDien.TrimStart('[', '"');
                ViewData["image"] = newString.TrimEnd('"', ']').ToString();
            }
            else
            {
                ViewData["image"] = "https://cdn.icon-icons.com/icons2/1378/PNG/512/avatardefault_92824.png";
            }
            var gioiTinhs = await db.Gioitinhs.ToListAsync();
            SelectList ddlStatus = new SelectList(gioiTinhs, "IdgioiTinh", "GioiTinh1");
            taiKhoan.GenderItems = ddlStatus;
            return View(taiKhoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLearnerAccounts([Bind(include: "Id, MatKhau, IdgioiTinh, Sdt, NgaySinh, AnhDaiDien")] int id, [FromForm] int IdgioiTinh, [FromForm] DateTime NgaySinh, [FromForm] string Sdt, [FromForm] string MatKhau, [FromForm] string ReMatKhau, List<IFormFile> avatar)
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
                    dbTaikhoannguoidung.AnhDaiDien = avatar.Count != 0 ? TutorServices.SaveUploadImages(this._environment.WebRootPath, avatarPath, avatar) : dbTaikhoannguoidung.AnhDaiDien;
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
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string HoTen, string Email, string MatKhau)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = db.Taikhoannguoidungs.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());

                if (taiKhoan == null)
                {
                    Taikhoannguoidung taiKhoanNguoiDung = new Taikhoannguoidung
                    {
                        HoTen = HoTen,
                        Email = Email,
                        MatKhau = MatKhau,
                        TrangThaiTaiKhoan = true,
                    };
                    try
                    {
                        db.Add(taiKhoanNguoiDung);
                        await db.SaveChangesAsync();
                        ViewBag.Message = "Đăng ký tài khoản thành công!";
                    }
                    catch
                    {
                        return RedirectToAction("Login", "Accounts");
                    }
                }
                else
                {
                    ViewBag.Message = "Email đã được đăng ký, vui lòng kiểm tra lại";
                    return RedirectToAction("Login", "Accounts");
                }
            }
            return RedirectToAction("Login", "Accounts");
        }

        public IActionResult LoginSuccessCall(Taikhoannguoidung taikhoannguoidung)
        {
            // add session info here
            //HttpContext.Session.
            HttpContext.Session.SetInt32("LoginId", taikhoannguoidung.Id);
            HttpContext.Session.SetInt32("IdGiaSu", (int)taikhoannguoidung.IdxetDuyet);
            HttpContext.Session.SetString("loginName", taikhoannguoidung.HoTen);
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