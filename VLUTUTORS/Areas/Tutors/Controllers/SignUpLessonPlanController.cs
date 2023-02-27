using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using VLUTUTORS.Areas.Tutors.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace VLUTUTORS.Areas.Tutors.Controllers
{
    [Area("Tutors")]
    public class SignUpLessonPlanController : Controller
    {
        private CP25Team01Context _db = new CP25Team01Context();
        private LessonPlan _lessonPlan = new LessonPlan();

        public IActionResult Index()
        {
            var userInfo = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
            Taikhoannguoidung taikhoannguoidung = _db.Taikhoannguoidungs.Find(userInfo.Id);

            LessonPlan lessonPlan = new LessonPlan();
            lessonPlan.ID = 1;
            lessonPlan.IDNguoiDay = userInfo.Id;
            lessonPlan.IDMonDay = (int) taikhoannguoidung.IdmonGiaSu1;
            lessonPlan.NgayDay = DateTime.Today;
            lessonPlan.GioBatDau = 20;
            lessonPlan.PhutBatDau = 0;
            //lessonPlan.GioKetThuc = lessonPlan.Gio
            //lessonPlan.IdCaDayNavigation.ThoiGianDay = 45;
            GetEndTime(lessonPlan);

            return View(lessonPlan);
        }

        private void GetEndTime(LessonPlan lessonPlan)
        {
            //int teachTime = lessonPlan.IdCaDayNavigation.ThoiGianDay;
            int teachTime = 45;
            int startHour = lessonPlan.GioBatDau;
            int startMinute = lessonPlan.PhutBatDau;
            var temp = startMinute + teachTime;
            if(temp >= 60)
            {
                lessonPlan.PhutKetThuc = temp - 60;
                lessonPlan.GioKetThuc = startHour + 1;
                if(lessonPlan.GioKetThuc >= 24)
                {
                    lessonPlan.GioKetThuc = 0;
                }
            }
            else
            {
                lessonPlan.GioKetThuc = startHour;
                lessonPlan.PhutKetThuc = temp;
            }
        }

        [HttpGet]
        public IActionResult AddLessonPlan()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddLessonPlan(IFormCollection timeSlot)
        {
            List<DateTime> timeSlots = new List<DateTime>();

            foreach (string time in timeSlot["inputDate"])
            {
                DateTime date = DateTime.Parse(time);
                timeSlots.Add(date);
            }

            return Index();
        }

        public IActionResult EditLessonPlan()
        {
            return View();
        }

        public IActionResult DeleteLessonPlan()
        {
            return View();
        }
    }
}
