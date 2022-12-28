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
            var khoas = await _context.Khoadaotaos.ToListAsync();
            foreach (var baiKT in baiKTs)
            {
                var mon = khoas.FirstOrDefault(it => it.IdKhoaHoc == baiKT.IdKhoaDaoTao);
                if (mon != null)
                    baiKT.KhoaHoc = mon.TenKhoaHoc;
            }
            return View(baiKTs);
        }

        [HttpGet]
        public IActionResult AddQuestion()
        {
            Baikiemtra baikiemtra = new Baikiemtra();

            baikiemtra.KhoaHocs = new SelectList(_context.Khoadaotaos, "IdKhoaHoc", "TenKhoaHoc", baikiemtra.IdKhoaDaoTao);

            return View(baikiemtra);
        }
        [HttpPost]
        public async Task<IActionResult> AddQuestion([Bind(include: "IdBaiKiemTra,IdKhoaDaoTao")] Baikiemtra baikiemtra)
        {
            if (ModelState.IsValid)
            {
                var baiKT = _context.Baikiemtras.AsNoTracking().SingleOrDefault(x => x.IdKhoaDaoTao == baikiemtra.IdKhoaDaoTao);
                if (baiKT != null)
                {
                    return RedirectToAction("Index");
                }
                else
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
            }
            return View(baikiemtra);
        }
        public IActionResult EditQuestion()
        {
            return View();
        }
    }
}
