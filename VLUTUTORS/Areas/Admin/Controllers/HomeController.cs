using Microsoft.AspNetCore.Authorization;
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
        /// Thông kê tổng tiền thu được.
        /// </summary>
        /// <returns>double</returns>
        private async Task<double> GetReportMoney()
        {
            int chietKhau = (int)_db.Phidays.FirstOrDefault(x => x.Id == 1).ChietKhau;
            return await (from caDay in _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay.Date <= DateTime.Now.Date) /// Thế nào là dung hiện có
                          join caHoc in _db.Cahocs on caDay.IdloaiCaDay equals caHoc.IdCaHoc
                          select new { caHoc.GiaTien }).SumAsync(x => x.GiaTien) / chietKhau;
        }

        /// <summary>
        /// Thống kê tất cả số ca học.
        /// </summary>
        /// <returns>long</returns>
        private async Task<long> GetReportAllLesson()
        {
            return await _db.Cadays.Where(x => x.TrangThai != null && x.NgayDay <= DateTime.Now.Date).LongCountAsync();
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

        /// <summary>
        /// Thông kê số lượng người dùng.
        /// </summary>
        /// <returns>long</returns>
        private async Task<long> GetReportUser()
        {
            return await _db.Taikhoannguoidungs.LongCountAsync();
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
    }
}
