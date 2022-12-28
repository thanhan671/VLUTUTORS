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
            //ViewData["link"] = "https://www.youtube.com/embed/" + "EIwhQLQhlXE";

            List<Baihoc> lesson = new List<Baihoc>();
            Baihoc bai1 = new Baihoc(1, "how to marketing", "https://www.youtube.com/embed/" + "4ti_uK60nLk");
            Baihoc bai2 = new Baihoc(2, "how to training", "https://www.youtube.com/embed/" + "Owf8emXuzAk");
            Baihoc bai3 = new Baihoc(3, "how to make powerpoint", "https://www.youtube.com/embed/" + "XF34-Wu6qWU");
            lesson.Add(bai1);
            lesson.Add(bai2);
            lesson.Add(bai3);
            Baihoc baihoc = new Baihoc();
            baihoc.courses = (from l in lesson
                              select l.TenBaiHoc).ToList();

            courseName = courseName == null ? bai1.TenBaiHoc : courseName;
            Console.WriteLine("ten bai hoc la gi: " + courseName);
            ViewData["link"] = lesson.Find(l => l.TenBaiHoc.Equals(courseName)).LinkBaiHoc;

            return View(baihoc);
        }

        [HttpPost]
        public IActionResult Index(string courseButton, int id = 1)
        {

            Console.WriteLine("lesson: " + courseButton);
            //int id = courseButton.
            return RedirectToAction("Index", new { courseName = courseButton});
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
