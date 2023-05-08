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
    public class ManageTimeClassController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoginADId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var caHoc = await _context.Cahocs.ToListAsync();
            var phiDay = _context.Phidays.FirstOrDefault(m => m.Id == 1);
            Tuple<IEnumerable<Cahoc>, Phiday> turple = new Tuple<IEnumerable<Cahoc>, Phiday>(caHoc.AsEnumerable(), phiDay);
            return View(turple);
        }

        [HttpGet]
        public IActionResult AddTimeClass()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTimeClass([Bind(include: "IdCaHoc,LoaiCa,GiaTien")] Cahoc caHoc)
        {
            if (ModelState.IsValid)
            {
                var checkCa = _context.Cahocs.AsNoTracking().SingleOrDefault(x => x.LoaiCa.ToString().ToLower() == caHoc.LoaiCa.ToString().ToLower());
                if (checkCa != null)
                {
                    TempData["Message"] = "Loại ca học này đã tồn tại!";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index");
                }
                else
                {
                    if (caHoc.LoaiCa > 0 && caHoc.GiaTien > 0)
                    {
                        try
                        {
                            TempData["Message"] = "Thêm mới thành công!";
                            TempData["MessageType"] = "success";
                            _context.Add(caHoc);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Message"] = "Loại ca và số tiền phải lớn hơn 0!";
                        TempData["MessageType"] = "error";
                        return RedirectToAction("Index");
                    }

                }
            }
            return View(caHoc);
        }
        [HttpGet]
        public async Task<IActionResult> EditTimeClass(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkCa = await _context.Cahocs.FirstOrDefaultAsync(m => m.IdCaHoc == id);
            if (checkCa == null)
                return NotFound();
            return View(checkCa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTimeClass(Cahoc caHoc)
        {
            if (ModelState.IsValid)
            {
                if (caHoc.LoaiCa > 0 && caHoc.GiaTien > 0)
                {
                    TempData["Message"] = "Cập nhật thành công!";
                    TempData["MessageType"] = "success";
                    _context.Cahocs.Update(caHoc);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "Loại ca và số tiền phải lớn hơn 0!";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index");
                }
            }
            return View(caHoc);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTimeClass([FromForm] int timeClassID)
        {
            Cahoc caHoc = _context.Cahocs.Where(p => p.IdCaHoc == timeClassID).FirstOrDefault();
            _context.Cahocs.Remove(caHoc);
            _context.SaveChanges();
            TempData["Message"] = "Xóa thành công!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePrice(Phiday phiday)
        {
            if (phiday.ChietKhau > 0)
            {
                _context.Phidays.Update(phiday);
                _context.SaveChanges();
                TempData["Message"] = "Cập nhật thành công!";
                TempData["MessageType"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Chiết khấu phải lớn hơn 0!";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }
        }
    }
}

