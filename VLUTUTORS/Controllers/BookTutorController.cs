using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Intrinsics.X86;
using System.Security.Principal;
using System.Threading.Tasks;
using VLUTUTORS.Areas.Tutors.Models;
using VLUTUTORS.Models;
using VLUTUTORS.Responses.BookTutors;
using VLUTUTORS.Support.Services;
using ZoomNet;
using ZoomNet.Models;
using X.PagedList;

namespace VLUTUTORS.Controllers
{
    public class BookTutorController : Controller
    {
        private CP25Team01Context _db = new CP25Team01Context();
        public IActionResult Index(string? keyword = "", int? subjectId = -1, string nameFilter = "", int page = 1)
        {
            page = page<1 ? 1 : page;
            int pageSize = 10;

            ViewData["Keyword"] = keyword;
            ViewData["SubjectId"] = subjectId;
            ViewData["NameFilter"] = nameFilter;
            var models = new List<BookTutorViewModel>();
            var currentUserId = HttpContext.Session.GetInt32("LoginId");

            var approvedStatus = _db.Xetduyets.FirstOrDefault(it => it.TenTrangThai == "Đã xét duyệt");
            var subjects = _db.Mongiasus.ToList();
            var wishlish = _db.Giasuyeuthichs.Where(it => it.NguoidungId == currentUserId).ToList();
            var tutors = _db.Taikhoannguoidungs.Where(it => (it.IdxetDuyet == approvedStatus.IdxetDuyet)).ToList();
            if (approvedStatus != null)
            {
                if (nameFilter == "favourite")
                {
                    var favoriteTutors = _db.Giasuyeuthichs.Where(it => (it.NguoidungId == currentUserId)).ToList();
                    var isInWishlish = false;
                    foreach (var giasu in tutors)
                    {
                        foreach (var user in favoriteTutors)
                        {
                            if (giasu.Id != currentUserId)
                            {
                                if (giasu.Id == user.GiasuId)
                                {
                                    string avatar = giasu.AnhDaiDien;
                                    if (!string.IsNullOrEmpty(avatar))
                                        avatar = avatar.TrimStart('[', '"').TrimEnd('"', ']').Replace("\\\\", "/");
                                    giasu.AnhDaiDien = avatar;

                                    var subject1 = subjects.FirstOrDefault(it => it.IdmonGiaSu == giasu.IdmonGiaSu1);
                                    var subject2 = subjects.FirstOrDefault(it => it.IdmonGiaSu == giasu.IdmonGiaSu2);
                                    foreach (var like in wishlish)
                                    {
                                        if (like.GiasuId == giasu.Id)
                                        {
                                            isInWishlish = true;
                                            break;
                                        }
                                        else
                                        {
                                            isInWishlish = false;
                                        }
                                    }

                                    var commentModel = new List<CommentViewModel>();
                                    var danhGiaGiaSu = _db.Danhgiagiasus.Where(it => it.GiasuId == giasu.Id).ToList();
                                    foreach (var danhGia in danhGiaGiaSu)
                                    {
                                        var nguoiDanhGia = _db.Taikhoannguoidungs.Find(danhGia.NguoidungId);
                                        if (nguoiDanhGia != null)
                                        {
                                            string avt = nguoiDanhGia.AnhDaiDien;
                                            if (!string.IsNullOrEmpty(avt))
                                            {
                                                avt = avt.TrimStart('[', '"').TrimEnd('"', ']').Replace("\\\\", "/");
                                                nguoiDanhGia.AnhDaiDien = avt;
                                            }
                                            else
                                            {
                                                nguoiDanhGia.AnhDaiDien = null;
                                            }
                                            commentModel.Add(new CommentViewModel
                                            {
                                                Comment = danhGia,
                                                NguoiDanhGia = nguoiDanhGia
                                            });
                                        }
                                    }
                                    models.Add(new BookTutorViewModel
                                    {
                                        Tutor = giasu,
                                        Subject1 = subject1,
                                        Subject2 = subject2,
                                        IsInWishlish = isInWishlish,
                                        Commnents = commentModel
                                    });
                                }
                            }
                        }
                    }
                }
                else if (nameFilter == "favourite" && subjectId > 0)
                {
                    tutors = tutors.Where(it => it.IdmonGiaSu1 == subjectId || it.IdmonGiaSu2 == subjectId).ToList();
                    var favoriteTutors = _db.Giasuyeuthichs.Where(it => (it.NguoidungId == currentUserId)).ToList();
                    ViewData["NameFilter"] = nameFilter;
                    var isInWishlish = false;
                    foreach (var giasu in tutors)
                    {
                        if (giasu.Id != currentUserId)
                        {
                            foreach (var user in favoriteTutors)
                            {
                                if (giasu.Id == user.GiasuId)
                                {
                                    string avatar = giasu.AnhDaiDien;
                                    if (!string.IsNullOrEmpty(avatar))
                                        avatar = avatar.TrimStart('[', '"').TrimEnd('"', ']').Replace("\\\\", "/");
                                    giasu.AnhDaiDien = avatar;

                                    var subject1 = subjects.FirstOrDefault(it => it.IdmonGiaSu == giasu.IdmonGiaSu1);
                                    var subject2 = subjects.FirstOrDefault(it => it.IdmonGiaSu == giasu.IdmonGiaSu2);
                                    foreach (var like in wishlish)
                                    {
                                        if (like.GiasuId == giasu.Id)
                                        {
                                            isInWishlish = true;
                                            break;
                                        }
                                        else
                                        {
                                            isInWishlish = false;
                                        }
                                    }

                                    var commentModel = new List<CommentViewModel>();
                                    var danhGiaGiaSu = _db.Danhgiagiasus.Where(it => it.GiasuId == giasu.Id).ToList();
                                    foreach (var danhGia in danhGiaGiaSu)
                                    {
                                        var nguoiDanhGia = _db.Taikhoannguoidungs.Find(danhGia.NguoidungId);
                                        if (nguoiDanhGia != null)
                                        {
                                            nguoiDanhGia.AnhDaiDien = nguoiDanhGia.AnhDaiDien.TrimStart('[', '"').TrimEnd('"', ']').Replace("\\\\", "/");
                                            commentModel.Add(new CommentViewModel
                                            {
                                                Comment = danhGia,
                                                NguoiDanhGia = nguoiDanhGia
                                            });
                                        }
                                    }
                                    models.Add(new BookTutorViewModel
                                    {
                                        Tutor = giasu,
                                        Subject1 = subject1,
                                        Subject2 = subject2,
                                        IsInWishlish = isInWishlish,
                                        Commnents = commentModel
                                    });
                                }
                            }

                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        tutors = tutors.Where(it => it.HoTen.ToLower().Contains(keyword.ToLower())).ToList();
                    }

                    if (subjectId > 0)
                    {
                        tutors = tutors.Where(it => it.IdmonGiaSu1 == subjectId || it.IdmonGiaSu2 == subjectId).ToList();
                    }

                    var isInWishlish = false;

                    foreach (var giasu in tutors)
                    {
                        if (giasu.Id != currentUserId)
                        {
                            string avatar = giasu.AnhDaiDien;
                            if (!string.IsNullOrEmpty(avatar))
                                avatar = avatar.TrimStart('[', '"').TrimEnd('"', ']').Replace("\\\\", "/");
                            giasu.AnhDaiDien = avatar;

                            var subject1 = subjects.FirstOrDefault(it => it.IdmonGiaSu == giasu.IdmonGiaSu1);
                            var subject2 = subjects.FirstOrDefault(it => it.IdmonGiaSu == giasu.IdmonGiaSu2);
                            foreach (var like in wishlish)
                            {
                                if (like.GiasuId == giasu.Id)
                                {
                                    isInWishlish = true;
                                    break;
                                }
                                else
                                {
                                    isInWishlish = false;
                                }
                            }

                            var commentModel = new List<CommentViewModel>();
                            var danhGiaGiaSu = _db.Danhgiagiasus.Where(it => it.GiasuId == giasu.Id).ToList();
                            foreach (var danhGia in danhGiaGiaSu)
                            {
                                var nguoiDanhGia = _db.Taikhoannguoidungs.Find(danhGia.NguoidungId);
                                if (nguoiDanhGia != null)
                                {
                                    string avt = nguoiDanhGia.AnhDaiDien;
                                    if (!string.IsNullOrEmpty(avt))
                                    {
                                        avt = avt.TrimStart('[', '"').TrimEnd('"', ']').Replace("\\\\", "/");
                                        nguoiDanhGia.AnhDaiDien = avt;
                                    }
                                    else
                                    {
                                        nguoiDanhGia.AnhDaiDien = "https://cdn.icon-icons.com/icons2/1378/PNG/512/avatardefault_92824.png";
                                    }
                                    commentModel.Add(new CommentViewModel
                                    {
                                        Comment = danhGia,
                                        NguoiDanhGia = nguoiDanhGia
                                    });
                                }
                            }
                            models.Add(new BookTutorViewModel
                            {
                                Tutor = giasu,
                                Subject1 = subject1,
                                Subject2 = subject2,
                                IsInWishlish = isInWishlish,
                                Commnents = commentModel
                            });
                        }
                    }
                }
            }
            ViewBag.Subjects = subjects;
       
            return View(models.ToPagedList(page,pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToWishlish(int id)
        {
            var currentUserId = HttpContext.Session.GetInt32("LoginId");
            if (currentUserId != null)
            {
                if (currentUserId > 0 && id > 0)
                {
                    var giaSuYeuThich = new Giasuyeuthich
                    {
                        GiasuId = id,
                        NguoidungId = currentUserId.Value
                    };
                    _db.Add(giaSuYeuThich);
                    await _db.SaveChangesAsync();
                    TempData["Message"] = "Thêm gia sư yêu thích thành công!";
                    TempData["MessageType"] = "success";
                }
            }
            else
            {
                TempData["Message"] = "Vui lòng đăng nhập để thêm gia sư yêu thích!";
                TempData["MessageType"] = "error";
            }
            return RedirectToAction("Index", "BookTutor");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromWishlish(int id)
        {
            var currentUserId = HttpContext.Session.GetInt32("LoginId");
            if (currentUserId > 0 && id > 0)
            {
                var giaSuYeuThich = _db.Giasuyeuthichs.FirstOrDefault(it => it.GiasuId == id && it.NguoidungId == currentUserId.Value);
                if (giaSuYeuThich != null)
                {
                    _db.Remove(giaSuYeuThich);
                    await _db.SaveChangesAsync();
                    TempData["Message"] = "Bỏ yêu thích thành công!";
                    TempData["MessageType"] = "success";
                }
            }
            return RedirectToAction("Index", "BookTutor");
        }

        public IActionResult DetailTutor(int id)
        {
            var tutor = _db.Taikhoannguoidungs.FirstOrDefault(it => it.Id == id);

            if (tutor == null)
            {
                return NotFound();
            }
            var subjects = _db.Mongiasus.ToList();

            string avatar = tutor.AnhDaiDien;
            if (!string.IsNullOrEmpty(avatar))
            {
                avatar = avatar.TrimStart('[', '"').TrimEnd('"', ']').Replace("\\\\", "/");
                tutor.AnhDaiDien = avatar;
            }
            else
            {
                tutor.AnhDaiDien = "https://cdn.icon-icons.com/icons2/1378/PNG/512/avatardefault_92824.png";
            }

            var subject1 = subjects.FirstOrDefault(it => it.IdmonGiaSu == tutor.IdmonGiaSu1);
            var subject2 = subjects.FirstOrDefault(it => it.IdmonGiaSu == tutor.IdmonGiaSu2);

            var commentModel = new List<CommentViewModel>();

            var giaSus = (from danhGiaGiaSu in _db.Danhgiagiasus
                          join caday in _db.Cadays on danhGiaGiaSu.IdCaDay equals caday.Id
                          where caday.IdnguoiDay == tutor.Id
                          select new { caday.IdnguoiHoc, danhGiaGiaSu }).ToList();

            var tieuChis = _db.Tieuchidanhgias.ToList();

            foreach (var danhGia in giaSus)
            {
                var nguoiDanhGia = _db.Taikhoannguoidungs.Find(danhGia.IdnguoiHoc);
                if (nguoiDanhGia != null)
                {
                    string avt = nguoiDanhGia.AnhDaiDien;
                    if (!string.IsNullOrEmpty(avt))
                    {
                        avt = avt.TrimStart('[', '"').TrimEnd('"', ']').Replace("\\\\", "/");
                        nguoiDanhGia.AnhDaiDien = avt;
                    }
                    else
                    {
                        nguoiDanhGia.AnhDaiDien = null;
                    }
                    commentModel.Add(new CommentViewModel
                    {
                        Comment = danhGia.danhGiaGiaSu,
                        Tieuchi = tieuChis,
                        NguoiDanhGia = nguoiDanhGia
                    });
                }
            }

            var model = new BookTutorViewModel
            {
                Tutor = tutor,
                Subject1 = subject1,
                Subject2 = subject2,
                Commnents = commentModel
            };

            List<Caday> cadays = _db.Cadays.Where(ca => ca.IdnguoiDay.Equals(tutor.Id)).ToList();
            foreach (var cadayItem in cadays)
            {
                cadayItem.tenMonDay = _db.Mongiasus.Find(cadayItem.IdmonDay).TenMonGiaSu.ToString();
                cadayItem.giaCaDay = _db.Cahocs.Find(cadayItem.IdloaiCaDay).GiaTien;
            }

            Tuple<BookTutorViewModel, IEnumerable<Caday>> turple = new Tuple<BookTutorViewModel, IEnumerable<Caday>>(model, cadays.AsEnumerable());
            return View(turple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int id, int diem, string danhGia)
        {
            var currentUserId = HttpContext.Session.GetInt32("LoginId");
            if (currentUserId > 0 && id > 0)
            {
                var comment = new Danhgiagiasu
                {
                    GiasuId = id,
                    Diem = diem,
                    DanhGia = danhGia,
                    NguoidungId = currentUserId.Value,
                    NgayTao = DateTime.Now
                };

                _db.Add(comment);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "BookTutor");
        }


        public IActionResult HistoryBooking(int id)
        {

            List<Caday> cadays = _db.Cadays.Where(ca => ca.IdnguoiHoc.Equals(id)).ToList();
            foreach (var cadayItem in cadays)
            {
                cadayItem.tenNguoiDay = _db.Taikhoannguoidungs.Find(cadayItem.IdnguoiDay).HoTen.ToString();
                cadayItem.tenMonDay = _db.Mongiasus.Find(cadayItem.IdmonDay).TenMonGiaSu.ToString();
                cadayItem.giaCaDay = _db.Cahocs.Find(cadayItem.IdloaiCaDay).GiaTien;
            }

            return View(cadays);
        }

        [HttpPost]
        public IActionResult AcceptBooking(int id)
        {
            Caday caday = _db.Cadays.FirstOrDefault(m => m.Id == id);
            caday.TrangThai = true;

            _db.Update(caday);
            _db.SaveChanges();

            TempData["link"] = caday.Link;

            return RedirectToAction("HistoryBooking", "BookTutor", new { id });
        }

        [HttpPost]
        public IActionResult CancelBooking(int lessonPlanId)
        {
            Caday caday = _db.Cadays.FirstOrDefault(c => c.Id.Equals(lessonPlanId));

            int id = (int)caday.IdnguoiHoc;

            int year = caday.NgayDay.Year;
            int month = caday.NgayDay.Month;
            int day = caday.NgayDay.Day;

            DateTime checkTime = new DateTime(year, month, day, caday.GioBatDau, caday.PhutBatDau, 0);

            TimeSpan result = DateTime.Now - checkTime;

            if(result.Days <= 0 && Math.Abs(result.Hours) > 1)
            {
                Cahoc cahoc = _db.Cahocs.Where(c => c.IdCaHoc == caday.IdloaiCaDay).FirstOrDefault();
                Phiday phiday = _db.Phidays.Where(ph => ph.Id == 1).FirstOrDefault();

                float commision = (int)cahoc.GiaTien * ((float)phiday.ChietKhau / 100);
                int money = (int)(cahoc.GiaTien - commision);

                MoneyServices.SubtractMoney(money, caday.IdnguoiDay, _db); 
                MoneyServices.AddMoney((int)cahoc.GiaTien, (int)caday.IdnguoiHoc, _db); 
            }

            try 
            {
                caday.Link = null;
                caday.TrangThai = false;
                _db.Update(caday);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return RedirectToAction("HistoryBooking", "BookTutor", new {id});
        }

        [HttpPost]
        public async Task<IActionResult> LessonRegis([FromForm] int lessonId)
        {
            if (HttpContext.Session.GetInt32("LoginId") == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            Caday caday = _db.Cadays.FirstOrDefault(c => c.Id.Equals(lessonId));
            Cahoc cahoc = _db.Cahocs.Where(c => c.IdCaHoc == caday.IdloaiCaDay).FirstOrDefault();
            int soDu = (int)_db.Taikhoannguoidungs.Find(HttpContext.Session.GetInt32("LoginId")).SoDuVi;

            if (soDu < cahoc.GiaTien)
            {
                TempData["Message"] = "Số dư ví không đủ, vui lòng nạp thêm!";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index", "Wallet");
            }

            JwtConnectionInfo connectionInfo = new JwtConnectionInfo("9wPjAoQIQsSEzltlIl_vQw", "84zfXjpKoHTUS2Tqjnfswk7pyezmMsbYRxvf");
            ZoomClient zoomClient = new ZoomClient(connectionInfo);

            var userId = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
            bool isOverLapse = CheckLessonHasRegister(userId.Id, caday.NgayDay, caday.GioBatDau, caday.PhutBatDau, caday.GioKetThuc, caday.PhutKetThuc);
            if (isOverLapse) {
                TempData["Message"] = "Thời gian bị trùng với ca dạy khác";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index", "BookTutor");
            }

            var hostMail = _db.Taikhoannguoidungs.Where(acc => acc.Id.Equals(caday.IdnguoiDay)).FirstOrDefault().Email;
            int lessonDuration = _db.Cahocs.Where(l => l.IdCaHoc.Equals(caday.IdloaiCaDay)).FirstOrDefault().LoaiCa;
            var result = await zoomClient.Meetings.CreateScheduledMeetingAsync(hostMail, "Buổi dạy và học gia sư Văn Lang", "Buổi dạy và học gia sư Văn Lang", caday.NgayDay, lessonDuration);
            caday.Link = result.JoinUrl;
            caday.IdnguoiHoc = HttpContext.Session.GetInt32("LoginId");
            caday.TrangThai = false;
            var monDay = _db.Mongiasus.Where(acc => acc.IdmonGiaSu.Equals(caday.IdmonDay)).FirstOrDefault().TenMonGiaSu;

            // wallet manage
            Phiday phiday = _db.Phidays.Where(ph => ph.Id == 1).FirstOrDefault();

            float commision = (int)cahoc.GiaTien * ((float)phiday.ChietKhau / 100);
            int money = (int)(cahoc.GiaTien - commision);

            MoneyServices.SubtractMoney((int)cahoc.GiaTien, (int) caday.IdnguoiHoc, _db); 
            MoneyServices.AddMoney(money, caday.IdnguoiDay, _db); 

            try
            {
                _db.Update(caday);
                await _db.SaveChangesAsync();
                TempData["Message"] = "Đặt lịch thành công!";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return RedirectToAction("SendMail", "BookTutor",
            new
            {
                toEmail = hostMail,
                mailBody = "<b>Xin thông báo! Ca dạy môn <b style=\"color: red;\">" + monDay + "</b> có thời gian " + caday.GioBatDau + ":" + caday.PhutBatDau + " - " + caday.GioKetThuc + ":" + caday.PhutKetThuc + " ngày " + caday.NgayDay.ToString("dd/MM/yyyy") + " đã được đặt!</b>" +
    "<p style = \"margin: 0%;\">Vui lòng chuẩn bị thật tốt cho buổi dạy, chúc bạn sẽ có một buổi dạy thật tốt!<br/>",
                id = caday.IdnguoiHoc
            });
        }

        private bool CheckLessonHasRegister(int learnerId, DateTime regisDate, int startHour, int startMinute, int endHour, int endMinute) 
        {
            List<Caday> caDayByLearner = _db.Cadays.Where(c => c.IdnguoiHoc == learnerId).ToList();
            List<Caday> caDayByDate = caDayByLearner.Where(c => c.NgayDay.Date == regisDate.Date).ToList();

            if (caDayByDate.Count == 0 || caDayByLearner.Count == 0)
            {
                return false;
            }

            TimeSpan startTime = new TimeSpan(startHour, startMinute, 0);
            TimeSpan endTime = new TimeSpan(endHour, endMinute, 0);

            bool isOverLapse = false;

            foreach (var caDay in caDayByDate) {
                TimeSpan caDayStartTime = new TimeSpan(caDay.GioBatDau, caDay.PhutBatDau, 0);
                TimeSpan caDayEndTime = new TimeSpan(caDay.GioKetThuc, caDay.PhutKetThuc, 0);

                isOverLapse = startTime <= caDayEndTime && caDayStartTime <= endTime;
                if (isOverLapse) {
                    break;
                }
            }

            return isOverLapse;
        }

        public IActionResult SendMail(string toEmail, string mailBody, int id)
        {
            string mailTitle = "Gia Sư Văn Lang";
            string fromMail = "giasuvanlang.thongtin@gmail.com";
            string fromEmailPass = "vrzaiqmdiycujvas";
            string bodyMail = "<!DOCTYPE html>" +
        "<html>" +
            "<body>" +
                "<p style = \"margin: 0%;\">" +
                "Xin chào gia sư,<br/>" +
                mailBody + "<br/>" +

               " Trân trọng!<br/>" +
                "<b>Gia Sư Văn Lang</b><br/>" +
                "<b style = \"font-size: 10px;text-align: center; margin: 0%;\"> Bạn nhận được thông báo này vì địa chỉ email " + toEmail + " đang được sử dụng cho " +
                "tài khoản trên trang Gia Sư Văn Lang. Nếu thông tin này không chính xác," +
                "vui lòng bỏ qua và không trả lời lại mail này.<br/>Xin cảm ơn!</b>" +
            "</body>" +
        "</html>";

            //Email and content
            MailMessage message = new MailMessage(new MailAddress(fromMail, mailTitle), new MailAddress(toEmail));
            message.Subject = "[VLUTUTORS] Thông báo đặt lịch dạy";
            message.Body = bodyMail;
            message.IsBodyHtml = true;

            //Server detail
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            //Credentials
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential();
            credential.UserName = fromMail;
            credential.Password = fromEmailPass;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credential;

            smtp.Send(message);

            return RedirectToAction("HistoryBooking", new { id });
        }

        /// <summary>
        /// Đánh gia tiêu chí đã được đánh giá gia sư của người học.
        /// </summary>
        /// <param name="idGiaSu">Int</param>
        /// <returns>IReadOnlyCollection -> GetAllEvaluationCriteriaTutorResponse</returns>
        public async Task<IReadOnlyCollection<GetAllEvaluationCriteriaTutorResponse>> GetAllEvaluationCriteriaTutor(int idGiaSu)
        {
            const string TIEU_CHI_DANH_GIA_GIA_SU = "Gia sư";
            List<GetAllEvaluationCriteriaTutorResponse> result = new();
            if (idGiaSu is 0)
            {
                return result;
            }

            List<string> danhGias = await (from danhGiaGiaSu in _db.Danhgiagiasus
                                           join caDay in _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay.Date <= DateTime.Now.Date)
                                           on danhGiaGiaSu.IdCaDay equals caDay.Id
                                           where caDay.IdnguoiDay == idGiaSu
                                           select danhGiaGiaSu.TieuChi).ToListAsync();

            if (danhGias is null)
            {
                return result;
            }

            List<int> tieuChiIds = new();
            List<Tieuchidanhgia> tieuChiDanhGias = await _db.Tieuchidanhgias.Where(x => x.DanhCho == TIEU_CHI_DANH_GIA_GIA_SU).ToListAsync();
            danhGias.ForEach(tieuChi =>
            {
                if (!string.IsNullOrEmpty(tieuChi))
                {
                    var tieuChiDanhGia = tieuChi?.Split(";").Select(tieuChiId => int.TryParse(tieuChiId, out int output) ? output : 0);
                    if (tieuChiDanhGia is not null)
                    {
                        tieuChiIds.AddRange(tieuChiDanhGias.Where(x => tieuChiDanhGia.Contains(x.IdTieuChi)).Select(x => x.IdTieuChi).ToList());
                    }
                }
            });

            foreach (var tieuChi in tieuChiDanhGias)
            {
                var tongDanhGia = tieuChiIds.LongCount(tieuchi => tieuchi == tieuChi.IdTieuChi);

                GetAllEvaluationCriteriaTutorResponse model = new()
                {
                    IdTieuChi = tieuChi.IdTieuChi,
                    TenTieuChi = tieuChi.TieuChi,
                    TongDiem = tongDanhGia
                };

                result.Add(model);
            }
            return result;
        }
    }
}
