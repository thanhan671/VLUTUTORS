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
        public async Task<IActionResult> AddQuestion([Bind(include: "IdBaiKiemTra,IdKhoaDaoTao")] Baikiemtra baikiemtra)
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
        public IActionResult EditQuestion()
        {
            return View();
        }
    }
}
