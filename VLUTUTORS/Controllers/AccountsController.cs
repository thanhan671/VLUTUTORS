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
using Org.BouncyCastle.Asn1.X509;
using static System.Net.Mime.MediaTypeNames;

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
                    TempData["Message"] = "Sai tài khoản hoặc mật khẩu";
                    TempData["MessageType"] = "error";
                    return View();
                }
                else
                {
                    if (checkAccount.XacThuc == false)
                    {
                        TempData["Message"] = "Vui lòng kiểm tra email để xác thực tài khoản!";
                        TempData["MessageType"] = "error";
                        return RedirectToAction("VerifyAccount", "Accounts");
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
                            TempData["Message"] = "Mật khẩu chưa đúng, vui lòng kiểm tra lại!";
                            TempData["MessageType"] = "error";
                        }
                        else
                        {
                            TempData["Message"] = "Tài khoản có Email đăng nhập là " + checkAccount.Email + " đã bị khóa, vui lòng liên hệ với chúng tôi để được giải quyết! Xin cảm ơn";
                            TempData["MessageType"] = "error";
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
                taiKhoan.AnhDaiDien = "https://cntttest.vanlanguni.edu.vn:18081/CP25Team01/"+taiKhoan.AnhDaiDien.TrimStart('[', '"').TrimEnd('"', ']').Replace("\\\\", "/");
            }
            else
            {
                taiKhoan.AnhDaiDien = "https://cdn.icon-icons.com/icons2/1378/PNG/512/avatardefault_92824.png";
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

            //foreach (IFormFile postedFile in avatar)
            //{
            //    string fileName = Path.GetFileName(postedFile.FileName);
            //    Console.WriteLine("get file name: " + fileName);
            //}

            if (dbTaikhoannguoidung == null || (dbTaikhoannguoidung != null && id != dbTaikhoannguoidung.Id))
            {
                return NotFound();
            }
            if (Sdt.Length < 10 || Sdt.Length >= 11)
            {
                TempData["errorMessage"] = "Số điện thoại phải đủ 10 số!";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (MatKhau == null && ReMatKhau==null)
                    {
                        try
                        {
                            dbTaikhoannguoidung.IdgioiTinh = IdgioiTinh;
                            dbTaikhoannguoidung.NgaySinh = NgaySinh;
                            dbTaikhoannguoidung.Sdt = Sdt;
                            dbTaikhoannguoidung.AnhDaiDien = avatar.Count != 0 ? TutorServices.SaveAvatar(this._environment.WebRootPath, avatarPath, avatar) : dbTaikhoannguoidung.AnhDaiDien;

                            TempData["Message"] = "Cập nhật thành công!";
                            TempData["MessageType"] = "success";
                            db.Entry(dbTaikhoannguoidung).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Details", new { id });
                        }
                        catch (Exception ex)
                        {
                            return RedirectToAction("Details", new { id });
                        }
                    }
                    else if (MatKhau != null && ReMatKhau == MatKhau && MatKhau.Length >=6)
                    {
                        try
                        {
                            dbTaikhoannguoidung.IdgioiTinh = IdgioiTinh;
                            dbTaikhoannguoidung.NgaySinh = NgaySinh;
                            dbTaikhoannguoidung.Sdt = Sdt;
                            dbTaikhoannguoidung.AnhDaiDien = avatar.Count != 0 ? TutorServices.SaveAvatar(this._environment.WebRootPath, avatarPath, avatar) : dbTaikhoannguoidung.AnhDaiDien;
                            dbTaikhoannguoidung.MatKhau = MatKhau;

                            TempData["Message"] = "Cập nhật thành công!";
                            TempData["MessageType"] = "success";
                            db.Entry(dbTaikhoannguoidung).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Details", new { id });
                        }
                        catch (Exception ex)
                        {
                            return RedirectToAction("Details", new { id });
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Mật khẩu phải đủ từ 6 ký tự và Xác nhận mật khẩu phải trùng khớp với Mật khẩu!";
                    }

                }
                else
                {
                    return RedirectToAction("EditLearnerAccounts", new { id });
                }
            }
            return RedirectToAction("EditLearnerAccounts", new { id });
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] string HoTen, [FromForm] string Email, [FromForm] string MatKhau, [FromForm] string rePass)
        {
            if (ModelState["Email"].Errors.Count == 0 && ModelState["MatKhau"].Errors.Count == 0)
            {
                ModelState.Clear();
            }
            if(HoTen!=null && Email != null && HoTen.Length >=5)
            {
                if (MatKhau != null && rePass == MatKhau && MatKhau.Length >= 6)
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
                            NgaySinh = DateTime.Parse("01/01/0001"),
                            TrangThaiTaiKhoan = true,
                            IdgioiTinh = 1,
                            Idkhoa = 1,
                            IdxetDuyet = 6,
                            MaXacThuc = numVerify,
                            XacThuc = false,
                            SoDuVi = 0,
                            NgayTao = DateTime.Now
                        };
                        try
                        {

                            db.Add(taiKhoanNguoiDung);
                            await db.SaveChangesAsync();
                            HttpContext.Session.SetString("email", Email);
                            return RedirectToAction("SendMail", "Accounts",
                            new { toEmail = Email, name = HoTen, verifyCode = numVerify });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            return RedirectToAction("Login", "Accounts");
                        }
                    }
                    else
                    {
                        TempData["Message"] = "Email đã được đăng ký, vui lòng kiểm tra lại!";
                        TempData["MessageType"] = "error";
                        return RedirectToAction("Login", "Accounts");
                    }
                }
                TempData["Message"] = "Mật khẩu phải đủ từ 6 ký tự và Xác nhận mật khẩu phải trùng khớp với Mật khẩu, vui lòng kiểm tra lại!";
                TempData["MessageType"] = "error";
                return RedirectToAction("Login", "Accounts");
            }
            TempData["Message"] = "Vui lòng điền đủ và đúng định dạng thông tin để đăng ký tài khoản!";
            TempData["MessageType"] = "error";
            return RedirectToAction("Login", "Accounts");
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
                        TempData["Message"] = "Đăng ký tài khoản thành công!";
                        TempData["MessageType"] = "success";
                    }
                    else
                    {
                        TempData["Message"] = "Mã xác thực chưa đúng!";
                        TempData["MessageType"] = "error";
                        return View();
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
                        TempData["Message"] = "Đăng ký tài khoản thành công!";
                        TempData["MessageType"] = "success";
                    }
                    else
                    {
                        TempData["Message"] = "Mã xác thực chưa đúng!";
                        TempData["MessageType"] = "error";
                        return View();
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
            var checkMail = db.Taikhoannguoidungs.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
            if (checkMail != null)
            {
                Random pass = new Random();
                int code = pass.Next(100000, 999999);

                var sqlStringBuilder = new SqlConnectionStringBuilder();
                sqlStringBuilder["Server"] = "tuleap.vanlanguni.edu.vn,18082";
                sqlStringBuilder["Database"] = "CP25Team01";
                sqlStringBuilder["UID"] = "CP25Team01";
                sqlStringBuilder["PWD"] = "VLUTUTORS01";

                var sqlStringConnection = sqlStringBuilder.ToString();

                using var connection = new SqlConnection(sqlStringConnection);

                connection.Open();

                using var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "UPDATE TAIKHOANNGUOIDUNG SET MaXacThuc = @MaXacThuc WHERE Email = @Email";

                command.Parameters.AddWithValue("@MaXacThuc", code);
                command.Parameters.AddWithValue("@Email", Email);

                command.ExecuteNonQuery();

                connection.Close();

                string mailTitle = "Gia Sư Văn Lang";
                string fromMail = "giasuvanlang.thongtin@gmail.com";
                string fromEmailPass = "vrzaiqmdiycujvas";
                string bodyMail = "<!DOCTYPE html>" +
                        "<html>" +
                            "<body>" +
                                "<p style = \"margin: 0%;\">" +
                                "Xin chào, <br/>" +
                                "Mã xác minh bạn cần dùng để thay đổi mật khẩu cho email <b>" + Email + "</b> là:</p>" +

                                "<p style = \"color: green;font-size: 40px; margin: 0 0 0 50px;\">" + code + "</p>" +

                                "<p style = \"margin: 0%;\" > Vui lòng nhập mã xác thực để thay đổi mật khẩu<br/>" +
                                "Nếu bạn không yêu cầu mã này thì có thể ai đó đang sử dụng email <b>" + Email + "</b> để thay đổi mật khẩu tài khoản." +
                                "<b style = \"color: red;\" > Không chuyển tiếp hoặc cung cấp mã này cho bất kỳ ai.</b><br/></p>" +

                                "<b style = \"font-size: small;text-align: center; margin: 0%;\"> Bạn nhận được thông báo này vì địa chỉ email đang được sử dụng cho " +
                                "tài khoản trên trang Gia Sư Văn Lang.Nếu thông tin này không chính xác," +
                                "vui lòng bỏ qua và không trả lời lại mail này.Xin cảm ơn!</b><br/>" +
                               " Trân trọng!<br/>" +
                                "<b>Gia Sư Văn Lang</b>" +
                            "</body>" +
                        "</html>";

                //Email and content
                MailMessage message = new MailMessage(new MailAddress(fromMail, mailTitle), new MailAddress(Email));
                message.Subject = "[VLUTUTORS] Khôi phục mật khẩu";
                message.Body = bodyMail;
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
                HttpContext.Session.SetString("Email", Email);

                TempData["Message"] = "Mã xác nhận đã được gửi đến mail của bạn, vui lòng kiểm tra!";
                TempData["MessageType"] = "success";

                return RedirectToAction("ChangesPass", "Accounts");
            }
            TempData["Message"] = "Email không tồn tại trên hệ thống, vui lòng kiểm tra lại!";
            TempData["MessageType"] = "error";
            return View();
        }

        [HttpGet]
        public IActionResult ChangesPass()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangesPass(string newPass, string verifyCode)
        {
            string email = HttpContext.Session.GetString("Email");
            var checkAccount = db.Taikhoannguoidungs.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == email.ToLower());
            if (checkAccount != null)
            {
                if (checkAccount.MaXacThuc.ToString() == verifyCode)
                {
                    if (newPass.Length >= 6)
                    {
                    checkAccount.MatKhau = newPass;
                    db.Update(checkAccount);
                    db.SaveChangesAsync();
                    }
                    else
                    {
                        TempData["Message"] = "Mật khẩu phải từ 6 ký tự trở lên!";
                        TempData["MessageType"] = "error";
                        return View();
                    }
                }
                else
                {
                    TempData["Message"] = "Mã xác thực không đúng!";
                    TempData["MessageType"] = "error";
                    return View();
                }
            }
            TempData["Message"] = "Thiết lập mật khẩu mới thành công!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Login", "Accounts");
        }

        public IActionResult SendMail(string toEmail, string name, int verifyCode)
        {
            string mailTitle = "Gia Sư Văn Lang";
            string fromMail = "giasuvanlang.thongtin@gmail.com";
            string fromEmailPass = "vrzaiqmdiycujvas";
            string bodyMail = "<!DOCTYPE html>" +
                    "<html>" +
                        "<body>" +
                            "<p style = \"margin: 0%;\">" +
                            "Xin chào, <b>" + name + "</b> !<br/>" +
                            "Mã xác minh bạn cần dùng để xác thực email <b>" + toEmail + "</b> là:</p>" +

                            "<p style = \"color: green;font-size: 40px; margin: 0 0 0 50px;\">" + verifyCode + "</p>" +

                            "<p style = \"margin: 0%;\" > Vui lòng xác thực để sử dụng các tính năng của trang web. Hoặc nhấn vào " +
                            "<a href=\"https://cntttest.vanlanguni.edu.vn:18081/CP25Team01/Accounts/VerifyAccount\">đây</a> để xác thực:<br/>" +
                            "Nếu bạn không yêu cầu mã này thì có thể ai đó đang sử dụng email <b>" + toEmail + "</b> để đăng ký tài khoản." +
                            "<b style = \"color: red;\" > Không chuyển tiếp hoặc cung cấp mã này cho bất kỳ ai.</b><br/></p>" +

                            "<b style = \"font-size: small;text-align: center; margin: 0%;\"> Bạn nhận được thông báo này vì địa chỉ email đang được sử dụng cho " +
                            "tài khoản trên trang Gia Sư Văn Lang.Nếu thông tin này không chính xác," +
                            "vui lòng bỏ qua và không trả lời lại mail này.Xin cảm ơn!</b><br/>" +
                           " Trân trọng!<br/>" +
                            "<b>Gia Sư Văn Lang</b>" +
                        "</body>" +
                    "</html>";

            //Email and content
            MailMessage message = new MailMessage(new MailAddress(fromMail, mailTitle), new MailAddress(toEmail));
            message.Subject = "[VLUTUTORS] Xác thực Email cho Gia sư Văn Lang";
            message.Body = bodyMail;
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