﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using VLUTUTORS.Support.Manager;
using VLUTUTORS.Support.Services;

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

            Tuvan tuVan = new Tuvan();

            var loaiTuVan = new SelectList(_db.Loaituvans.ToList(), "IdLoaiTuVan", "TenLoaiTuVan");
            ViewData["loaiTuVan"] = loaiTuVan;

            int giaSu = 0;
            int hocVien = 0;
            var taiKhoans = await _db.Taikhoannguoidungs.ToListAsync();

            foreach (var taiKhoan in taiKhoans)
            {
                if (taiKhoan.IdxetDuyet == 6)
                {
                    hocVien++;
                }
                if (taiKhoan.IdxetDuyet == 5)
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
        public IActionResult RegisterAsTutor([Bind(include: "Id, HoTen, Email, MatKhau, IdgioiTinh, Sdt, NgaySinh, Idkhoa, AnhDaiDien, TrangThaiTaiKhoan, SoTaiKhoan, IdnganHang, GioiThieu, DanhGiaVeViecGiaSu, DiemTrungBinh, IdmonGiaSu1, TenChungChiHoacDiemMon1, ChungChiMon1, GioiThieuVeMonGiaSu1, IdmonGiaSu2, TenChungChiHoacDiemMon2, ChungChiMon2, GioiThieuVeMonGiaSu2, MaXacThuc, XacThuc")] Taikhoannguoidung taikhoannguoidung, List<IFormFile> avatar, List<IFormFile> certificates1, List<IFormFile> certificates2)
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendConsulting(Tuvan tuVan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TempData["Message"] = "Gửi đăng ký tư vấn thành công!";
                    TempData["MessageType"] = "success";
                    _db.Add(tuVan);
                    await _db.SaveChangesAsync();
                }
                catch
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["Message"] = "Vui lòng điền đúng và đủ thông tin!";
                TempData["MessageType"] = "error";
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(Lienhe lienHe)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TempData["Message"] = "Gửi đăng ký tư vấn thành công!";
                    TempData["MessageType"] = "success";
                    _db.Add(lienHe);
                    await _db.SaveChangesAsync();
                }
                catch
                {
                    return RedirectToAction("Contact", "Home");
                }
            }
            else
            {
                TempData["Message"] = "Vui lòng điền đúng và đủ thông tin!";
                TempData["MessageType"] = "error";
                return View(lienHe);
            }

            return RedirectToAction("Contact", "Home");
        }

        public async Task<IActionResult> AboutUs()
        {
            return View();
        }


        #region Method Report

        /// <summary>
        /// Thống kê giờ dạy đã hoàn thành của tất cả gia sư hiện có của website.
        /// </summary>
        /// <returns>long</returns>
        private async Task<long> GetReportTeachingHours()
        {
            const int DA_XET_DUYET = 5;

            return await (from caDay in _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay <= DateTime.Now.Date)
                          join giaSu in _db.Taikhoannguoidungs.Where(x => x.IdxetDuyet == DA_XET_DUYET)
                          on caDay.IdnguoiDay equals giaSu.Id
                          select caDay).SumAsync(x => ((x.GioKetThuc - x.GioBatDau) * 60) + (x.PhutKetThuc - x.PhutBatDau)) / 60;
        }

        /// <summary>
        /// Thống kê số ca học đã diễn ra.
        /// </summary>
        /// <returns>long</returns>
        private async Task<long> GetReportLessonAlready()
        {
            return await _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay <= DateTime.Now.Date).LongCountAsync();
        }

        /// <summary>
        /// Thống kê người học hiện có.
        /// </summary>
        /// <returns>long</returns>
        private async Task<long> GetReportLearner()
        {
            const int KHONG_XET_DUYET = 6;
            return await _db.Taikhoannguoidungs.Where(x => x.IdxetDuyet == KHONG_XET_DUYET).LongCountAsync();
        }

        /// <summary>
        /// Thông kê gia sư hiện có.
        /// </summary>
        /// <returns>long</returns>
        private async Task<long> GetReportTutor()
        {
            const int DA_XET_DUYET = 5;
            return await _db.Taikhoannguoidungs.Where(x => x.IdxetDuyet == DA_XET_DUYET).LongCountAsync();
        }

        /// <summary>
        /// Thông kê số lượng người dùng.
        /// </summary>
        /// <returns>long</returns>
        private async Task<long> GetReportUser()
        {
            return await _db.Taikhoannguoidungs.LongCountAsync();
        }

        /// <summary>
        /// Thông kê tổng tiền thu được.
        /// </summary>
        /// <returns>double</returns>
        private async Task<double> GetReportMoney()
        {
            return await (from caDay in _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay.Date <= DateTime.Now.Date) /// Thế nào là dung hiện có
                          join caHoc in _db.Cahocs on caDay.IdloaiCaDay equals caHoc.IdCaHoc
                          select new { caHoc.GiaTien }).SumAsync(x => x.GiaTien);
        }

        #endregion
    }
}