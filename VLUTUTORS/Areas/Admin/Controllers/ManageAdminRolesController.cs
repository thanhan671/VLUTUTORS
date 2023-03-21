using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Quản trị viên hệ thống")]
    public class ManageAdminRolesController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();

        public async Task<IActionResult> Index()
        {
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
                    
                    _context.Update(quyen);
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
            return View(quyen);
        }

        public async Task<IActionResult> EditRole(int id)
        {
            var quyen = await _context.Quyens.FindAsync(id);

            return View(quyen);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(int id, [Bind("IdQuyen,TenQuyen")] Quyen quyen)
        {
            if (id != quyen.IdQuyen)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quyen);
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
            return View(quyen);
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
