﻿using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "1")]
    public class ManageEvaluationCriteriaController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoginADId") == null)
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
            if (ModelState.IsValid)
            {
                var checkTC = _context.Tieuchidanhgias.AsNoTracking().SingleOrDefault(x => (x.TieuChi.ToLower() == tieuChiDanhGia.TieuChi.ToLower())&&(x.DanhCho.ToLower()==tieuChiDanhGia.DanhCho.ToLower()));
                if (checkTC != null)
                {
                    TempData["Message"] = "Tiêu chí này đã tồn tại!";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "Cập nhật thành công!";
                    TempData["MessageType"] = "success";
                    _context.Tieuchidanhgias.Update(tieuChiDanhGia);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
