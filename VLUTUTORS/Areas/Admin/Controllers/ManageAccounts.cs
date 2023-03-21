using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VLUTUTORS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Quản trị viên hệ thống")]

    public class ManageAccounts : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            var taiKhoans = await _context.Taikhoanadmins.ToListAsync();
            var quyens = await _context.Quyens.ToListAsync();
            foreach (var taiKhoan in taiKhoans)
            {
                var quyen = quyens.FirstOrDefault(it => it.IdQuyen == taiKhoan.IdQuyen);
                if (quyen != null)
                    taiKhoan.Quyen = quyen.TenQuyen;
            }
            return View(taiKhoans);
        }

        [HttpGet]
        public IActionResult AddNewAccounts()
        {
            Taikhoanadmin taikhoanadmin = new Taikhoanadmin();

            taikhoanadmin.Quyens = new SelectList(_context.Quyens, "IdQuyen", "TenQuyen", taikhoanadmin.IdQuyen);

            return View(taikhoanadmin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewAccounts([Bind(include:"Id,TaiKhoan,MatKhau,IdQuyen")] Taikhoanadmin taikhoanadmin)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = _context.Taikhoanadmins.AsNoTracking().SingleOrDefault(x => x.TaiKhoan.ToLower() == taikhoanadmin.TaiKhoan.ToLower());
                if(taiKhoan != null)
                {
                    TempData["Message"] = "Tài khoản đã tồn tại, vui lòng kiểm tra lại";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index");
                }
                else
                {
                    try
                    {

                        _context.Add(taikhoanadmin);
                        await _context.SaveChangesAsync();                        
                        TempData["Message"] = "Thêm mới tài khoản thành công!";
                        TempData["MessageType"] = "success";
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(taikhoanadmin);
        }

        [HttpGet]
        public async Task<IActionResult> EditAccounts(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.Taikhoanadmins.FirstOrDefaultAsync(m => m.Id == id);
            if (taiKhoan == null)
                return NotFound();
            var quyens = await _context.Quyens.ToListAsync();
            SelectList ddlStatus = new SelectList(quyens, "IdQuyen", "TenQuyen");
            taiKhoan.Quyens = ddlStatus;
            return View(taiKhoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccounts(int id, [Bind(include: "Id,TaiKhoan,MatKhau,IdQuyen")] Taikhoanadmin taikhoanadmin)
        {
            if (id != taikhoanadmin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taikhoanadmin);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Cập nhật thành công!";
                    TempData["MessageType"] = "success";
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Index");
            }
            return View(taikhoanadmin);
        }
    }
}
