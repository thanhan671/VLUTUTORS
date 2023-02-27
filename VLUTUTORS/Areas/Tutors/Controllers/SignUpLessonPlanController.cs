using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using VLUTUTORS.Areas.Tutors.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

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

            List<Caday> cadays = _db.Cadays.Where(ca => ca.IdnguoiDay.Equals(taikhoannguoidung.Id)).ToList();
            foreach(var cadayItem in cadays)
            {
                cadayItem.tenMonDay = _db.Mongiasus.Find(cadayItem.IdmonDay).TenMonGiaSu.ToString();
                cadayItem.tenLoaiCaDay = _db.Cahocs.Find(cadayItem.IdloaiCaDay).LoaiCa.ToString();
            }

            List<Mongiasu> subjects = new List<Mongiasu>();
            subjects.Add(_db.Mongiasus.FirstOrDefault(i => i.IdmonGiaSu.Equals(taikhoannguoidung.IdmonGiaSu1)));
            if (taikhoannguoidung.IdmonGiaSu2 != null)
            {
                subjects.Add(_db.Mongiasus.FirstOrDefault(i => i.IdmonGiaSu.Equals(taikhoannguoidung.IdmonGiaSu2)));
            }

            Caday caday = new Caday();
            caday.IdnguoiDay = userInfo.Id;
            caday.subjectItems = new SelectList(subjects, "IdmonGiaSu", "TenMonGiaSu", caday.IdmonDay);
            caday.teachTimeItems = new SelectList(_db.Cahocs, "IdCaHoc", "LoaiCa", caday.IdloaiCaDay);

            Tuple<Caday, IEnumerable<Caday>> turple = new Tuple<Caday, IEnumerable<Caday>>(caday, cadays.AsEnumerable());
            return View(turple);
        }

        [HttpGet]
        public IActionResult AddLessonPlan()
        {
            var userInfo = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
            Taikhoannguoidung taikhoannguoidung = _db.Taikhoannguoidungs.Find(userInfo.Id);

            List<Mongiasu> subjects = new List<Mongiasu>();
            subjects.Add(_db.Mongiasus.FirstOrDefault(i => i.IdmonGiaSu.Equals(taikhoannguoidung.IdmonGiaSu1)));
            if(taikhoannguoidung.IdmonGiaSu2 != null)
            {
                subjects.Add(_db.Mongiasus.FirstOrDefault(i => i.IdmonGiaSu.Equals(taikhoannguoidung.IdmonGiaSu2)));
            }

            Caday caday = new Caday();
            caday.IdnguoiDay = userInfo.Id;
            caday.subjectItems = new SelectList(subjects, "IdmonGiaSu", "TenMonGiaSu", caday.IdmonDay);
            caday.teachTimeItems = new SelectList(_db.Cahocs, "IdCaHoc", "LoaiCa", caday.IdloaiCaDay);

            Tuple<Caday,List<Caday>> turple = new Tuple<Caday, List<Caday>>(caday, new List<Caday>());
            return View(turple);
        }

        [HttpPost]
        public async Task<IActionResult> AddLessonPlan(IFormCollection timeSlot, [Bind(Prefix = "Item1")] Caday caday)
        {
            List<DateTime> timeSlots = new List<DateTime>();
            DateTime date = DateTime.Parse(timeSlot["inputDate"]);
            caday.NgayDay = date;

            int teachTime = _db.Cahocs.Find(caday.IdloaiCaDay).LoaiCa;

            foreach (string time in timeSlot["inputHour"])
            {
                DateTime hour = DateTime.Parse(time);
                caday.GioBatDau = hour.Hour;
                caday.PhutBatDau = hour.Minute;
                GetEndTime(caday, teachTime);
                if (ModelState.IsValid)
                {
                    _db.Add(caday);
                    await _db.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index", "SignUpLessonPlan");
        }

        private void GetEndTime(Caday caDay, int teachTime)
        {
            //int teachTime = lessonPlan.IdCaDayNavigation.ThoiGianDay;
            teachTime = 45;
            int startHour = caDay.GioBatDau;
            int startMinute = caDay.PhutBatDau;
            var temp = startMinute + teachTime;
            if (temp >= 60)
            {
                caDay.PhutKetThuc = temp - 60;
                caDay.GioKetThuc = startHour + 1;
                if (caDay.GioKetThuc >= 24)
                {
                    caDay.GioKetThuc = 0;
                }
            }
            else
            {
                caDay.GioKetThuc = startHour;
                caDay.PhutKetThuc = temp;
            }
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
