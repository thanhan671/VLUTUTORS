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

            //List<Khoadaotao> lesson = new List<Khoadaotao>(); // remove this when function complete
            List<Khoadaotao> lesson = _db.Khoadaotaos.ToList();

            //Khoadaotao bai1 = new Khoadaotao(1, "how to marketing", "https://www.youtube.com/embed/" + "4ti_uK60nLk");
            //Khoadaotao bai2 = new Khoadaotao(2, "how to training", "https://www.youtube.com/embed/" + "Owf8emXuzAk");
            //Khoadaotao bai3 = new Khoadaotao(3, "how to make powerpoint", "https://www.youtube.com/embed/" + "XF34-Wu6qWU");
            //lesson.Add(bai1);
            //lesson.Add(bai2);
            //lesson.Add(bai3);
            // can remove above code lines

            Khoadaotao baihoc = new Khoadaotao();
            baihoc.courses = (from l in lesson
                              select l.TenBaiHoc).ToList();
            // generate course name list (keep this)

            //courseName = courseName == null ? bai1.TenBaiHoc : courseName; // can remove this
            courseName = courseName == null ? _db.Khoadaotaos.FirstOrDefault(kdt => kdt.IdBaiHoc == 1).TenBaiHoc : courseName; // use this instead of above code line
            Console.WriteLine("ten bai hoc la gi: " + courseName);
            //ViewData["link"] = lesson.Find(l => l.TenBaiHoc.Equals(courseName)).Link; // can remove this
            ViewData["link"] = _db.Khoadaotaos.FirstOrDefault(l => l.TenBaiHoc.Equals(courseName)).Link; // use this instead of above code line

            return View(baihoc);
        }

        [HttpPost]
        public IActionResult Index(string courseName, int id = 1)
        {
            Console.WriteLine("lesson: " + courseName);
            return RedirectToAction("Index", new { courseName = courseName});
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
