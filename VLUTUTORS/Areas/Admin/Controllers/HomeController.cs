using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Areas.Admin.Models;
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
            TempData["AllLesson"] = _db.Cadays.Count();
            TempData["AllUser"] = _db.Taikhoannguoidungs.Count();
            TempData["TeachingHours"] = (double)_db.Cadays.Where(x => x.TrangThai == true && x.NgayDay <= DateTime.Now.Date).Sum(x => ((x.GioKetThuc - x.GioBatDau) * 60) + (x.PhutKetThuc - x.PhutBatDau))/60;
            var reportMoney = GetReportMoney();
            TempData["ReportMoney"] = reportMoney.Result.ToString("#,##0.###");
            TempData["TutorEvaluation"] = _db.Danhgiagiasus.Count();
            TempData["LearnerEvaluation"] = _db.Danhgianguoihocs.Count();
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
            int success = _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay < DateTime.Now.Date).Count();
            int cancel = _db.Cadays.Where(x => x.TrangThai == false && x.NgayDay < DateTime.Now.Date).Count();
            int notBooking = _db.Cadays.Where(x => x.TrangThai == null && x.NgayDay < DateTime.Now.Date && x.IdnguoiHoc == null).Count();
            int registBooking = _db.Cadays.Where(x => x.TrangThai == null && x.NgayDay >= DateTime.Now.Date).Count();
            int booking = _db.Cadays.Where(x => x.TrangThai == false && x.Link != null && x.NgayDay >= DateTime.Now.Date).Count();

            List<Chart> list = new List<Chart>();

            list.Add(new Chart { CategoryName = "Số ca hoàn thành", PostCount = success });
            list.Add(new Chart { CategoryName = "Số ca hủy", PostCount = cancel });
            list.Add(new Chart { CategoryName = "Số ca không được đặt", PostCount = notBooking });
            list.Add(new Chart { CategoryName = "Số ca đang được đặt", PostCount = booking });
            list.Add(new Chart { CategoryName = "Số ca chưa được đặt", PostCount = registBooking });

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

        public JsonResult GetLearnerEvaluation()
        {
            List<Chart> list = new List<Chart>();

            list.Add(new Chart { CategoryName = "Đánh giá 1 sao", PostCount = _db.Danhgianguoihocs.Where(m => m.Diem == 1).Count() });
            list.Add(new Chart { CategoryName = "Đánh giá 2 sao", PostCount = _db.Danhgianguoihocs.Where(m => m.Diem == 2).Count() });
            list.Add(new Chart { CategoryName = "Đánh giá 3 sao", PostCount = _db.Danhgianguoihocs.Where(m => m.Diem == 3).Count() });
            list.Add(new Chart { CategoryName = "Đánh giá 4 sao", PostCount = _db.Danhgianguoihocs.Where(m => m.Diem == 4).Count() });
            list.Add(new Chart { CategoryName = "Đánh giá 5 sao", PostCount = _db.Danhgianguoihocs.Where(m => m.Diem == 5).Count() });

            return Json(new { JSONList = list });

        }

        public JsonResult GetTutorEvaluation()
        {
            List<Chart> list = new List<Chart>();

            list.Add(new Chart { CategoryName = "Đánh giá 1 sao", PostCount = _db.Danhgiagiasus.Where(m => m.Diem == 1).Count() });
            list.Add(new Chart { CategoryName = "Đánh giá 2 sao", PostCount = _db.Danhgiagiasus.Where(m => m.Diem == 2).Count() });
            list.Add(new Chart { CategoryName = "Đánh giá 3 sao", PostCount = _db.Danhgiagiasus.Where(m => m.Diem == 3).Count() });
            list.Add(new Chart { CategoryName = "Đánh giá 4 sao", PostCount = _db.Danhgiagiasus.Where(m => m.Diem == 4).Count() });
            list.Add(new Chart { CategoryName = "Đánh giá 5 sao", PostCount = _db.Danhgiagiasus.Where(m => m.Diem == 5).Count() });

            return Json(new { JSONList = list });

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
            var giaSu = _db.Cadays.Select(m => m.IdnguoiDay).ToList();
            var result = new List<LessonList>();
            var tmp = giaSu;
            giaSu = giaSu.Distinct().ToList();
            float chietKhau = (float)_db.Phidays.FirstOrDefault(x => x.Id == 1).ChietKhau / 100;
            float tienNhan = 0;
            DateTime day = DateTime.Today.AddDays(-1);
            TempData["Day"] = day.ToString("dd/MM/yyyy");

            foreach (var item in giaSu)
            {
                double soGio = (double)_db.Cadays.Where(x => x.IdnguoiDay == item && x.TrangThai == true && x.NgayDay <= DateTime.Now.Date).Sum(x => ((x.GioKetThuc - x.GioBatDau) * 60) + (x.PhutKetThuc - x.PhutBatDau)) / 60;
                var tenGiaSu = _db.Taikhoannguoidungs.Find(item).HoTen.ToString();
                var caHocs = _db.Cadays.Where(m => m.IdnguoiDay == item).ToList();
                foreach (var cahoc in caHocs)
                {
                    if(cahoc.TrangThai == true)
                    {
                        tienNhan += ((float)_db.Cahocs.Find(cahoc.IdloaiCaDay).GiaTien);
                    }
                }
                result.Add(new LessonList()
                {
                    idGiaSu = item,
                    tenGiaSu = tenGiaSu,
                    soCa = caHocs.Count(),
                    soGio = soGio,
                    tongTien = tienNhan - (tienNhan*chietKhau)
                });
                tienNhan = 0;
            }
            ViewBag.GiaSu = result.OrderBy(m=>m.tenGiaSu);
            return View();
        }

        public IActionResult DetailStaticLessonOfTutor(int id)
        {
            if (HttpContext.Session.GetString("LoginADId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            List<Caday> caDays = _db.Cadays.Where(ca => ca.IdnguoiDay == id).ToList();

            foreach (var cadayItem in caDays)
            {
                cadayItem.tenNguoiDay = _db.Taikhoannguoidungs.Find(cadayItem.IdnguoiDay).HoTen.ToString();
                if (cadayItem.IdnguoiHoc != null)
                {
                    cadayItem.tenNguoiHoc = _db.Taikhoannguoidungs.Find(cadayItem.IdnguoiHoc).HoTen.ToString();
                }
                else
                {
                    cadayItem.tenNguoiHoc = "Chưa được đặt";
                }
                cadayItem.tenMonDay = _db.Mongiasus.Find(cadayItem.IdmonDay).TenMonGiaSu.ToString();
                cadayItem.giaCaDay = _db.Cahocs.Find(cadayItem.IdloaiCaDay).GiaTien;
            }
            return View(caDays);
        }
    }
}
