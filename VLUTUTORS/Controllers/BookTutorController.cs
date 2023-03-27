using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using VLUTUTORS.Models;

namespace VLUTUTORS.Controllers
{
    public class BookTutorController : Controller
    {
        private CP25Team01Context _db = new CP25Team01Context();
        public async Task<IActionResult> Index(string? keyword = "", int? subjectId = -1, string nameFilter = "")
        {
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
                        tutors = tutors.Where(it => it.HoTen.Contains(keyword)).ToList();
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
            ViewBag.Subjects = subjects;
            return View(models);
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

                else
                {
                    TempData["Message"] = "Vui lòng đăng nhập để thêm gia sư yêu thích!";
                    TempData["MessageType"] = "error";
                }
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
                avatar = avatar.TrimStart('[', '"').TrimEnd('"', ']').Replace("\\\\", "/");
            tutor.AnhDaiDien = avatar;

            var subject1 = subjects.FirstOrDefault(it => it.IdmonGiaSu == tutor.IdmonGiaSu1);
            var subject2 = subjects.FirstOrDefault(it => it.IdmonGiaSu == tutor.IdmonGiaSu2);

            var commentModel = new List<CommentViewModel>();
            var danhGiaGiaSu = _db.Danhgiagiasus.Where(it => it.GiasuId == tutor.Id).ToList();
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
                    Danhgia = danhGia,
                    NguoidungId = currentUserId.Value,
                    NgayTao = DateTime.Now
                };

                _db.Add(comment);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "BookTutor");
        }

        public async Task<IActionResult> HistoryBooking()
        {
            return View();
        }
    }
}
