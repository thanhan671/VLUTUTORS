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
        public async Task<IActionResult> Index(int id, Noidung noiDung, List<IFormFile> imgAbout)
        {
            string imgPath = Path.Combine("images", "AboutUs");
            if (ModelState.IsValid)
            {
                try
                {
                    noiDung.AnhGioiThieu = imgAbout.Count != 0 ? TutorServices.SaveAvatar(this._environment.WebRootPath, imgPath, imgAbout) : noiDung.AnhGioiThieu;
                    TempData["message"] = "Cập nhật thành công!";
                    _context.Update(noiDung);
                    await _context.SaveChangesAsync();
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
