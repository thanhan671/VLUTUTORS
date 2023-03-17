using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using static System.Net.Mime.MediaTypeNames;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Trung tâm hỗ trợ sinh viên")]

    public class ManageConsultingController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();

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
        public async Task<IActionResult> Detail(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tuVan = await _context.Tuvans.FirstOrDefaultAsync(m => m.IdtuVan == id);
            if (tuVan == null)
                return NotFound();
            var trangThais = await _context.Trangthais.ToListAsync();
            SelectList ddlStatus = new SelectList(trangThais, "IdtrangThai", "TrangThai1");
            tuVan.TrangThais = ddlStatus;
            return View(tuVan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail(int id, [Bind("IdtuVan,HoVaTen,Sdt,NoiDungTuVan,IdtrangThai,IdLoaiTuVan, Email")] Tuvan tuVan)
        {
            if (id != tuVan.IdtuVan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["message"] = "Cập nhật thành công!";
                    _context.Update(tuVan);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex) 
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Index");
            }
            return View(tuVan);
        }
    }
}
