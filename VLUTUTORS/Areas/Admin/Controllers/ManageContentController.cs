using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

    public class ManageContentController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();

        
        public async Task<IActionResult> Index()
        {
            var noiDung = await _context.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);
            return View(noiDung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id, Noidung noiDung)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TempData["message"] = "Cập nhật thành công!";
                    _context.Update(noiDung);
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
