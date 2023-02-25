using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index(string? keyword = "", int? subjectId = -1)
        {
            ViewData["Keyword"] = keyword;
            ViewData["SubjectId"] = subjectId;
            var models = new List<BookTutorViewModel>();
            var currentUserId = HttpContext.Session.GetInt32("LoginId");

            var approvedStatus = _db.Xetduyets.FirstOrDefault(it => it.TenTrangThai == "Đã xét duyệt");
            var subjects = _db.Mongiasus.ToList();
            var wishlish = _db.Giasuyeuthichs.Where(it => it.NguoidungId == currentUserId).ToList();
            if (approvedStatus != null)
            {
                var tutors = _db.Taikhoannguoidungs.Where(it => (it.IdxetDuyet == approvedStatus.IdxetDuyet)).ToList();
                if (!string.IsNullOrEmpty(keyword))
                    tutors = tutors.Where(it => it.HoTen.Contains(keyword)).ToList();

                if (subjectId > 0)
                    tutors = tutors.Where(it => it.IdmonGiaSu1 == subjectId || it.IdmonGiaSu2 == subjectId).ToList();

                foreach (var tutor in tutors)
                {
                    string avatar = tutor.AnhDaiDien;
                    if (!string.IsNullOrEmpty(avatar))
                        avatar = avatar.TrimStart('[', '"').TrimEnd('"', ']').Replace("\\\\", "/");
                    tutor.AnhDaiDien = avatar;

                    var subject1 = subjects.FirstOrDefault(it => it.IdmonGiaSu == tutor.IdmonGiaSu1);
                    var subject2 = subjects.FirstOrDefault(it => it.IdmonGiaSu == tutor.IdmonGiaSu2);
                    var isInWishlish = wishlish.FirstOrDefault(it => it.GiasuId == tutor.Id);

                    var commentModel = new List<CommentViewModel>();
                    var danhGiaGiaSu = _db.Danhgiagiasus.Where(it=> it.GiasuId == tutor.Id).ToList();
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
                        Tutor = tutor,
                        Subject1 = subject1,
                        Subject2 = subject2,
                        IsInWishlish = (isInWishlish != null),
                        Commnents = commentModel
                    }) ;
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
            if(currentUserId > 0 && id > 0)
            {
                var giaSuYeuThich = new Giasuyeuthich
                {
                    GiasuId = id,
                    NguoidungId = currentUserId.Value
                };
                _db.Add(giaSuYeuThich);
                await _db.SaveChangesAsync();
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
                if(giaSuYeuThich != null)
                {
                    _db.Remove(giaSuYeuThich);
                    await _db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index", "BookTutor");
        }

        public async Task<IActionResult> DetailTutor(int id)
        {
            var tutor = _db.Taikhoannguoidungs.FirstOrDefault(it => it.Id == id);
            if(tutor == null)
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
            return View(model);
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
