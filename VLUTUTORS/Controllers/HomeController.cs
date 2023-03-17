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
using System.Data;
using VLUTUTORS.Support.Manager;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("LoginId") != 0)
            {
                Console.WriteLine("login id: " + HttpContext.Session.GetInt32("LoginId"));
            }

            var loaiTuVan = new SelectList(_db.Loaituvans.ToList(), "IdLoaiTuVan", "TenLoaiTuVan");
            ViewData["loaiTuVan"] = loaiTuVan;

            int giaSu = 0;
            int hocVien = 0;
            var taiKhoans = await _db.Taikhoannguoidungs.ToListAsync();

            foreach(var taiKhoan in taiKhoans)
            {
                if (taiKhoan.IdxetDuyet == 6)
                {
                    hocVien++;
                }
                if(taiKhoan.IdxetDuyet == 5)
                {
                    giaSu++;
                }
            }

            var monGiaSu = await _db.Mongiasus.ToListAsync();
            ViewData["monGiaSu"] = monGiaSu.Count();
            ViewData["giaSu"] = giaSu;
            ViewData["hocVien"] = hocVien;

            return View();
        }

        [HttpGet]
        public IActionResult RegisterAsTutor()
        {
            string user = HttpContext.Session.GetString("LoginId");
            if (user == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            Taikhoannguoidung dbTaikhoannguoidung = new Taikhoannguoidung();
            var userInfo = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
            dbTaikhoannguoidung.Id = userInfo.Id;
            dbTaikhoannguoidung.HoTen = userInfo.HoTen;
            dbTaikhoannguoidung.Email = userInfo.Email;
            dbTaikhoannguoidung.MatKhau = userInfo.MatKhau;

            dbTaikhoannguoidung.DepartmentItems = new SelectList(_db.Khoas, "Idkhoa", "TenKhoa", dbTaikhoannguoidung.Idkhoa);
            dbTaikhoannguoidung.GenderItems = new SelectList(_db.Gioitinhs, "IdgioiTinh", "GioiTinh1", dbTaikhoannguoidung.IdgioiTinh);
            dbTaikhoannguoidung.BankItems = new SelectList(_db.Nganhangs, "Id", "TenNganHangHoacViDienTu", dbTaikhoannguoidung.IdnganHang);
            dbTaikhoannguoidung.Subject1Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", dbTaikhoannguoidung.IdmonGiaSu1);
            dbTaikhoannguoidung.Subject2Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", dbTaikhoannguoidung.IdmonGiaSu2);
            return View(dbTaikhoannguoidung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterAsTutor([Bind(include: "Id, HoTen, Email, MatKhau, IdgioiTinh, Sdt, NgaySinh, Idkhoa, AnhDaiDien, TrangThaiTaiKhoan, SoTaiKhoan, IdnganHang, GioiThieu, DanhGiaVeViecGiaSu, DiemTrungBinh, IdmonGiaSu1, TenChungChiHoacDiemMon1, ChungChiMon1, GioiThieuVeMonGiaSu1, IdmonGiaSu2, TenChungChiHoacDiemMon2, ChungChiMon2, GioiThieuVeMonGiaSu2, MaXacThuc, XacThuc")]Taikhoannguoidung taikhoannguoidung, List<IFormFile> avatar, List<IFormFile> certificates1, List<IFormFile> certificates2)
        {
            string certificates1Path = Path.Combine("certificates", taikhoannguoidung.Id.ToString(), "cer1");
            string certificates2Path = Path.Combine("certificates", taikhoannguoidung.Id.ToString(), "cer2");
            string avatarPath = Path.Combine("avatars", taikhoannguoidung.Id.ToString());

            if (ModelState.IsValid)
            {
                taikhoannguoidung.TrangThaiTaiKhoan = true;
                taikhoannguoidung.ChungChiMon1 = TutorServices.SaveUploadFiles(this._environment.WebRootPath, certificates1Path, certificates1);
                taikhoannguoidung.ChungChiMon2 = TutorServices.SaveUploadFiles(this._environment.WebRootPath, certificates2Path, certificates2);
                taikhoannguoidung.AnhDaiDien = TutorServices.SaveAvatar(this._environment.WebRootPath, avatarPath, avatar);
                taikhoannguoidung.IdxetDuyet = (int)ApprovalStatus.TRAINING;
                taikhoannguoidung.TrangThaiGiaSu = true;

                _db.Entry(taikhoannguoidung).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
                TempData["message"] = "Hồ sơ đã được gửi đến quản trị viên, để hoàn tất thủ tục bạn cần hoàn thành khóa đào tạo gia sư!";
                return RedirectToAction("Index", "TutorTraining", new { courseName = "" });
            }
            taikhoannguoidung.DepartmentItems = new SelectList(_db.Khoas, "Idkhoa", "TenKhoa", taikhoannguoidung.Idkhoa);
            taikhoannguoidung.GenderItems = new SelectList(_db.Gioitinhs, "IdgioiTinh", "GioiTinh1", taikhoannguoidung.IdgioiTinh);
            taikhoannguoidung.BankItems = new SelectList(_db.Nganhangs, "Id", "TenNganHangHoacViDienTu", taikhoannguoidung.IdnganHang);
            taikhoannguoidung.Subject1Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", taikhoannguoidung.IdmonGiaSu1);
            taikhoannguoidung.Subject2Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", taikhoannguoidung.IdmonGiaSu2);
            Console.WriteLine(" error post: " + ModelState["Idkhoa"].Errors.Count);
            return View(taikhoannguoidung);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Send consulting register

        [HttpPost]
        public async Task<IActionResult> SendConsulting(string HoTen, string Email, string SDT, string NoiDung, int LoaiTuVan)
        {
            try
            {
                Tuvan tuVan = new Tuvan
                {
                    HoVaTen = HoTen,
                    Email = Email,
                    Sdt = SDT,
                    NoiDungTuVan = NoiDung,
                    IdtrangThai = 1,
                    IdLoaiTuVan= LoaiTuVan
                };
                try
                {
                    TempData["message"] = "Gửi đăng ký tư vấn thành công!";
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
        public async Task<IActionResult> Contact()
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
            TempData["message"] = "Gửi phản hồi thành công!";
            return RedirectToAction("Contact", "Home");
        }

        public async Task<IActionResult> AboutUs()
        {
            return View();
        }

    }
}