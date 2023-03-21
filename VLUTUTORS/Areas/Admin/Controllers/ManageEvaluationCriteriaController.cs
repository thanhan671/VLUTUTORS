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
    public class ManageEvaluationCriteriaController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            var tieuChi = await _context.Tieuchidanhgias.ToListAsync();
            return View(tieuChi);
        }

        [HttpGet]
        public IActionResult AddCriteria()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCriteria([Bind(include: "TieuChi")] Tieuchidanhgia tieuChiDanhGia)
        {
            if (ModelState.IsValid)
            {
                var checkTieuChi = _context.Tieuchidanhgias.AsNoTracking().SingleOrDefault(x => x.TieuChi.ToLower() == tieuChiDanhGia.TieuChi.ToLower());
                if (checkTieuChi != null)
                {
                    TempData["Message"] = "Tiêu chí này đã tồn tại!";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("AddCriteria");
                }
                else
                {
                    try
                    {
                        _context.Add(tieuChiDanhGia);
                        await _context.SaveChangesAsync();
                        TempData["Message"] = "Thêm mới thành công!";
                        TempData["MessageType"] = "success";
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(tieuChiDanhGia);
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
        public async Task<IActionResult> EditCriteria(Tieuchidanhgia tieuChiDanhGia)
        {
            TempData["Message"] = "Cập nhật thành công!";
            TempData["MessageType"] = "success";
            _context.Tieuchidanhgias.Update(tieuChiDanhGia);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCriteria([FromForm] int criteriaID)
        {
            Tieuchidanhgia tieuChi = _context.Tieuchidanhgias.Where(p => p.IdTieuChi == criteriaID).FirstOrDefault();
            _context.Tieuchidanhgias.Remove(tieuChi);
            _context.SaveChanges();
            TempData["Message"] = "Xóa thành công!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }
    }
}
