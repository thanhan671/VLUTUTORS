﻿using Microsoft.AspNetCore.Hosting;
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

namespace VLUTUTORS.Areas.Tutors.Controllers
{
    [Area("Tutors")]
    public class AccountController : Controller
    {
        private readonly CP25Team01Context _db = new CP25Team01Context();
        private readonly ILogger<AccountController> _logger;
        private IHostingEnvironment _environment;

        public AccountController(ILogger<AccountController> logger, IHostingEnvironment environment)
        {
            _logger = logger;
            this._environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            int idGiaSu = (int)HttpContext.Session.GetInt32("IdGiaSu");
            string user = HttpContext.Session.GetString("LoginId");
            var userInfo = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
            Taikhoannguoidung taikhoannguoidung = _db.Taikhoannguoidungs.Find(userInfo.Id);


            taikhoannguoidung.DepartmentItems = new SelectList(_db.Khoas, "Idkhoa", "TenKhoa", taikhoannguoidung.Idkhoa);
            taikhoannguoidung.GenderItems = new SelectList(_db.Gioitinhs, "IdgioiTinh", "GioiTinh1", taikhoannguoidung.IdgioiTinh);
            taikhoannguoidung.BankItems = new SelectList(_db.Nganhangs, "Id", "TenNganHangHoacViDienTu", taikhoannguoidung.IdnganHang);
            taikhoannguoidung.Subject1Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", taikhoannguoidung.IdmonGiaSu1);
            taikhoannguoidung.Subject2Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", taikhoannguoidung.IdmonGiaSu2);
            Console.WriteLine("file path: " + taikhoannguoidung.AnhDaiDien);

            if (taikhoannguoidung == null)
                return View();

            return View(taikhoannguoidung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind(include: "Id, HoTen, Email, MatKhau, IdgioiTinh, Sdt, NgaySinh, Idkhoa, AnhDaiDien, TrangThaiTaiKhoan, SoTaiKhoan, IdnganHang, GioiThieu, DanhGiaVeViecGiaSu, DiemTrungBinh, IdmonGiaSu1, TenChungChiHoacDiemMon1, ChungChiMon1, GioiThieuVeMonGiaSu1, IdmonGiaSu2, TenChungChiHoacDiemMon2, ChungChiMon2, GioiThieuVeMonGiaSu2, DiemBaiTest, TrangThaiGiaSu")] Taikhoannguoidung taikhoannguoidung, List<IFormFile> avatar, List<IFormFile> certificates1, List<IFormFile> certificates2)
        {
            string certificates1Path = Path.Combine("certificates", taikhoannguoidung.Id.ToString(), "cer1");
            string certificates2Path = Path.Combine("certificates", taikhoannguoidung.Id.ToString(), "cer2");
            string avatarPath = Path.Combine("avatars", taikhoannguoidung.Id.ToString());

            if (ModelState.IsValid)
            {
                taikhoannguoidung.TrangThaiTaiKhoan = true;
                taikhoannguoidung.ChungChiMon1 = certificates1.Count != 0 ? TutorServices.SaveUploadFiles(this._environment.WebRootPath, certificates1Path, certificates1) : taikhoannguoidung.ChungChiMon1;
                taikhoannguoidung.ChungChiMon2 = certificates2.Count != 0 ? TutorServices.SaveUploadFiles(this._environment.WebRootPath, certificates2Path, certificates2) : taikhoannguoidung.ChungChiMon2;
                taikhoannguoidung.AnhDaiDien = avatar.Count != 0 ? TutorServices.SaveAvatar(this._environment.WebRootPath, avatarPath, avatar) : taikhoannguoidung.AnhDaiDien;
                taikhoannguoidung.IdxetDuyet = (int)ApprovalStatus.APPROVED;

                Console.WriteLine("anh dai dien: " + taikhoannguoidung.AnhDaiDien);
                //_db.Entry(taikhoannguoidung).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.Update(taikhoannguoidung);
                _db.SaveChanges();
                TempData["Message"] = "Cập nhật thành công!";
                TempData["MessageType"] = "success";
                return RedirectToAction("Index", "Account");
            }

            taikhoannguoidung.AnhDaiDien = taikhoannguoidung.AnhDaiDien;
            taikhoannguoidung.DepartmentItems = new SelectList(_db.Khoas, "Idkhoa", "TenKhoa", taikhoannguoidung.Idkhoa);
            taikhoannguoidung.GenderItems = new SelectList(_db.Gioitinhs, "IdgioiTinh", "GioiTinh1", taikhoannguoidung.IdgioiTinh);
            taikhoannguoidung.BankItems = new SelectList(_db.Nganhangs, "Id", "TenNganHangHoacViDienTu", taikhoannguoidung.IdnganHang);
            taikhoannguoidung.Subject1Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", taikhoannguoidung.IdmonGiaSu1);
            taikhoannguoidung.Subject2Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", taikhoannguoidung.IdmonGiaSu2);
            return View(taikhoannguoidung);
        }

        public FileResult DownloadFile(string fileName, int id)
        {
            var userInfo = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
            string certificatesPath = id == 1 ? Path.Combine("certificates", userInfo.Id.ToString(), "cer1") : Path.Combine("certificates", userInfo.Id.ToString(), "cer2");

            string path = Path.Combine(this._environment.WebRootPath, certificatesPath, fileName);

            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", fileName);
        }
    }
}