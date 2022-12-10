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
using VLUTUTORS.Support.Services;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace VLUTUTORS.Controllers
{
    public class HomeController : Controller
    {
        private CP25Team01Context _db = new CP25Team01Context();
        private readonly ILogger<HomeController> _logger;
        private IHostingEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, IHostingEnvironment environment)
        {
            _logger = logger;
            this._environment = environment;
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
            var userInfo = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
            taikhoannguoidung.HoTen = userInfo.HoTen;
            taikhoannguoidung.Email = userInfo.Email;

            taikhoannguoidung.DepartmentItems = _db.Khoas.Select(k => new SelectListItem { Text = k.TenKhoa, Value = k.Idkhoa.ToString() }).ToList();
            taikhoannguoidung.GenderItems = _db.Gioitinhs.Select(k => new SelectListItem { Text = k.GioiTinh1, Value = k.IdgioiTinh.ToString() }).ToList();
            taikhoannguoidung.BankItems = _db.Nganhangs.Select(k => new SelectListItem { Text = k.TenNganHangHoacViDienTu, Value = k.Id.ToString() }).ToList();
            taikhoannguoidung.Subject1Items = _db.Mongiasus.Select(k => new SelectListItem { Text = k.TenMonGiaSu, Value = k.IdmonGiaSu.ToString() }).ToList();
            taikhoannguoidung.Subject2Items = _db.Mongiasus.Select(k => new SelectListItem { Text = k.TenMonGiaSu, Value = k.IdmonGiaSu.ToString() }).ToList();

            return View(taikhoannguoidung);
        }

        [HttpPost]
        public IActionResult RegisterAsTutor([Bind(include: "HoTen, Email, IdgioiTinh, Sdt, NgaySinh, IdKhoa, AnhDaiDien, TrangThaiTaiKhoan, SoTaiKhoan, IdNganHang, GioiThieu, DanhGiaVeViecGiaSu, DiemTrungBinh, IdmonGiaSu1, ChungChiMon1, GioiThieuVeMonGiaSu1, IdmonGiaSu2, ChungChiMon2, GioiThieuVeMonGiaSu2")]Taikhoannguoidung taikhoannguoidung, List<IFormFile> postedFiles)
        {
            Console.WriteLine("run this when click button");
            string path = Path.Combine(this._environment.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                Console.WriteLine("get file name: " + fileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
            }

            var user = TutorServices.FindUserAccountByEmail(taikhoannguoidung.Email);
            TutorServices.UpdateUserInfo(user);

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
                    _db.Add(tuVan);
                    await _db.SaveChangesAsync();
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
                    _db.Add(lienHe);
                    await _db.SaveChangesAsync();
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