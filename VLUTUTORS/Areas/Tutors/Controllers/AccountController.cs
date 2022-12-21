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
using VLUTUTORS.Support.Services;

namespace VLUTUTORS.Areas.Tutors.Controllers
{
    [Area("Tutors")]
    public class AccountController : Controller
    {
        private readonly CP25Team01Context _db = DataManager.Instance().db();
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
            var userInfo = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
            Taikhoannguoidung taikhoannguoidung = _db.Taikhoannguoidungs.Find(userInfo.Id);

            taikhoannguoidung.DepartmentItems = new SelectList(_db.Khoas, "Idkhoa", "TenKhoa", taikhoannguoidung.Idkhoa);
            taikhoannguoidung.GenderItems = new SelectList(_db.Gioitinhs, "IdgioiTinh", "GioiTinh1", taikhoannguoidung.IdgioiTinh);
            taikhoannguoidung.BankItems = new SelectList(_db.Nganhangs, "Id", "TenNganHangHoacViDienTu", taikhoannguoidung.IdnganHang);
            taikhoannguoidung.Subject1Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", taikhoannguoidung.IdmonGiaSu1);
            taikhoannguoidung.Subject2Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", taikhoannguoidung.IdmonGiaSu2);

            //string certificates1Path = Path.Combine(this._environment.WebRootPath, "certificates", taikhoannguoidung.Id.ToString(), "cer1");
            //string certificates2Path = Path.Combine(this._environment.WebRootPath, "certificates", taikhoannguoidung.Id.ToString(), "cer2");
            //string avatarPath = Path.Combine(this._environment.WebRootPath, "avatars", taikhoannguoidung.Id.ToString());

            //List<string> certificate1Files = JsonConvert.DeserializeObject<List<string>>(taikhoannguoidung.ChungChiMon1);
            //List<string> certificate2Files = JsonConvert.DeserializeObject<List<string>>(taikhoannguoidung.ChungChiMon2);
            //List<string> avatarFile = JsonConvert.DeserializeObject<List<string>>(taikhoannguoidung.AnhDaiDien);
            

            if (taikhoannguoidung == null)
                return View();

            return View(taikhoannguoidung);
        }

        [HttpPost]
        public IActionResult Index([Bind(include: "Id, HoTen, Email, MatKhau, IdgioiTinh, Sdt, NgaySinh, Idkhoa, AnhDaiDien, TrangThaiTaiKhoan, SoTaiKhoan, IdnganHang, GioiThieu, DanhGiaVeViecGiaSu, DiemTrungBinh, IdmonGiaSu1, TenChungChiHoacDiemMon1, ChungChiMon1, GioiThieuVeMonGiaSu1, IdmonGiaSu2, TenChungChiHoacDiemMon2, ChungChiMon2, GioiThieuVeMonGiaSu2")] Taikhoannguoidung taikhoannguoidung, List<IFormFile> avatar, List<IFormFile> certificates1, List<IFormFile> certificates2)
        {
            string certificates1Path = Path.Combine("certificates", taikhoannguoidung.Id.ToString(), "cer1");
            string certificates2Path = Path.Combine("certificates", taikhoannguoidung.Id.ToString(), "cer2");
            string avatarPath = Path.Combine("avatars", taikhoannguoidung.Id.ToString());

            taikhoannguoidung.ChungChiMon1 = TutorServices.SaveUploadImages(this._environment.WebRootPath, certificates1Path, certificates1);
            taikhoannguoidung.ChungChiMon2 = TutorServices.SaveUploadImages(this._environment.WebRootPath, certificates2Path, certificates2);
            taikhoannguoidung.AnhDaiDien = TutorServices.SaveUploadImages(this._environment.WebRootPath, avatarPath, avatar);

            Console.WriteLine("chung chi: " + JsonConvert.DeserializeObject(taikhoannguoidung.ChungChiMon1));
            if (ModelState.IsValid)
            {
                _db.Entry(taikhoannguoidung).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
            }

            taikhoannguoidung.DepartmentItems = new SelectList(_db.Khoas, "Idkhoa", "TenKhoa", taikhoannguoidung.Idkhoa);
            taikhoannguoidung.GenderItems = new SelectList(_db.Gioitinhs, "IdgioiTinh", "GioiTinh1", taikhoannguoidung.IdgioiTinh);
            taikhoannguoidung.BankItems = new SelectList(_db.Nganhangs, "Id", "TenNganHangHoacViDienTu", taikhoannguoidung.IdnganHang);
            taikhoannguoidung.Subject1Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", taikhoannguoidung.IdmonGiaSu1);
            taikhoannguoidung.Subject2Items = new SelectList(_db.Mongiasus, "IdmonGiaSu", "TenMonGiaSu", taikhoannguoidung.IdmonGiaSu2);

            return View(taikhoannguoidung);
        }
    }
}
