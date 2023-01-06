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
                    return RedirectToAction("AddSubject");
                }
                else
                {
                    try
                    {
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
        public async Task<IActionResult> EditSubject(int id, [Bind(include: "IdmonGiaSu,TenMonGiaSu")] Mongiasu mongiasu)
        {
            if (id != mongiasu.IdmonGiaSu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mongiasu);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
