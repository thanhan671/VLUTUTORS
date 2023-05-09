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
    [Authorize(Roles = "1")]
    public class ManageFacultyController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoginADId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
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
                    TempData["Message"] = "Khoa này đã tồn tại!";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index");
                }
                else
                {
                    try
                    {
                        _context.Add(khoa);
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
            TempData["Message"] = "Tên khoa tối đa 100 ký tự!";
            TempData["MessageType"] = "error";
            return RedirectToAction("Index");
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
        public IActionResult EditFaculty(Khoa khoa)
        {
            if (ModelState.IsValid)
            {
                var checkKhoa = _context.Khoas.AsNoTracking().SingleOrDefault(x => x.TenKhoa.ToLower() == khoa.TenKhoa.ToLower());
                if (checkKhoa != null)
                {
                    TempData["Message"] = "Khoa này đã tồn tại!";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index");
                }
                else
                {
                    try
                    {
                        TempData["Message"] = "Cập nhật thành công!";
                        TempData["MessageType"] = "success";
                        _context.Khoas.Update(khoa);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            TempData["Message"] = "Tên khoa tối đa 100 ký tự!";
            TempData["MessageType"] = "error";
            return RedirectToAction("Index");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFaculty([FromForm] int facultyID)
        {
            Khoa khoa = _context.Khoas.Where(p => p.Idkhoa == facultyID).FirstOrDefault();
            _context.Khoas.Remove(khoa);
            _context.SaveChanges();
            TempData["Message"] = "Xóa thành công!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }
    }
}
