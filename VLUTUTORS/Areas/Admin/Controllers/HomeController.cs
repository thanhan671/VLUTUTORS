using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class HomeController : Controller
    {
        private CP25Team01Context _db = new CP25Team01Context();
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("LoginADId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            TempData["AllLesson"] = _db.Cadays.Where(x => x.TrangThai != null && x.NgayDay <= DateTime.Now.Date).Count();
            TempData["AllUser"] = _db.Taikhoannguoidungs.Count();
            TempData["TeachingHours"] = (double)_db.Cadays.Where(x => x.TrangThai == true && x.NgayDay <= DateTime.Now.Date).Sum(x => ((x.GioKetThuc - x.GioBatDau) * 60) + (x.PhutKetThuc - x.PhutBatDau))/60;
            var reportMoney = GetReportMoney();
            TempData["ReportMoney"] = reportMoney.Result.ToString();
            return View();
        }
        #region Method Report

        /// <summary>
        /// Thông kê tổng tiền thu được.
        /// </summary>
        /// <returns>double</returns>
        private async Task<double> GetReportMoney()
        {
            float chietKhau = (int)_db.Phidays.FirstOrDefault(x => x.Id == 1).ChietKhau;
            return await (from caDay in _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay.Date <= DateTime.Now.Date) 
                          join caHoc in _db.Cahocs on caDay.IdloaiCaDay equals caHoc.IdCaHoc
                          select new { caHoc.GiaTien }).SumAsync(x => x.GiaTien) * (chietKhau / 100);
        }

        public JsonResult GetLessonWeb()
        {
            int success = _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay <= DateTime.Now.Date).Count();
            int cancel = _db.Cadays.Where(x => x.TrangThai == false && x.NgayDay <= DateTime.Now.Date).Count();

            List<Chart> list = new List<Chart>();

            list.Add(new Chart { CategoryName = "Số ca hoàn thành", PostCount = success });
            list.Add(new Chart { CategoryName = "Số ca hủy", PostCount = cancel });

            return Json(new { JSONList = list });

        }

        public JsonResult GetUserWeb()
        {
            int learners = _db.Taikhoannguoidungs.Where(x => x.IdxetDuyet != 5).Count();
            int tutors = _db.Taikhoannguoidungs.Where(x => x.IdxetDuyet == 5).Count();

            List<Chart> list = new List<Chart>();

            list.Add(new Chart { CategoryName = "Số người học", PostCount = learners });
            list.Add(new Chart { CategoryName = "Số gia sư", PostCount = tutors });

            return Json(new { JSONList = list});

        }


        #endregion

        public async Task<IActionResult> DetailStaticUser()
        {
            if (HttpContext.Session.GetString("LoginADId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var taiKhoans = await _db.Taikhoannguoidungs.ToListAsync();
            int newAcc = 0;
            int newLearners = 0;
            int newTutors = 0;
            DateTime day = DateTime.Today.AddDays(-7);

            foreach (var user in taiKhoans)
            {
                TimeSpan checkTime = (TimeSpan)(DateTime.Now - user.NgayTao);
                if(checkTime.Days <= 7)
                {
                    newAcc++;
                    if(user.IdxetDuyet == 5)
                    {
                        newTutors++;
                    }
                    else
                    {
                        newLearners++;
                    }
                }
            }

            TempData["NowDay"] = DateTime.Now.ToString("dd/MM/yyyy");
            TempData["Day"] = day.ToString("dd/MM/yyyy");
            TempData["NewAcc"] = newAcc;
            TempData["NewLearner"] = newLearners;
            TempData["NewTutor"] = newTutors;
         
            return View(taiKhoans);
        }

        public IActionResult DetailStaticLesson()
        {
            if (HttpContext.Session.GetString("LoginADId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            List<Caday> caDays = _db.Cadays.Where(ca => ca.TrangThai != null).ToList();

            foreach (var cadayItem in caDays)
            {
                cadayItem.tenNguoiDay = _db.Taikhoannguoidungs.Find(cadayItem.IdnguoiDay).HoTen.ToString();
                cadayItem.tenNguoiHoc = _db.Taikhoannguoidungs.Find(cadayItem.IdnguoiHoc).HoTen.ToString();
                cadayItem.tenMonDay = _db.Mongiasus.Find(cadayItem.IdmonDay).TenMonGiaSu.ToString();
                cadayItem.giaCaDay = _db.Cahocs.Find(cadayItem.IdloaiCaDay).GiaTien;
            }
            return View(caDays);
        }
    }
}
