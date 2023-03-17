using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using VLUTUTORS.Support.Services;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Quản trị viên hệ thống")]

    public class ManageContentController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        private IHostingEnvironment _environment;

        public async Task<IActionResult> Index()
        {
            var noiDung = await _context.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);
            return View(noiDung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int id, [Bind(include: "Id,GioiThieuChanTrang,DiaChi,Sdt,Email,Facebook,GioiThieu,Slogan,AnhGioiThieu,TieuDeGt1,GioiThieu1,TieuDeGt2,GioiThieu2,TieuDeGt3,GioiThieu3")] Noidung noiDung, List<IFormFile> anhGt)
        {
            string imgPath = Path.Combine("images", "about");
            if (ModelState.IsValid)
            {
                try
                {
                    noiDung.AnhGioiThieu = anhGt.Count != 0 ? TutorServices.SaveAvatar(this._environment.WebRootPath, imgPath, anhGt) : noiDung.AnhGioiThieu;
                    TempData["message"] = "Cập nhật thành công!";
                    _context.Entry(noiDung).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
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
