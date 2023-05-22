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
using Microsoft.AspNetCore.Http;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "1")]

    public class ManageAccounts : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoginADId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var taiKhoans = await _context.Taikhoanadmins.ToListAsync();
            var quyens = await _context.Quyens.ToListAsync();
            foreach (var taiKhoan in taiKhoans)
            {
                var quyen = quyens.FirstOrDefault(it => it.IdQuyen == taiKhoan.IdQuyen);
                if (quyen != null)
                    taiKhoan.Quyen = quyen.TenQuyen;
            }
            Taikhoanadmin taikhoanadmin = new Taikhoanadmin();
            taikhoanadmin.listQuyen = new SelectList(_context.Quyens, "IdQuyen", "TenQuyen", taikhoanadmin.IdQuyen);

            Tuple<Taikhoanadmin, IEnumerable<Taikhoanadmin>> turple = new Tuple<Taikhoanadmin, IEnumerable<Taikhoanadmin>>(taikhoanadmin, taiKhoans.AsEnumerable());

            return View(turple);
        }

        [HttpGet]
        public IActionResult AddNewAccounts()
        {
            Taikhoanadmin taikhoanadmin = new Taikhoanadmin();

            taikhoanadmin.listQuyen = new SelectList(_context.Quyens, "IdQuyen", "TenQuyen", taikhoanadmin.IdQuyen);

            return View(taikhoanadmin);
            //return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewAccounts([Bind(include: "Id,TaiKhoan,MatKhau,IdQuyen")] Taikhoanadmin taikhoanadmin)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = _context.Taikhoanadmins.AsNoTracking().SingleOrDefault(x => x.TaiKhoan.ToLower() == taikhoanadmin.TaiKhoan.ToLower());
                if (taiKhoan != null)
                {
                    TempData["Message"] = "Tài khoản đã tồn tại, vui lòng kiểm tra lại";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index");
                }
                else
                {
                    if(taikhoanadmin.MatKhau.Length >= 6)
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
                    else
                    {
                        TempData["Message"] = "Mật khẩu phải từ 6 ký tự, vui lòng kiểm tra lại!";
                        TempData["MessageType"] = "error";
                    }
                }
            }
            TempData["Message"] = "Tên đăng nhập phải từ 5-30 ký tự, vui lòng kiểm tra lại!";
            TempData["MessageType"] = "error";
            var quyens = await _context.Quyens.ToListAsync();
            SelectList ddlStatus = new SelectList(quyens, "IdQuyen", "TenQuyen");
            taikhoanadmin.listQuyen = ddlStatus;
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
            taiKhoan.listQuyen = ddlStatus;
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

                if (taikhoanadmin.MatKhau.Length >= 6)
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
                else
                {
                    TempData["Message"] = "Mật khẩu phải từ 6 ký tự, vui lòng kiểm tra lại";
                    TempData["MessageType"] = "error";
                }
            }
            var quyens = await _context.Quyens.ToListAsync();
            SelectList ddlStatus = new SelectList(quyens, "IdQuyen", "TenQuyen");
            taikhoanadmin.listQuyen = ddlStatus;
            return View(taikhoanadmin);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAccounts([FromForm] int accountID)
        {
            Taikhoanadmin taikhoanadmin = _context.Taikhoanadmins.Where(p => p.Id == accountID).FirstOrDefault();
            _context.Taikhoanadmins.Remove(taikhoanadmin);
            _context.SaveChanges();
            TempData["Message"] = "Xóa thành công!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }
    }
}
