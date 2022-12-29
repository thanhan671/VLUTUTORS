using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Quản trị viên hệ thống")]

    public class ManageCourseController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            var khoaHocs = await _context.Khoadaotaos.ToListAsync();
            return View(khoaHocs);
        }

        [HttpGet]
        public IActionResult AddLesson()
        {
            Khoadaotao khoadaotao = new Khoadaotao();

            return View(khoadaotao);
        }

        [HttpPost]
        public async Task<IActionResult> AddLesson([Bind(include: "IdBaiHoc,TenBaiHoc,Link")] Khoadaotao khoadaotao)
        {
            if (ModelState.IsValid)
            {
                var khoaDaoTao = _context.Khoadaotaos.AsNoTracking().SingleOrDefault(x => x.TenBaiHoc.ToLower() == khoadaotao.TenBaiHoc.ToLower());
                if (khoaDaoTao != null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    try
                    {
                        string newLink = khoadaotao.Link.Replace(@"https://www.youtube.com/watch?v=", @"https://www.youtube.com/embed/");
                        khoadaotao.Link = newLink;
                        _context.Add(khoadaotao);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(khoadaotao);
        }

        [HttpGet]
        public async Task<IActionResult> EditLesson(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khoaDaoTao = await _context.Khoadaotaos.FirstOrDefaultAsync(m => m.IdBaiHoc == id);
            if (khoaDaoTao == null)
                return NotFound();
            return View(khoaDaoTao);
        }

        [HttpPost]
        public async Task<IActionResult> EditLesson(int id, [Bind(include: "IdBaiHoc,TenBaiHoc,Link")] Khoadaotao khoadaotao)
        {
            if (id != khoadaotao.IdBaiHoc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string newLink = khoadaotao.Link.Replace(@"https://www.youtube.com/watch?v=", @"https://www.youtube.com/embed/");
                    khoadaotao.Link = newLink;
                    _context.Update(khoadaotao);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Index");
            }
            return View(khoadaotao);
        }

        [HttpPost]
        public IActionResult DeleteLesson(int id)
        {
            Khoadaotao khoadaotao = _context.Khoadaotaos.Where(p => p.IdBaiHoc == id).FirstOrDefault();
            _context.Khoadaotaos.Remove(khoadaotao);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
