using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Areas.Tutors.Requests.Reports;
using VLUTUTORS.Models;

namespace VLUTUTORS.Areas.Tutors.Controllers
{
    [Area("Tutors")]
    public class HomeController : Controller
    {
        private readonly CP25Team01Context _db = new();
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("LoginId") != null)
            {
                int checkUser = (int)_db.Taikhoannguoidungs.Where(m => m.Id == HttpContext.Session.GetInt32("LoginId")).First().IdxetDuyet;
                var checkBlock = _db.Taikhoannguoidungs.Find(HttpContext.Session.GetInt32("LoginId")).TrangThaiGiaSu;
                if(checkBlock == true)
                {
                    if (checkUser == 5)
                    {
                        int? userId = await IsExistUser();
                        TempData["TeachingHours"] = (double)_db.Cadays.Where(x => x.TrangThai == true && x.NgayDay <= DateTime.Now.Date && x.IdnguoiDay == userId).Sum(x => ((x.GioKetThuc - x.GioBatDau) * 60) + (x.PhutKetThuc - x.PhutBatDau)) / 60;
                        var reportIncome = GetReportIncome();
                        TempData["ReportIncome"] = reportIncome.Result.ToString("#,##0.###");
                        TempData["AllLesson"] = _db.Cadays.Where(m => m.IdnguoiDay == HttpContext.Session.GetInt32("LoginId")).Count();

                        TempData["success"]= _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay < DateTime.Now.Date && x.IdnguoiDay == userId).Count();
                        TempData["cancel"]= _db.Cadays.Where(x => x.TrangThai == false && x.NgayDay < DateTime.Now.Date && x.IdnguoiDay == userId).Count();
                        TempData["notBooking"] = _db.Cadays.Where(x => x.TrangThai == null && x.NgayDay < DateTime.Now.Date && x.IdnguoiHoc == null && x.IdnguoiDay == userId).Count();
                        TempData["registBooking"] = _db.Cadays.Where(x => x.TrangThai == null && x.NgayDay >= DateTime.Now.Date && x.IdnguoiDay == userId).Count();
                        TempData["booking"] = _db.Cadays.Where(x => x.TrangThai == false && x.Link != null && x.NgayDay >= DateTime.Now.Date && x.IdnguoiDay == userId).Count();

                        return View();
                    }
                }
                else
                {
                    TempData["Message"] = "Tài khoản đã bị khóa, vui lòng liên hệ quản trị viên!";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index", "Home", new { area = "default" });
                }
            }

            return RedirectToAction("Login", "Accounts", new { area = "default" });
        }

        #region Method 

        /// <summary>
        /// Xem thống kê thu nhập từng gia sư.
        /// </summary>
        /// <returns>float</returns>

        public async Task<float> GetReportIncome()
        {
            int? userId = await IsExistUser();
            if (userId is null)
            {
                return 0;
            }

            var caDays = from caDay in _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay.Date <= DateTime.Now.Date)
                         join caHoc in _db.Cahocs
                         on caDay.IdloaiCaDay equals caHoc.IdCaHoc
                         where caDay.IdnguoiDay == userId
                         select new
                         {
                             NgayDay = caDay.NgayDay,
                             GiaTien = caHoc.GiaTien
                         };
            float chietKhau = (float)_db.Phidays.FirstOrDefault(x => x.Id == 1).ChietKhau / 100;
            float tienThue = (float)await caDays.SumAsync(x => x.GiaTien) * chietKhau;

            float tienNhan = (float)await caDays.SumAsync(x => x.GiaTien) - tienThue;

            return tienNhan;
        }

        private async Task<int?> IsExistUser()
        {
            int userId = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo")).Id;

            int DA_XET_DUYET = 5;

            if (userId is 0)
            {
                return null;
            }

            Taikhoannguoidung result = await _db.Taikhoannguoidungs.FirstOrDefaultAsync(x => x.Id == userId && x.IdxetDuyet == DA_XET_DUYET);

            return result?.Id;
        }

        #endregion

        public IActionResult DetailStaticLesson()
        {
            if (HttpContext.Session.GetString("LoginId") == null)
            {
                return RedirectToAction("Index", "Login", new { area = "default" });
            }
            int id = (int)HttpContext.Session.GetInt32("LoginId");
            List<Caday> caDays = _db.Cadays.Where(ca => ca.IdnguoiDay == id).ToList();

            foreach (var cadayItem in caDays)
            {
                if(cadayItem.IdnguoiHoc != null)
                {
                    cadayItem.tenNguoiHoc = _db.Taikhoannguoidungs.Find(cadayItem.IdnguoiHoc).HoTen.ToString();
                }
                else
                {
                    cadayItem.tenNguoiHoc = "Không có";
                }
                cadayItem.tenNguoiDay = _db.Taikhoannguoidungs.Find(cadayItem.IdnguoiDay).HoTen.ToString();
                cadayItem.tenMonDay = _db.Mongiasus.Find(cadayItem.IdmonDay).TenMonGiaSu.ToString();
                cadayItem.giaCaDay = _db.Cahocs.Find(cadayItem.IdloaiCaDay).GiaTien;
            }
            return View(caDays);
        }
    }
}

