using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
using Newtonsoft.Json;
using QuickMailer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using VLUTUTORS.Support.Services;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "2")]
    public class ManageTutors : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        private readonly ILogger<ManageTutors> _logger;
        private IHostingEnvironment _environment;

        public ManageTutors(ILogger<ManageTutors> logger, IHostingEnvironment environment)
        {
            _logger = logger;
            this._environment = environment;
        }

        public async Task<IActionResult> Index([FromQuery] string search)
        {
            if (HttpContext.Session.GetString("LoginADId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = new TutorListModel();
            var awaitTutors = new List<TutorViewModel>();
            var approvedTutors = new List<TutorViewModel>();

            var accounts = await _context.Taikhoannguoidungs.ToListAsync();
            var xetduyets = await _context.Xetduyets.ToListAsync();
            var monGiaSus = await _context.Mongiasus.ToListAsync();

            foreach (var account in accounts)
            {
                if (account.IdxetDuyet >= 1 && account.IdxetDuyet < 4)
                {
                    var awaitApproveStatus = xetduyets.FirstOrDefault(it => it.IdxetDuyet == account.IdxetDuyet);
                    if (awaitApproveStatus == null)
                        awaitApproveStatus = new Xetduyet();

                    var subject = monGiaSus.FirstOrDefault(it => it.IdmonGiaSu == account.IdmonGiaSu1);
                    var subject2 = monGiaSus.FirstOrDefault(it => it.IdmonGiaSu == account.IdmonGiaSu2);
                    if (subject == null)
                    {
                        subject = new Mongiasu();
                    }
                        

                        awaitTutors.Add(new TutorViewModel()
                        {
                            Tutor = account,
                            Subject1 = subject.TenMonGiaSu,
                            ApprovedStatus = awaitApproveStatus.TenTrangThai
                        });
                }
            }

            model.awaitTutors = awaitTutors;

            var approvedStatus = xetduyets.FirstOrDefault(it => it.TenTrangThai == "Đã xét duyệt");
            if (approvedStatus != null)
            {
                foreach (var account in accounts)
                {
                    if (account.IdxetDuyet == approvedStatus.IdxetDuyet)
                    {
                        var subject = monGiaSus.FirstOrDefault(it => it.IdmonGiaSu == account.IdmonGiaSu1);
                        var subject2 = monGiaSus.FirstOrDefault(it => it.IdmonGiaSu == account.IdmonGiaSu2);
                        if (subject != null && subject2 ==null)
                        {
                            approvedTutors.Add(new TutorViewModel()
                            {
                                Tutor = account,
                                Subject1 = subject.TenMonGiaSu,
                                ApprovedStatus = approvedStatus.TenTrangThai
                            });
                        }
                        else if (subject != null && subject2 != null)
                        {
                            approvedTutors.Add(new TutorViewModel()
                            {
                                Tutor = account,
                                Subject1 = subject.TenMonGiaSu,
                                Subject2 = subject2.TenMonGiaSu,
                                ApprovedStatus = approvedStatus.TenTrangThai
                            });
                        }
                        else
                        {
                            subject = new Mongiasu();
                            subject2 = new Mongiasu();
                        }
                    }
                }
            }

            model.approvedTutors = approvedTutors;

            ViewData["search"] = search;
            return View(model);
        }

        public async Task<IActionResult> DetailTutor(int id)
        {
            if (HttpContext.Session.GetString("LoginADId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var account = await _context.Taikhoannguoidungs.FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
                return NotFound();

            string avatar = account.AnhDaiDien;
            if (!string.IsNullOrEmpty(avatar))
                avatar = avatar.TrimStart('[', '"').TrimEnd('"', ']').Replace("\\\\", "/");
            account.AnhDaiDien = avatar;

            var khoas = await _context.Khoas.ToListAsync();
            var gioiTinhs = await _context.Gioitinhs.ToListAsync();
            var nganHangs = await _context.Nganhangs.ToListAsync();
            var monGiaSus = await _context.Mongiasus.ToListAsync();

            var statuts = await _context.Xetduyets.ToListAsync();
            SelectList ddlStatus = new SelectList(statuts, "IdxetDuyet", "TenTrangThai");
            var department = khoas.FirstOrDefault(it => it.Idkhoa == account.Idkhoa);
            if (department == null)
                department = new Khoa();

            var gender = gioiTinhs.FirstOrDefault(it => it.IdgioiTinh == account.IdgioiTinh);
            if (gender == null)
                gender = new Gioitinh();

            var bank = nganHangs.FirstOrDefault(it => it.Id == account.IdnganHang);
            if (bank == null)
                bank = new Nganhang();

            var subject1 = monGiaSus.FirstOrDefault(it => it.IdmonGiaSu == account.IdmonGiaSu1);
            if (subject1 == null)
                subject1 = new Mongiasu();

            var subject2 = monGiaSus.FirstOrDefault(it => it.IdmonGiaSu == account.IdmonGiaSu2);
            if (subject2 == null)
                subject2 = new Mongiasu();

            var model = new TutorViewModel()
            {
                Tutor = account,
                Status = ddlStatus,
                Department = department.TenKhoa,
                Gender = gender.GioiTinh1,
                Bank = bank.TenNganHangHoacViDienTu,
                Subject1 = subject1.TenMonGiaSu,
                Subject2 = subject2.TenMonGiaSu,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailTutor(int id, IFormCollection form, [FromForm] string reason)
        {
            var account = await _context.Taikhoannguoidungs.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                int.TryParse(form["Tutor.IdxetDuyet"], out int idxetDuyet);
                try
                {

                    account.IdxetDuyet = idxetDuyet;
                    account.TrangThaiGiaSu = true;
                    HttpContext.Session.SetString("Email", account.Email);
                    HttpContext.Session.SetString("Name", account.HoTen);

                    _context.Update(account);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "Cập nhật thành công!";
                    TempData["MessageType"] = "success";
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index), new { error = ex.InnerException });
                }
                if (idxetDuyet == 3)
                {
                    return RedirectToAction("SendMail", "ManageTuTors",
                        new { toEmail = account.Email, mailBody = "<b>Chúc mừng! Hồ sơ của bạn đã đủ điều kiện tham gia phỏng vấn, vui lòng theo dõi điện thoại để nhận lịch hẹn phỏng vấn từ chúng tôi!</b>" +
                        "<p style = \"margin: 0%;\">Một lần nữa cảm ơn bạn đã quan tâm và mong muốn trở thành một thành viên của Gia Sư Văn Lang. Hẹn gặp bạn vào buổi phỏng vấn và chúc bạn sẽ có một buổi phỏng " +
                        "vấn thật thành công!<br/>",checkStatus=false});
                }
                else if (idxetDuyet == 4)
                {
                    return RedirectToAction("SendMail", "ManageTuTors",
                        new { toEmail = account.Email, mailBody = "<b>Rất tiếc! Bạn đã không đạt được các tiêu chí để trở thành gia sư của Gia Sư Văn Lang với lý do từ chối là: <i>"+reason+ "</i></b>"+
                        "<p style = \"margin: 0%;\">Một lần nữa cảm ơn bạn đã quan tâm và mong muốn trở thành một thành viên của Gia Sư Văn Lang. Hẹn gặp bạn vào một dịp khác và có cơ hội hợp tác cùng bạn.<br/>"
                        ,checkStatus=false});
                }
                else if (idxetDuyet == 5)
                {
                    return RedirectToAction("SendMail", "ManageTuTors",
                        new { toEmail = account.Email, mailBody = "<b>Chúc mừng! Bạn đã đáp ứng đầy đủ các yêu cầu và chính thức trở thành gia sư của chúng tôi, chào mừng bạn đến với đại gia đình Gia Sư Văn Lang.</b><br/>" +
                        "<b style=\"color: red;\">Vui lòng kiểm tra email để xác nhận quyền tạo buổi học trên ZOOM! Tài khoản sử dụng ZOOM để giảng dạy là tài khoản có địa chỉ mail là: "+account.Email+" .</b>" +
                        "Bây giờ bạn có thể đăng nhập và sử dụng chức năng của gia sư!"+
                        "<p style = \"margin: 0%;\">Một lần nữa cảm ơn bạn đã quan tâm và mong muốn trở thành một thành viên của Gia Sư Văn Lang. Chúc bạn sẽ có những trải nghiệm thật tốt trên Gia Sư Văn Lang với " +
                        "vai trò là gia sư của chúng tôi!.<br/>"
                        ,checkStatus=true});
                }
            }
            return View(account);
        }

        public async Task<IActionResult> UpdateTutor(int id)
        {
            if (HttpContext.Session.GetString("LoginADId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var account = await _context.Taikhoannguoidungs.FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
                return NotFound();

            string avatar = account.AnhDaiDien;
            if (!string.IsNullOrEmpty(avatar))
                avatar = avatar.TrimStart('[', '"').TrimEnd('"', ']').Replace("\\\\", "/");
            account.AnhDaiDien = avatar;

            var khoas = await _context.Khoas.ToListAsync();
            var gioiTinhs = await _context.Gioitinhs.ToListAsync();
            var nganHangs = await _context.Nganhangs.ToListAsync();
            var monGiaSus = await _context.Mongiasus.ToListAsync();

            var statuts = await _context.Xetduyets.ToListAsync();
            SelectList ddlStatus = new SelectList(statuts, "IdxetDuyet", "TenTrangThai");
            var department = khoas.FirstOrDefault(it => it.Idkhoa == account.Idkhoa);
            if (department == null)
                department = new Khoa();

            var gender = gioiTinhs.FirstOrDefault(it => it.IdgioiTinh == account.IdgioiTinh);
            if (gender == null)
                gender = new Gioitinh();

            var bank = nganHangs.FirstOrDefault(it => it.Id == account.IdnganHang);
            if (bank == null)
                bank = new Nganhang();

            var subject1 = monGiaSus.FirstOrDefault(it => it.IdmonGiaSu == account.IdmonGiaSu1);
            if (subject1 == null)
                subject1 = new Mongiasu();

            var subject2 = monGiaSus.FirstOrDefault(it => it.IdmonGiaSu == account.IdmonGiaSu2);
            if (subject2 == null)
                subject2 = new Mongiasu();

            var model = new TutorViewModel()
            {
                Tutor = account,
                Status = ddlStatus,
                Department = department.TenKhoa,
                Gender = gender.GioiTinh1,
                Bank = bank.TenNganHangHoacViDienTu,
                Subject1 = subject1.TenMonGiaSu,
                Subject2 = subject2.TenMonGiaSu,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTutor(int id, IFormCollection form)
        {
            var account = await _context.Taikhoannguoidungs.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    int.TryParse(form["status"], out int status);

                    account.TrangThaiGiaSu = (status > 0);

                    _context.Update(account);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "Cập nhật thành công!";
                    TempData["MessageType"] = "success";
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index), new { error = ex.InnerException });
                }
                return RedirectToAction("Index");
            }
            return View(account);
        }
        public IActionResult SendMail(string toEmail, string mailBody, bool checkStatus)
        {
            string mailTitle = "Gia Sư Văn Lang";
            string fromMail = "giasuvanlang.thongtin@gmail.com";
            string fromEmailPass = "vrzaiqmdiycujvas";
            string email = HttpContext.Session.GetString("Email");
            string name = HttpContext.Session.GetString("Name");
            string bodyMail = "<!DOCTYPE html>" +
        "<html>" +
            "<body>" +
                "<p style = \"margin: 0%;\">" +
                "Xin chào, <b>"+name+"</b><br/>" +
                "Lời đầu tiên, Gia Sư Văn Lang xin cảm ơn bạn đã dành thời gian tìm hiểu và đăng ký trở thành gia sư trên nền tảng của chúng tôi. Qua email này chúng tôi muốn thông báo kết quả xét duyệt hồ sơ như sau:</p>" +

                "<p style = \"margin: 0%;\">"+mailBody+"<br/>" +

               " Trân trọng!<br/>" +
                "<b>Gia Sư Văn Lang</b><br/>" +
                "<b style = \"font-size: 10px;text-align: center; margin: 0%;\"> Bạn nhận được thông báo này vì địa chỉ email "+email+" đang được sử dụng cho " +
                "tài khoản trên trang Gia Sư Văn Lang. Nếu thông tin này không chính xác," +
                "vui lòng bỏ qua và không trả lời lại mail này.<br/>Xin cảm ơn!</b>" +
            "</body>" +
        "</html>";

            //Email and content
            MailMessage message = new MailMessage(new MailAddress(fromMail, mailTitle), new MailAddress(toEmail));
            message.Subject = "[VLUTUTORS] Thông báo kết quả xét duyệt hồ sơ";
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

            if (checkStatus == true)
            {
                TempData["messageCheck"] = "Để cấp quyền cho gia sư, vui lòng thêm email gia sư vào phần quản lý ZOOM!";
            }
            else
            {
                TempData["messageCheck"] = null;
            }

            return RedirectToAction("Index", "ManageTutors");
        }

        public FileResult DownloadFile(string fileName, int tutorId, int id)
        {
            string certificatesPath = id == 1 ? Path.Combine("certificates", tutorId.ToString(), "cer1") : Path.Combine("certificates", tutorId.ToString(), "cer2");

            string path = Path.Combine(this._environment.WebRootPath, certificatesPath, fileName);

            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", fileName);
        }
    }
}