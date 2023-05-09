using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "1")]
    public class ManageAdminRolesController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoginADId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var quyens = await _context.Quyens.ToListAsync();

            return View(quyens);
        }
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole([Bind("TenQuyen")] Quyen quyen)
        {
            if (ModelState.IsValid)
            {
                var Quyen = _context.Quyens.AsNoTracking().SingleOrDefault(x => x.TenQuyen.ToLower() == quyen.TenQuyen.ToLower());
                if (Quyen != null)
                {
                    TempData["Message"] = "Quyền đã tồn tại, vui lòng kiểm tra lại!";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index");
                }
                else
                {
                    try
                    {
                        _context.Add(quyen);
                        await _context.SaveChangesAsync();
                        TempData["Message"] = "Thêm mới thành công!";
                        TempData["MessageType"] = "success";
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return RedirectToAction("Index");
                }
            }
            TempData["Message"] = "Tên quyền tối đa 40 ký tự, vui lòng kiểm tra lại!";
            TempData["MessageType"] = "error";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditRole(int id)
        {
            var quyen = await _context.Quyens.FindAsync(id);

            return View(quyen);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRole(Quyen quyen)
        {
            if (ModelState.IsValid)
            {
                var Quyen = _context.Quyens.AsNoTracking().SingleOrDefault(x => x.TenQuyen.ToLower() == quyen.TenQuyen.ToLower());
                if (Quyen != null)
                {
                    TempData["Message"] = "Quyền đã tồn tại, vui lòng kiểm tra lại!";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "Cập nhật thành công!";
                    TempData["MessageType"] = "success";
                    _context.Quyens.Update(quyen);
                    _context.SaveChanges();
                }
            }
            TempData["Message"] = "Tên quyền tối đa 40 ký tự và không chứa ký tự đặc biệt, vui lòng kiểm tra lại!";
            TempData["MessageType"] = "error";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRole([FromForm] int roleID)
        {
            Quyen quyen = _context.Quyens.Where(p => p.IdQuyen == roleID).FirstOrDefault();
            _context.Quyens.Remove(quyen);
            _context.SaveChanges();
            TempData["Message"] = "Xóa thành công!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }
    }
}
