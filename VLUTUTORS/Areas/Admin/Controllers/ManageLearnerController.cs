using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Quản trị viên người học")]
    public class ManageLearnerController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();

        public async Task<IActionResult> Index()
        {
            var taiKhoans = await _context.Taikhoannguoidungs.Where(it=> it.IdxetDuyet == 6).ToListAsync();
            return View(taiKhoans);
        }
        public async Task<IActionResult> DetailLearner(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.Taikhoannguoidungs.FirstOrDefaultAsync(m => m.Id == id);
            if (taiKhoan == null || (taiKhoan != null && taiKhoan.IdxetDuyet != 6))
                return NotFound();
            return View(taiKhoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailLearner(int? id = -1, int? trangThai = 1)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = await _context.Taikhoannguoidungs.FirstOrDefaultAsync(m => m.Id == id);
                if (taiKhoan == null || (taiKhoan != null && taiKhoan.IdxetDuyet != 6))
                    return NotFound();

                if (taiKhoan != null)
                {
                    taiKhoan.TrangThaiTaiKhoan = Convert.ToBoolean(trangThai);

                    _context.Update(taiKhoan);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Cập nhật thành công!";
                    TempData["MessageType"] = "success";

                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
