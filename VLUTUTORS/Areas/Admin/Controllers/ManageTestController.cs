using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public class ManageTestController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            var baiKTs = await _context.Baikiemtras.ToListAsync();
            var khoas = await _context.Khoadaotaos.ToListAsync();
            foreach (var baiKT in baiKTs)
            {
                var mon = khoas.FirstOrDefault(it => it.IdKhoaHoc == baiKT.IdKhoaHoc);
                if (mon != null)
                    baiKT.KhoaHoc = mon.TenKhoaHoc;
            }
            return View(baiKTs);
        }
        public IActionResult DetailTest()
        {
            return View();
        }
        public IActionResult AddTest()
        {
            return View();
        }
        public IActionResult EditTest()
        {
            return View();
        }
        public IActionResult AddQuestion()
        {
            return View();
        }
        public IActionResult EditQuestion()
        {
            return View();
        }
    }
}
