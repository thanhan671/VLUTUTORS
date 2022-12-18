﻿using Microsoft.AspNetCore.Mvc;
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
            taikhoannguoidung.Id = userInfo.Id;
            taikhoannguoidung.HoTen = userInfo.HoTen;
            taikhoannguoidung.Email = userInfo.Email;
            taikhoannguoidung.MatKhau = userInfo.MatKhau;

            taikhoannguoidung.DepartmentItems = new SelectList(_db.Khoas, "Idkhoa", "TenKhoa", taikhoannguoidung.Idkhoa);
            taikhoannguoidung.GenderItems = new SelectList(_db.Gioitinhs, "IdgioiTinh", "GioiTinh1", taikhoannguoidung.IdgioiTinh);
            taikhoannguoidung.BankItems = new SelectList(_db.Nganhangs, "Id", "TenNganHangHoacViDienTu", taikhoannguoidung.IdnganHang); 
            taikhoannguoidung.Subject1Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", taikhoannguoidung.IdmonGiaSu1);
            taikhoannguoidung.Subject2Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", taikhoannguoidung.IdmonGiaSu2);

            return View(taikhoannguoidung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterAsTutor([Bind(include: "Id, HoTen, Email, MatKhau, IdgioiTinh, Sdt, NgaySinh, Idkhoa, AnhDaiDien, TrangThaiTaiKhoan, SoTaiKhoan, IdNganHang, GioiThieu, DanhGiaVeViecGiaSu, DiemTrungBinh, IdmonGiaSu1, ChungChiMon1, GioiThieuVeMonGiaSu1, IdmonGiaSu2, ChungChiMon2, GioiThieuVeMonGiaSu2")]Taikhoannguoidung taikhoannguoidung, List<IFormFile> certificates1, List<IFormFile> certificates2)
        {
            string path = Path.Combine(this._environment.WebRootPath, "certificates");
            taikhoannguoidung.ChungChiMon1 = TutorServices.SaveUploadImages(path, certificates1);
            taikhoannguoidung.ChungChiMon2 = TutorServices.SaveUploadImages(path, certificates2);
            taikhoannguoidung.IdxetDuyet = (int) ApprovalStatus.TRAINING;

            Console.WriteLine("chung chi: " + JsonConvert.DeserializeObject(taikhoannguoidung.ChungChiMon1));
            if (ModelState.IsValid)
            {
                Console.WriteLine("run this in model valid");

                _db.Entry(taikhoannguoidung).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
            }
            Console.WriteLine("test: " + taikhoannguoidung.HoTen + " : " + taikhoannguoidung.Email + " : " + taikhoannguoidung.Idkhoa.ToString() + " : " + taikhoannguoidung.NgaySinh);
            taikhoannguoidung.DepartmentItems = new SelectList(_db.Khoas, "Idkhoa", "TenKhoa", taikhoannguoidung.Idkhoa);
            taikhoannguoidung.GenderItems = new SelectList(_db.Gioitinhs, "IdgioiTinh", "GioiTinh1", taikhoannguoidung.IdgioiTinh);
            taikhoannguoidung.BankItems = new SelectList(_db.Nganhangs, "Id", "TenNganHangHoacViDienTu", taikhoannguoidung.IdnganHang);
            taikhoannguoidung.Subject1Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", taikhoannguoidung.IdmonGiaSu1);
            taikhoannguoidung.Subject2Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", taikhoannguoidung.IdmonGiaSu2);

            return View(taikhoannguoidung);
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