using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class ManageEvaluationCriteriaController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoginId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var tieuChi = await _context.Tieuchidanhgias.ToListAsync();
            return View(tieuChi);
        }

        [HttpGet]
        public async Task<IActionResult> EditCriteria(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkTieuChi = await _context.Tieuchidanhgias.FirstOrDefaultAsync(m => m.IdTieuChi == id);
            if (checkTieuChi == null)
                return NotFound();
            return View(checkTieuChi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCriteria(Tieuchidanhgia tieuChiDanhGia)
        {
            TempData["Message"] = "Cập nhật thành công!";
            TempData["MessageType"] = "success";
            _context.Tieuchidanhgias.Update(tieuChiDanhGia);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
