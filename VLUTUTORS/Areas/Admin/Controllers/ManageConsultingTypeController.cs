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
    public class ManageConsultingTypeController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoginId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var loaituvans = await _context.Loaituvans.ToListAsync();

            return View(loaituvans);
        }
        public IActionResult AddType()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddType([Bind("TenLoaiTuVan")] Loaituvan loaiTuVan)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Vui lòng điền thông tin!";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }
            else
            {
                var tuVan = _context.Loaituvans.AsNoTracking().SingleOrDefault(x => x.TenLoaiTuVan.ToLower() == loaiTuVan.TenLoaiTuVan.ToLower());
                if (tuVan != null)
                {
                    TempData["Message"] = "Loại tư vấn đã tồn tại, vui lòng kiểm tra lại";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index");
                }
                else
                {
                    try
                    {
                        _context.Update(loaiTuVan);
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
        }

        public IActionResult EditType(int id)
        {
            Loaituvan loaituvan = _context.Loaituvans.Where(p => p.IdLoaiTuVan == id).FirstOrDefault();
            return PartialView("_ConsultingType", loaituvan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditType(Loaituvan loaituvan)
        {

            TempData["Message"] = "Cập nhật thành công!";
            TempData["MessageType"] = "success";
            _context.Loaituvans.Update(loaituvan);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConsulting([FromForm] int consultingID)
        {
            Loaituvan loaiTuVan = _context.Loaituvans.Where(p => p.IdLoaiTuVan == consultingID).FirstOrDefault();
            _context.Loaituvans.Remove(loaiTuVan);
            _context.SaveChanges();
            TempData["Message"] = "Xóa thành công!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }
    }
}
