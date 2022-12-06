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
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("LoginId") != 0)
            {
                Console.WriteLine("login id: " + HttpContext.Session.GetInt32("LoginId"));
                //Console.WriteLine(JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo")).HoTen);
            }
            return View();
        }

        [HttpGet]
        public IActionResult RegisterAsTutor()
        {
            Taikhoannguoidung taikhoannguoidung = new Taikhoannguoidung();
            taikhoannguoidung.KhoaItems = db.Khoas.Select(k => new SelectListItem { Text = k.TenKhoa, Value = k.Idkhoa.ToString()}).ToList();

            //var khoaData = db.Khoas;
            foreach (var item in taikhoannguoidung.KhoaItems)
            {
                Console.WriteLine("item: " + item.Text);
            }
            //taikhoannguoidung.Khoas = db.Khoas.Select(x => new Khoa{ Idkhoa = x.Idkhoa, TenKhoa = x.TenKhoa }).ToList();
            return View(taikhoannguoidung);
        }

        [HttpPost]
        public IActionResult RegisterAsTutor([Bind(include:"Email")]Taikhoannguoidung taikhoannguoidung)
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
        public IActionResult Contact()
        {
            return View();
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

        public IActionResult AboutUs()
        {
            return View();
        }

    }
}