using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;

namespace VLUTUTORS.Controllers
{
    public class TutorTrainingController : Controller
    {
        private CP25Team01Context _db = new CP25Team01Context();

        [HttpGet]
        public async Task<IActionResult> Index(string courseName)
        {
            var noiDung = await _db.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);

            ViewData["Slogan"] = noiDung.Slogan;
            ViewData["gtChanTrang"] = noiDung.GioiThieuChanTrang;
            ViewData["diaChi"] = noiDung.DiaChi;
            ViewData["Sdt"] = noiDung.Sdt;
            ViewData["Email"] = noiDung.Email;
            ViewData["Fb"] = noiDung.Facebook;
            ViewData["gioiThieu"] = noiDung.GioiThieu;

            List<Khoadaotao> lesson = _db.Khoadaotaos.ToList();

            Khoadaotao baihoc = new Khoadaotao();
            baihoc.courses = (from l in lesson
                              select l.TenBaiHoc).ToList();

            courseName = courseName == null ? _db.Khoadaotaos.FirstOrDefault(kdt => kdt.IdBaiHoc == 1).TenBaiHoc : courseName; // use this instead of above code line
            Console.WriteLine("ten bai hoc la gi: " + courseName);

            ViewData["link"] = _db.Khoadaotaos.FirstOrDefault(l => l.TenBaiHoc.Equals(courseName)).Link; // use this instead of above code line

            return View(baihoc);
        }

        [HttpPost]
        public IActionResult Index(string courseName, int id = 1)
        {
            Console.WriteLine("lesson: " + courseName);
            return RedirectToAction("Index", new { courseName = courseName });
        }

        public async Task<IActionResult> DoTest()
        {
            var noiDung = await _db.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);
            ViewData["Slogan"] = noiDung.Slogan;
            ViewData["gtChanTrang"] = noiDung.GioiThieuChanTrang;
            ViewData["diaChi"] = noiDung.DiaChi;
            ViewData["Sdt"] = noiDung.Sdt;
            ViewData["Email"] = noiDung.Email;
            ViewData["Fb"] = noiDung.Facebook;
            ViewData["gioiThieu"] = noiDung.GioiThieu;
            return View();
        }

        private void LoadCourse()
        {

        }
    }
}