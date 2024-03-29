﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Authorize(Roles = "1")]

    public class ManageTestController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoginADId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuestion([Bind(include: "IdCauHoi,CauHoi,DapAnA,DapAnB,DapAnC,DapAnD,DapAnDung")] Baikiemtra baikiemtra)
        {
            if (ModelState.IsValid)
            {
                var checkQues = _context.Baikiemtras.AsNoTracking().SingleOrDefault(x => x.CauHoi.ToLower() == baikiemtra.CauHoi.ToLower());
                if (checkQues != null)
                {
                    TempData["Message"] = "Câu hỏi này đã tồn tại!";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("AddQuestion");
                }
                else
                {
                    try
                    {
                        _context.Add(baikiemtra);
                        await _context.SaveChangesAsync();
                        TempData["Message"] = "Thêm thành công!";
                        TempData["MessageType"] = "success";
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(baikiemtra);
        }
        [HttpGet]
        public async Task<IActionResult> EditQuestion(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiKiemTra = await _context.Baikiemtras.FirstOrDefaultAsync(m => m.IdCauHoi == id);
            if (baiKiemTra == null)
                return NotFound();
            return View(baiKiemTra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestion(int id, [Bind(include: "IdCauHoi,CauHoi,DapAnA,DapAnB,DapAnC,DapAnD,DapAnDung")] Baikiemtra baikiemtra)
        {
            if (id != baikiemtra.IdCauHoi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(baikiemtra);
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
            return View(baikiemtra);
        }
        [HttpPost]
        public IActionResult Delete([FromForm] int hdInput)
        {
            Baikiemtra baiKiemTra = _context.Baikiemtras.Where(p => p.IdCauHoi == hdInput).FirstOrDefault();
            _context.Baikiemtras.Remove(baiKiemTra);
            _context.SaveChanges();
            TempData["Message"] = "Xóa thành công!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }
    }
}
