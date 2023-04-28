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
        public IActionResult Index()
        {
            int checkUser = (int)_db.Taikhoannguoidungs.Where(m => m.Id == HttpContext.Session.GetInt32("LoginId")).First().IdxetDuyet;
            if (checkUser == 5)
            {
                return View();
            }
            return RedirectToAction("Login", "Accounts", new { area = "default" });
        }

        #region Method 

        /// <summary>
        /// Xem thống kê giờ dạy của từng gia sư.
        /// </summary>
        /// <returns>long</returns>
        public async Task<long> GetReportTeachingHours()
        {
            int? userId = await IsExistUser();
            if (userId is null)
            {
                return 0;
            }

            return await _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay.Date <= DateTime.Now.Date && x.IdnguoiDay == userId)
                .SumAsync(x => ((x.GioKetThuc - x.GioBatDau) * 60) + (x.PhutKetThuc - x.PhutBatDau)) / 60;
        }

        public JsonResult GetLessonTutor()
        {
            int userId = (int)HttpContext.Session.GetInt32("LoginId");

            int success = _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay <= DateTime.Now.Date && x.IdnguoiDay == userId).Count();
            int cancel = _db.Cadays.Where(x => x.TrangThai == false && x.NgayDay <= DateTime.Now.Date && x.IdnguoiDay == userId).Count();
            int regist = _db.Cadays.Where(x => x.TrangThai == null && x.NgayDay <= DateTime.Now.Date && x.IdnguoiDay == userId).Count();

            List<Chart> list = new List<Chart>();

            list.Add(new Chart { CategoryName = "Số ca chưa được đặt", PostCount = regist });
            list.Add(new Chart { CategoryName = "Số ca hoàn thành", PostCount = success });
            list.Add(new Chart { CategoryName = "Số ca hủy", PostCount = cancel });

            return Json(new { JSONList = list });

        }

        /// <summary>
        /// Xem thống kê thu nhập từng gia sư.
        /// </summary>
        /// <param name="request">GetReportIncomeByDurationRequest</param>
        /// <returns>double</returns>
        public async Task<double> GetReportIncomeByDuration(GetReportIncomeByDurationRequest request)
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

            if (request.FromDate is not null)
            {
                caDays = caDays.Where(x => x.NgayDay <= request.FromDate.Value.Date);
            }
            if (request.ToDate is not null)
            {
                caDays = caDays.Where(x => x.NgayDay >= request.ToDate.Value.Date);
            }

            return await caDays.SumAsync(x => x.GiaTien);
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

        }
    }

