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
    [Authorize(Roles = "Quản trị viên gia sư")]
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
                    if (subject == null)
                        subject = new Mongiasu();
                    if (
                        string.IsNullOrEmpty(search) ||
                        (!string.IsNullOrEmpty(search) && (account.HoTen.ToLower().Contains(search.ToLower()) || subject.TenMonGiaSu.ToLower().Contains(search.ToLower())))
                    )
                    {
                        awaitTutors.Add(new TutorViewModel()
                        {
                            Tutor = account,
                            Subject1 = subject.TenMonGiaSu,
                            ApprovedStatus = awaitApproveStatus.TenTrangThai
                        });

                    }

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
                        if (subject == null)
                            subject = new Mongiasu();

                        if (
                           string.IsNullOrEmpty(search) ||
                           (!string.IsNullOrEmpty(search) && (account.HoTen.ToLower().Contains(search.ToLower()) || subject.TenMonGiaSu.ToLower().Contains(search.ToLower())))
                        )
                        {
                            approvedTutors.Add(new TutorViewModel()
                            {
                                Tutor = account,
                                Subject1 = subject.TenMonGiaSu,
                                ApprovedStatus = approvedStatus.TenTrangThai
                            });
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
        public async Task<IActionResult> DetailTutor(int id, IFormCollection form)
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
                    if (idxetDuyet > 0 && idxetDuyet != 3)
                    {
                        account.IdxetDuyet = idxetDuyet;
                        account.TrangThaiGiaSu = true;

                        _context.Update(account);
                        await _context.SaveChangesAsync();
                    }
                    else if (idxetDuyet == 3)
                    {
                        account.IdxetDuyet = idxetDuyet;
                        account.TrangThaiGiaSu = true;
                        account.DiemBaiTest = null;

                        _context.Update(account);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index), new { error = ex.InnerException });
                }
                if (idxetDuyet == 3)
                {
                    return RedirectToAction("SendMail", "ManageTuTors",
                        new { toEmail = account.Email, mailBody = "Rất tiếc! Điểm bài kiểm tra của bạn chưa đủ để xét duyệt, vui lòng thực hiện lại bài kiểm tra!", mailSubject = "Thông báo kết quả xét duyệt hồ sơ" });
                }
                else if (idxetDuyet == 4)
                {
                    return RedirectToAction("SendMail", "ManageTuTors",
                        new { toEmail = account.Email, mailBody = "Chúc mừng! Hồ sơ của bạn đã đủ điều kiện tham gia phỏng vấn, vui lòng theo dõi điện thoại để nhận lịch hẹn phỏng vấn!", mailSubject = "Thông báo kết quả xét duyệt hồ sơ" });
                }
                else if (idxetDuyet == 5)
                {
                    return RedirectToAction("SendMail", "ManageTuTors",
                        new { toEmail = account.Email, mailBody = "Rất tiếc! Bạn đã không đạt được các tiêu chí để trở thành gia sư của Văn Lang, hẹn gặp lại bạn dịp khác!", mailSubject = "Thông báo kết quả xét duyệt hồ sơ" });
                }
                else if (idxetDuyet == 6)
                {
                    return RedirectToAction("SendMail", "ManageTuTors",
                        new { toEmail = account.Email, mailBody = "Chúc mừng! Bạn đã chính thức trở thành gia sư của Văn Lang, bây giờ bạn có thể đăng nhập và sử dụng chức năng của gia sư!", mailSubject = "Thông báo kết quả xét duyệt hồ sơ" });
                }
            }
            return View(account);
        }

        public async Task<IActionResult> UpdateTutor(int id)
        {
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
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index), new { error = ex.InnerException });
                }
                return RedirectToAction("Index");
            }
            return View(account);
        }
        public IActionResult SendMail(string toEmail, string mailBody, string mailSubject)
        {
            string mailTitle = "Gia Sư Văn Lang";
            string fromMail = "giasuvanlang.thongtin@gmail.com";
            string fromEmailPass = "wwxtjmqczzdgwqke";

            //Email and content
            MailMessage message = new MailMessage(new MailAddress(fromMail, mailTitle), new MailAddress(toEmail));
            message.Subject = mailSubject;
            message.Body = mailBody;

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