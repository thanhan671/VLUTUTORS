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
    public class ManageConsultingTypeController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();

        public async Task<IActionResult> Index()
        {
            var loaituvans = await _context.Loaituvans.ToListAsync();

            return View(loaituvans);
        }
        public IActionResult AddType()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddType([Bind("TenLoaiTuVan")] Loaituvan loaiTuVan)
        {
            if (ModelState.IsValid)
            {
                var tuVan = _context.Loaituvans.AsNoTracking().SingleOrDefault(x => x.TenLoaiTuVan.ToLower() == loaiTuVan.TenLoaiTuVan.ToLower());
                if (tuVan != null)
                {
                    TempData["message"] = "Loại tư vấn đã tồn tại, vui lòng kiểm tra lại";
                    return RedirectToAction("Index");
                }
                else
                {
                    try
                    {
                        _context.Update(loaiTuVan);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(loaiTuVan);
        }

        public async Task<IActionResult> EditType(int id)
        {
            var loaituvan = await _context.Loaituvans.FindAsync(id);

            return View(loaituvan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditType(int id, [Bind("IdLoaiTuVan,TenLoaiTuVan")] Loaituvan loaituvan)
        {
            if (id != loaituvan.IdLoaiTuVan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaituvan);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Index");
            }
            return View(loaituvan);
        }
    }
}
