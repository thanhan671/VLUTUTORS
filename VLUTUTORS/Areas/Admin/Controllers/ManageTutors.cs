using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageTutors : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();

        public async Task<IActionResult> Index()
        {
            var model = new TutorListModel();
            var awaitTutors = new List<TutorViewModel>();
            var approvedTutors = new List<TutorViewModel>();

            var accounts = await _context.Taikhoannguoidungs.ToListAsync();
            var xetduyets = await _context.Xetduyets.ToListAsync();
            var monGiaSus = await _context.Mongiasus.ToListAsync(); 

            foreach (var account in accounts)
            {
                if (account.IdxetDuyet >= 2 && account.IdxetDuyet < 7)
                {
                    var awaitApproveStatus = xetduyets.FirstOrDefault(it => it.IdxetDuyet == account.IdxetDuyet);
                    if (awaitApproveStatus == null)
                        awaitApproveStatus = new Xetduyet();
                    
                    var subject = monGiaSus.FirstOrDefault(it => it.IdmonGiaSu == account.IdmonGiaSu1);
                    if (subject == null)
                        subject = new Mongiasu();

                    awaitTutors.Add(new TutorViewModel()
                    {
                        Tutor = account,
                        Subject1 = subject.TenMonGiaSu,
                        ApprovedStatus = awaitApproveStatus.TenTrangThai
                    }) ;
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

                        approvedTutors.Add(new TutorViewModel()
                        {
                            Tutor = account,
                            Subject1 = subject.TenMonGiaSu,
                            ApprovedStatus = approvedStatus.TenTrangThai
                        });
                    }
                }
            }

            model.approvedTutors = approvedTutors;


            return View(model);
        }

        public async Task<IActionResult> DetailTutor(int id)
        {
            var account = await _context.Taikhoannguoidungs.FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
                return NotFound();

            string avatar = account.AnhDaiDien;
            if(!string.IsNullOrEmpty(avatar))
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
                try
                {
                    int.TryParse(form["Tutor.IdxetDuyet"], out int idxetDuyet);
                    if(idxetDuyet > 0)
                        account.IdxetDuyet = idxetDuyet;

                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index), new { error = ex.InnerException});
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
                    bool.TryParse(form["Status"], out bool status);
                    account.TrangThaiTaiKhoan = status;

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
    }
}
