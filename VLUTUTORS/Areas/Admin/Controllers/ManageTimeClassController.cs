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
    public class ManageTimeClassController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            var caHoc = await _context.Cahocs.ToListAsync();
            return View(caHoc);
        }

        [HttpGet]
        public IActionResult AddTimeClass()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTimeClass([Bind(include: "IdCaHoc,LoaiCa,GiaTien")] Cahoc caHoc)
        {
            if (ModelState.IsValid)
            {
                var checkCa = _context.Cahocs.AsNoTracking().SingleOrDefault(x => x.LoaiCa.ToString().ToLower() == caHoc.LoaiCa.ToString().ToLower());
                if (checkCa != null)
                {
                    TempData["message"] = "Ca học này đã tồn tại!";
                    return RedirectToAction("AddTimeClass");
                }
                else
                {
                    try
                    {
                        TempData["message"] = "Thêm mới thành công!";
                        _context.Add(caHoc);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(caHoc);
        }
        [HttpGet]
        public async Task<IActionResult> EditTimeClass(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkCa = await _context.Cahocs.FirstOrDefaultAsync(m => m.IdCaHoc == id);
            if (checkCa == null)
                return NotFound();
            return View(checkCa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTimeClass(int id, [Bind(include: "IdCaHoc,LoaiCa,GiaTien")] Cahoc caHoc)
        {
            if (id != caHoc.IdCaHoc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["message"] = "Cập nhật thành công!";
                    _context.Update(caHoc);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTimeClass([FromForm] int timeClassID)
        {
            Cahoc caHoc = _context.Cahocs.Where(p => p.IdCaHoc == timeClassID).FirstOrDefault();
            _context.Cahocs.Remove(caHoc);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

