using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using QuickMailer;
using System;
using System.Collections.Generic;
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
                if (account.IdxetDuyet >= 1 && account.IdxetDuyet < 6)
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
                    if (idxetDuyet > 0)
                        account.IdxetDuyet = idxetDuyet;
                        account.TrangThaiGiaSu = true;

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
        public IActionResult SendMail(string Email, string mailBody, string mailSubject)
        {
            string mailTitle = "Gia Sư Văn Lang";
            string fromMail = "giasuvanlang.thongtin@gmail.com";
            string fromEmailPass = "wwxtjmqczzdgwqke";

            //Email and content
            MailMessage message = new MailMessage(new MailAddress(fromMail, mailTitle), new MailAddress(Email));
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


            return RedirectToAction("Login", "Accounts");
        }
    }
}