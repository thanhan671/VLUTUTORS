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
        private CP25Team01Context db = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            var noiDung = await db.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);
            ViewData["Slogan"] = noiDung.Slogan;
            ViewData["gtChanTrang"] = noiDung.GioiThieuChanTrang;
            ViewData["diaChi"] = noiDung.DiaChi;
            ViewData["Sdt"] = noiDung.Sdt;
            ViewData["Email"] = noiDung.Email;
            ViewData["Fb"] = noiDung.Facebook;
            ViewData["gioiThieu"] = noiDung.GioiThieu;
            ViewData["link"] = "https://www.youtube.com/embed/" + "EIwhQLQhlXE";
            return View();
        }

        public async Task<IActionResult> DoTest()
        {
            var noiDung = await db.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);
            ViewData["Slogan"] = noiDung.Slogan;
            ViewData["gtChanTrang"] = noiDung.GioiThieuChanTrang;
            ViewData["diaChi"] = noiDung.DiaChi;
            ViewData["Sdt"] = noiDung.Sdt;
            ViewData["Email"] = noiDung.Email;
            ViewData["Fb"] = noiDung.Facebook;
            ViewData["gioiThieu"] = noiDung.GioiThieu;
            return View();
        }
    }
}
