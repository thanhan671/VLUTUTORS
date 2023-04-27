using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Quản trị viên hệ thống")]

    public class ManageTutorSubjectController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            var monGiaSus = await _context.Mongiasus.ToListAsync();
            return View(monGiaSus);
        }

        [HttpGet]
        public IActionResult AddSubject()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSubject([Bind(include: "IdmonGiaSu,TenMonGiaSu")] Mongiasu mongiasu)
        {
            if (ModelState.IsValid)
            {
                var mon = _context.Mongiasus.AsNoTracking().SingleOrDefault(x => x.TenMonGiaSu.ToLower() == mongiasu.TenMonGiaSu.ToLower());
                if (mon != null)
                {
                    TempData["Message"] = "Môn gia sư đã tồn tại!";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index");
                }
                else
                {
                    try
                    {
                        TempData["Message"] = "Thêm mới thành công!";
                        TempData["MessageType"] = "success";
                        _context.Add(mongiasu);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(mongiasu);
        }
        [HttpGet]
        public async Task<IActionResult> EditSubject(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mon = await _context.Mongiasus.FirstOrDefaultAsync(m => m.IdmonGiaSu == id);
            if (mon == null)
                return NotFound();
            return View(mon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSubject(Mongiasu mongiasu)
        {
            var mon = _context.Mongiasus.AsNoTracking().SingleOrDefault(x => x.TenMonGiaSu.ToLower() == mongiasu.TenMonGiaSu.ToLower());
            if (mon != null)
            {
                TempData["Message"] = "Môn gia sư đã tồn tại!";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Cập nhật thành công!";
                TempData["MessageType"] = "success";
                _context.Mongiasus.Update(mongiasu);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSubject([FromForm] int subjectID)
        {
            Mongiasu monGiaSu = _context.Mongiasus.Where(p => p.IdmonGiaSu == subjectID).FirstOrDefault();
            _context.Mongiasus.Remove(monGiaSu);
            _context.SaveChanges();
            TempData["Message"] = "Xóa thành công!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }
    }
}
