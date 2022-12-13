using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace VLUTUTORS.Controllers
{
    public class HomeController : Controller
    {
        private CP25Team01Context db = new CP25Team01Context();
        private readonly ILogger<HomeController> _logger;
        //private readonly ISession session;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public HomeController(IHttpContextAccessor httpContextAccessor)
        //{
        //    this.session = httpContextAccessor.HttpContext.Session;
        //}

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("LoginId") != 0)
            {
                Console.WriteLine("login id: " + HttpContext.Session.GetInt32("LoginId"));
                //Console.WriteLine(JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo")).HoTen);
            }
            var noiDung = await db.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);
            ViewData["Slogan"] = noiDung.Slogan;
            return View(noiDung);
        }

        public IActionResult RegisterAsTutor()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Send consulting register

        [HttpPost]
        public async Task<IActionResult> SendConsulting(string HoTen, string SDT, string NoiDung)
        {
            try
            {
                Tuvan tuVan = new Tuvan
                {
                    HoVaTen = HoTen,
                    Sdt = SDT,
                    NoiDungTuVan = NoiDung,
                    IdtrangThai = 1
                };
                try
                {
                    db.Add(tuVan);
                    await db.SaveChangesAsync();
                }
                catch
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");

            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Contact()
        {
            var noiDung = await db.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);
            return View(noiDung);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(string Ten, string Email, string Mon, string SDT, string NoiDung)
        {
            try
            {
                Lienhe lienHe = new Lienhe
                {
                    HoVaTen = Ten,
                    Email = Email,
                    MonHoc = Mon,
                    Sdt = SDT,
                    NoiDung = NoiDung,
                    IdtrangThai = 1
                };
                try
                {
                    db.Add(lienHe);
                    await db.SaveChangesAsync();
                }
                catch
                {
                    return RedirectToAction("Contact", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Contact", "Home");

            }
            return RedirectToAction("Contact", "Home");
        }

        public async Task<IActionResult> AboutUs()
        {
            var noiDung = await db.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);
            return View(noiDung);
        }

    }
}