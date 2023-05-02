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
using VLUTUTORS.Support.Services;

namespace VLUTUTORS.Areas.Tutors.Controllers
{
    [Area("Tutors")]
    public class ManageTeachScheduleController : Controller
    {
        private CP25Team01Context _db = new CP25Team01Context();
        private LessonPlan _lessonPlan = new LessonPlan();

        public IActionResult Index()
        {
            string user = HttpContext.Session.GetString("LoginId");
            if (user == null)
            {
                return RedirectToAction("Login", "Accounts", new { area = "default" });
            }
            int checkUser = (int)_db.Taikhoannguoidungs.Where(m => m.Id == HttpContext.Session.GetInt32("LoginId")).First().IdxetDuyet;
            if (checkUser == 5)
            {
                var userInfo = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
                Taikhoannguoidung taikhoannguoidung = _db.Taikhoannguoidungs.Find(userInfo.Id);

                //List<Taikhoannguoidung> teee = _db.Taikhoannguoidungs.ToList();
                List<Caday> cadays = _db.Cadays.Where(ca => ca.IdnguoiDay.Equals(taikhoannguoidung.Id)).ToList();
                foreach (var cadayItem in cadays)
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
            return RedirectToAction("Login", "Accounts", new { area = "default" });
        }

        [HttpGet]
        public IActionResult AddLessonPlan()
        {
            var userInfo = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
            Taikhoannguoidung taikhoannguoidung = _db.Taikhoannguoidungs.Find(userInfo.Id);

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

            Tuple<Caday, List<Caday>> turple = new Tuple<Caday, List<Caday>>(caday, new List<Caday>());
            return View(turple);
        }

        [HttpPost]
        public async Task<IActionResult> AddLessonPlan(IFormCollection timeSlot, [Bind(Prefix = "Item1")] Caday caday, [FromForm] string inputHour, [FromForm] string inputDate)
        {
            if (inputDate != null && inputHour != null)
            {
                List<DateTime> timeSlots = new List<DateTime>();
                DateTime date = DateTime.Parse(timeSlot["inputDate"]);
                caday.NgayDay = date;

                int teachTime = _db.Cahocs.Find(caday.IdloaiCaDay).LoaiCa;

                List<Caday> lessonPlans = new List<Caday>();
                foreach (string time in timeSlot["inputHour"])
                {
                    Caday lessonPlan = new Caday()
                    {
                        IdnguoiDay = caday.IdnguoiDay,
                        IdmonDay = caday.IdmonDay,
                        IdloaiCaDay = caday.IdloaiCaDay,
                        NgayDay = caday.NgayDay,
                        LapLich = caday.LapLich
                    };
                    DateTime hour = new DateTime();

                    if (time == "")
                    {
                        TempData["Message"] = "Vui lòng điền đủ thông tin cho ca dạy!";
                        TempData["MessageType"] = "error";
                        return RedirectToAction("Index", "ManageTeachSchedule");
                    }

                    hour = DateTime.Parse(time);
                    lessonPlan.GioBatDau = hour.Hour;
                    lessonPlan.PhutBatDau = hour.Minute;

                    GetEndTime(lessonPlan, teachTime);

                    bool isOverLapse = CheckLessonHasRegister(lessonPlan.IdnguoiDay, lessonPlan.NgayDay, lessonPlan.GioBatDau, lessonPlan.PhutBatDau, lessonPlan.GioKetThuc, lessonPlan.PhutKetThuc);
                    if (isOverLapse)
                    {
                        TempData["Message"] = "Thời gian bị trùng với ca dạy khác";
                        TempData["MessageType"] = "error";
                        return RedirectToAction("Index", "ManageTeachSchedule");
                    }

                    lessonPlans.Add(lessonPlan);
                }


                if (ModelState.IsValid)
                {
                    await _db.AddRangeAsync(lessonPlans);
                    await _db.SaveChangesAsync();
                    TempData["Message"] = "Đăng ký ca dạy thành công!";
                    TempData["MessageType"] = "success";
                }
            }
            else
            {
                TempData["Message"] = "Vui lòng điền đủ thông tin cho ca dạy!";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction("Index", "ManageTeachSchedule");
        }
        [HttpPost]
        public IActionResult AcceptBooking(int id)
        {
            Caday caday = _db.Cadays.FirstOrDefault(m => m.Id == id);
            caday.TrangThai = true;

            _db.Update(caday);
            _db.SaveChanges();

            TempData["link"] = caday.Link;

            return RedirectToAction("Index", "ManageTeachSchedule");
        }
        private void GetEndTime(Caday caDay, int teachTime)
        {
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

        private List<Mongiasu> GetListSubject()
        {
            var userInfo = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
            Taikhoannguoidung taikhoannguoidung = _db.Taikhoannguoidungs.Find(userInfo.Id);

            List<Mongiasu> subjects = new List<Mongiasu>();
            subjects.Add(_db.Mongiasus.FirstOrDefault(i => i.IdmonGiaSu.Equals(taikhoannguoidung.IdmonGiaSu1)));
            if (taikhoannguoidung.IdmonGiaSu2 != null)
            {
                subjects.Add(_db.Mongiasus.FirstOrDefault(i => i.IdmonGiaSu.Equals(taikhoannguoidung.IdmonGiaSu2)));
            }
            return subjects;
        }

        [HttpGet]
        public IActionResult EditLessonPlan(int lessonPlanId)
        {
            List<Mongiasu> subjects = GetListSubject();

            Caday caDay = _db.Cadays.Find(lessonPlanId);
            caDay.subjectItems = new SelectList(subjects, "IdmonGiaSu", "TenMonGiaSu", caDay.IdmonDay);
            caDay.teachTimeItems = new SelectList(_db.Cahocs, "IdCaHoc", "LoaiCa", caDay.IdloaiCaDay);

            return View(caDay);
        }

        [HttpPost]
        public async Task<IActionResult> EditLessonPlan(IFormCollection timeSlot, [Bind(include: "Id, IdnguoiDay, IdmonDay, IdloaiCaDay")] Caday caday)
        {
            List<DateTime> timeSlots = new List<DateTime>();
            DateTime date = DateTime.Parse(timeSlot["inputDate"]);
            caday.NgayDay = date;
            int teachTime = _db.Cahocs.Find(caday.IdloaiCaDay).LoaiCa;

            DateTime hour = DateTime.Parse(timeSlot["inputHour"]);
            caday.GioBatDau = hour.Hour;
            caday.PhutBatDau = hour.Minute;
            GetEndTime(caday, teachTime);

            Caday lessonPlan = _db.Cadays.Find(caday.Id);
            int oldStartHour = lessonPlan.GioBatDau;
            int oldStartMinute = lessonPlan.PhutBatDau;

            bool isOverLapse = CheckLessonHasRegister(caday.IdnguoiDay, caday.NgayDay, caday.GioBatDau, caday.PhutBatDau, caday.GioKetThuc, caday.PhutKetThuc, oldStartHour, oldStartMinute);
            if (isOverLapse)
            {
                TempData["Message"] = "Thời gian bị trùng với ca dạy khác";
                TempData["MessageType"] = "error";
                return RedirectToAction("EditLessonPlan", "ManageTeachSchedule", new { lessonPlanId = caday.Id});
            }

            if (ModelState.IsValid)
            {
                _db.ChangeTracker.Clear();
                _db.Update(caday);
                await _db.SaveChangesAsync();
                TempData["Message"] = "Cập nhật ca dạy thành công!";
                TempData["MessageType"] = "success";
            }

            return RedirectToAction("Index", "ManageTeachSchedule");
        }

        public async Task<IActionResult> DeleteLessonPlan(int lessonPlanId)
        {
            // call check lesson has register

            Caday caDay = _db.Cadays.Where(p => p.Id == lessonPlanId).FirstOrDefault();
            caDay.Link = null;
            caDay.TrangThai = false;
            _db.Cadays.Update(caDay);
            await _db.SaveChangesAsync();

            Cahoc cahoc = _db.Cahocs.Where(c => c.IdCaHoc == caDay.IdloaiCaDay).FirstOrDefault();
            Phiday phiday = _db.Phidays.Where(ph => ph.Id == 1).FirstOrDefault();

            float commision = (int)cahoc.GiaTien * ((float)phiday.ChietKhau/100);
            int money = (int) (cahoc.GiaTien + commision);

            if(caDay.IdnguoiHoc != null)
            {
                MoneyServices.SubtractMoney(money, caDay.IdnguoiDay, _db);
                MoneyServices.AddMoney((int)cahoc.GiaTien, (int)caDay.IdnguoiHoc, _db);
            }

            TempData["Message"] = "Hủy ca dạy thành công!";
            TempData["MessageType"] = "success";

            return RedirectToAction("Index", "ManageTeachSchedule");
        }

        private bool CheckLessonHasRegister(int tutorId, DateTime regisDate, int startHour, int startMinute, int endHour, int endMinute, int editStartHour = 0, int editStartMinute = 0)
        {
            List<Caday> caDayByTutor = _db.Cadays.Where(c => c.IdnguoiDay == tutorId).ToList();
            List<Caday> caDayByDate = caDayByTutor.Where(c => c.NgayDay.Date == regisDate.Date).ToList();
            if(editStartMinute != 0 && editStartHour != 0)
            {
                caDayByDate.Remove(caDayByDate.Where(c => c.GioBatDau == editStartHour && c.PhutBatDau == editStartMinute).FirstOrDefault());
            }    

            if (caDayByDate.Count == 0)
            {
                return false;
            }

            TimeSpan startTime = new TimeSpan(startHour, startMinute, 0);
            TimeSpan endTime = new TimeSpan(endHour, endMinute, 0);

            bool isOverLapse = false;

            foreach (var caDay in caDayByDate)
            {
                TimeSpan caDayStartTime = new TimeSpan(caDay.GioBatDau, caDay.PhutBatDau, 0);
                TimeSpan caDayEndTime = new TimeSpan(caDay.GioKetThuc, caDay.PhutKetThuc, 0);

                isOverLapse = startTime <= caDayEndTime && caDayStartTime <= endTime;
                if (isOverLapse)
                {
                    break;
                }
            }
 
            return isOverLapse;
        }
    }
}
