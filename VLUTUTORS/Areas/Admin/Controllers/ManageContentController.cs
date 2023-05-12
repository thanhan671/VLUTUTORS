using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using VLUTUTORS.Support.Manager;
using VLUTUTORS.Support.Services;
using Microsoft.EntityFrameworkCore;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "1")]

    public class ManageContentController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        private readonly ILogger<ManageContentController> _logger;
        private IHostingEnvironment _environment;

        public ManageContentController(ILogger<ManageContentController> logger, IHostingEnvironment environment)
        {
            _logger = logger;
            this._environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoginADId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var noiDung = await _context.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);
            return View(noiDung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind(include: "Id,GioiThieuChanTrang,DiaChi,Sdt,Email,Facebook,GioiThieu,Slogan,AnhGioiThieu,TieuDeGt1,GioiThieu1,TieuDeGt2,GioiThieu2,TieuDeGt3,GioiThieu3")] Noidung noiDung, List<IFormFile> anhGt)
        {
            string imgPath = Path.Combine("images", "about");

            if (ModelState.IsValid)
            {
                    noiDung.AnhGioiThieu = anhGt.Count != 0 ? TutorServices.SaveAvatar(this._environment.WebRootPath, imgPath, anhGt) : noiDung.AnhGioiThieu;
                    _context.Entry(noiDung).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    TempData["message"] = "Cập nhật thành công!";
                    TempData["MessageType"] = "success";
                return RedirectToAction("Index");
            }
            return View(noiDung);
        }
    }
}
