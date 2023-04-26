using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using VLUTUTORS.Requests.HistoryOfLearnings;

namespace VLUTUTORS.Controllers
{
    public class MeetingController : Controller
    {
        private readonly CP25Team01Context _db = new();

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RatingTutor(int id)
        {
            if (id is 0)
            {
                TempData["Message"] = "Không tìm thấy Ca dạy !";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index", "HistoryOfLearning");
            }
            ViewData["idCaDay"] = id;
            var tieuChi = await _db.Tieuchidanhgias.ToListAsync();
            return View(tieuChi);

        }

        /// <summary>
        /// Thêm thông tin đánh giá gia sư của người học.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTutorEvaluation(CreateTutorEvaluationRequest request)
        {
            int? userId = await IsExistUser();
            if (userId is null)
            {
                TempData["Message"] = "Bạn không có quyền thực hiện chức năng này !";
                TempData["MessageType"] = "error";

                return RedirectToAction("Index", "HistoryOfLearning");

            }

            if (ModelState.IsValid)
            {
                var existEvaluation = await (from caDay in _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay.Date <= DateTime.Now.Date) // Ca dạy đã hoàn tất

                                             join danhGiaGiaSu in _db.Danhgiagiasus on caDay.Id equals danhGiaGiaSu.IdCaDay into nhomDanhGia
                                             from danhGiaGiaSu in nhomDanhGia.DefaultIfEmpty()

                                             where caDay.Id == request.LessonId && caDay.IdnguoiHoc == userId
                                             select new { danhGiaGiaSu, caDay }).FirstOrDefaultAsync();

                if (existEvaluation is null)
                {
                    TempData["Message"] = "Không tìm thấy thông tin yêu cầu !";
                    TempData["MessageType"] = "error";

                    return RedirectToAction("Index", "HistoryOfLearning", new { id = existEvaluation.caDay.IdnguoiHoc });
                }

                if (existEvaluation.caDay is null || existEvaluation.danhGiaGiaSu is not null)
                {
                    TempData["Message"] = "Đã đánh giá gia sư với buổi dạy này!.";
                    TempData["MessageType"] = "error";

                    return RedirectToAction("Index", "HistoryOfLearning", new { id = existEvaluation.caDay.IdnguoiHoc });
                }

                Danhgiagiasu model = new()
                {
                    Diem = request.Rating ?? 0,
                    IdCaDay = request.LessonId,
                    GiasuId = existEvaluation.caDay.IdnguoiDay,
                    NguoidungId = existEvaluation.caDay.IdnguoiHoc.Value,
                    DanhGia = request.FeedBack ?? string.Empty,
                    TieuChi = string.Join(";", request.Skills)
                };

                await _db.Danhgiagiasus.AddAsync(model);
                bool result = await _db.SaveChangesAsync() > 0;

                if (result)
                {
                    TempData["Message"] = "Đánh giá gia sư thành công.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("Index", "HistoryOfLearning", new { id = existEvaluation.caDay.IdnguoiHoc });
                }
            }

            TempData["Message"] = "Thực hiện yêu cầu không thành công.";
            TempData["MessageType"] = "error";
            return View() /*RedirectToAction("Index", "HistoryOfLearning")*/;
        }

        private async Task<int?> IsExistUser()
        {
            int userId = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo")).Id;

            if (userId is 0)
            {
                return null;
            }

            Taikhoannguoidung result = await _db.Taikhoannguoidungs.FirstOrDefaultAsync(x => x.Id == userId);

            return result?.Id;
        }
    }
}
