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
    public class ManageFacultyController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            var khoa = await _context.Khoas.ToListAsync();
            return View(khoa);
        }

        [HttpGet]
        public IActionResult AddFaculty()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFaculty([Bind(include: "Idkhoa,TenKhoa")] Khoa khoa)
        {
            if (ModelState.IsValid)
            {
                var checkKhoa = _context.Khoas.AsNoTracking().SingleOrDefault(x => x.TenKhoa.ToLower() == khoa.TenKhoa.ToLower());
                if (checkKhoa != null)
                {
                    TempData["message"] = "Khoa này đã tồn tại!";
                    return RedirectToAction("AddSubject");
                }
                else
                {
                    try
                    {
                        TempData["message"] = "Thêm mới thành công!";
                        _context.Add(khoa);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(khoa);
        }
        [HttpGet]
        public async Task<IActionResult> EditFaculty(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkKhoa = await _context.Khoas.FirstOrDefaultAsync(m => m.Idkhoa == id);
            if (checkKhoa == null)
                return NotFound();
            return View(checkKhoa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFaculty(int id, [Bind(include: "Idkhoa,TenKhoa")] Khoa khoa)
        {
            if (id != khoa.Idkhoa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["message"] = "Cập nhật thành công!";
                    _context.Update(khoa);
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
