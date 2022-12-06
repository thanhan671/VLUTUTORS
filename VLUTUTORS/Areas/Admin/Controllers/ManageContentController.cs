using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageContentController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tuVans = await _context.Tuvans.ToListAsync();
            var trangThais = await _context.Trangthais.ToListAsync();
            foreach (var tuVan in tuVans)
            {
                var trangThai = trangThais.FirstOrDefault(it => it.IdtrangThai == tuVan.IdtrangThai);
                if (trangThai != null)
                    tuVan.TrangThai = trangThai.TrangThai1;
            }
            return View(tuVans);
        }

            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Slogan,GioiThieuChanTrang,DiaChi,Sdt,Email,Facebook,GioiThieu")] Noidung noiDung)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(noiDung);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(noiDung);
            }
            return View(noiDung);
        }
    }
}
