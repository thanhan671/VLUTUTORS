using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "1")]

    public class ManageFeedback : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoginId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var lienHes = await _context.Lienhes.ToListAsync();
            var trangThais = await _context.Trangthais.ToListAsync();
            foreach (var lienHe in lienHes)
            {
                var trangThai = trangThais.FirstOrDefault(it => it.IdtrangThai == lienHe.IdtrangThai);
                if (trangThai != null)
                    lienHe.TrangThai = trangThai.TrangThai1;
            }
            return View(lienHes);
        }
        public async Task<IActionResult> Detail(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lienHe = await _context.Lienhes.FirstOrDefaultAsync(m => m.IdlienHe == id);
            if (lienHe == null)
                return NotFound();
            var trangThais = await _context.Trangthais.ToListAsync();
            SelectList ddlStatus = new SelectList(trangThais, "IdtrangThai", "TrangThai1");
            lienHe.TrangThais = ddlStatus;
            return View(lienHe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail(int id, [Bind("IdlienHe,HoVaTen,Email,MonHoc,Sdt,NoiDung,IdtrangThai")] Lienhe lienHe)
        {
            if (id != lienHe.IdlienHe)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lienHe);
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
            return View(lienHe);
        }
    }
}