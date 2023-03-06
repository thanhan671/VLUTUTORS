﻿using Microsoft.AspNetCore.Authorization;
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
                    TempData["message"] = "Tiêu chí này đã tồn tại!";
                    return RedirectToAction("AddCriteria");
                }
                else
                {
                    try
                    {
                        TempData["message"] = "Thêm mới thành công!";
                        _context.Add(tieuChiDanhGia);
                        await _context.SaveChangesAsync();
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
        public async Task<IActionResult> EditCriteria(int id, [Bind(include: "IdTieuChi,TieuChi")] Tieuchidanhgia tieuChiDanhGia)
        {
            if (id != tieuChiDanhGia.IdTieuChi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["message"] = "Cập nhật thành công!";
                    _context.Update(tieuChiDanhGia);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCriteria([FromForm] int criteriaID)
        {
            Tieuchidanhgia tieuChi = _context.Tieuchidanhgias.Where(p => p.IdTieuChi == criteriaID).FirstOrDefault();
            _context.Tieuchidanhgias.Remove(tieuChi);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
