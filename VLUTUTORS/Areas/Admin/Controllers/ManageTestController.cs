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

    public class ManageTestController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            var baiKTs = await _context.Baikiemtras.ToListAsync();
            return View(baiKTs);
        }

        [HttpGet]
        public IActionResult AddQuestion()
        {
            Baikiemtra baikiemtra = new Baikiemtra();

            return View(baikiemtra);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuestion([Bind(include: "IdCauHoi,CauHoi,DapAnA,DapAnB,DapAnC,DapAnD,DapAnDung")] Baikiemtra baikiemtra)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(baikiemtra);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Index");
            }
            return View(baikiemtra);
        }
        [HttpGet]
        public async Task<IActionResult> EditQuestion(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiKiemTra = await _context.Baikiemtras.FirstOrDefaultAsync(m => m.IdCauHoi == id);
            if (baiKiemTra == null)
                return NotFound();
            return View(baiKiemTra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestion(int id, [Bind(include: "IdCauHoi,CauHoi,DapAnA,DapAnB,DapAnC,DapAnD,DapAnDung")] Baikiemtra baikiemtra)
        {
            if (id != baikiemtra.IdCauHoi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(baikiemtra);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Index");
            }
            return View(baikiemtra);
        }
        [HttpPost]
        public IActionResult Delete([FromForm] int hdInput)
        {
            Baikiemtra baiKiemTra = _context.Baikiemtras.Where(p => p.IdCauHoi == hdInput).FirstOrDefault();
            _context.Baikiemtras.Remove(baiKiemTra);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
