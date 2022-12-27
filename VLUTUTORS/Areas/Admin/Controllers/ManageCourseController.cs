using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    public class ManageCourseController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            var khoaHocs = await _context.Khoadaotaos.ToListAsync();
            var mons = await _context.Mongiasus.ToListAsync();
            foreach (var khoaHoc in khoaHocs)
            {
                var mon = mons.FirstOrDefault(it => it.IdmonGiaSu == khoaHoc.IdMonGiaSu);
                if (mon != null)
                    khoaHoc.MonGiaSu = mon.TenMonGiaSu;
            }
            return View(khoaHocs);
        }

        [HttpGet]
        public IActionResult AddCourse()
        {
            Khoadaotao khoadaotao = new Khoadaotao();

            khoadaotao.MonGiaSus = new SelectList(_context.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", khoadaotao.IdMonGiaSu);

            return View(khoadaotao);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([Bind(include: "IdKhoaHoc,TenKhoaHoc,IdMonGiaSu")] Khoadaotao khoadaotao)
        {
            if (ModelState.IsValid)
            {
                var khoaDaoTao = _context.Khoadaotaos.AsNoTracking().SingleOrDefault(x => x.TenKhoaHoc.ToLower() == khoadaotao.TenKhoaHoc.ToLower());
                if (khoaDaoTao != null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    try
                    {
                        _context.Add(khoadaotao);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(khoadaotao);
        }
        public IActionResult DetailCourse()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditCourse(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khoaDaoTao = await _context.Khoadaotaos.FirstOrDefaultAsync(m => m.IdKhoaHoc == id);
            if (khoaDaoTao == null)
                return NotFound();
            var mons = await _context.Mongiasus.ToListAsync();
            SelectList ddlStatus = new SelectList(mons, "IdmonGiaSu", "TenMonGiaSu");
            khoaDaoTao.MonGiaSus = ddlStatus;
            return View(khoaDaoTao);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(int id, [Bind(include: "IdKhoaHoc,TenKhoaHoc,IdMonGiaSu")] Khoadaotao khoadaotao)
        {
            if (id != khoadaotao.IdKhoaHoc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khoadaotao);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Index");
            }
            return View(khoadaotao);
        }

        public IActionResult AddLesson()
        {
            return View();
        }
        public IActionResult EditLesson()
        {
            return View();
        }
    }
}
