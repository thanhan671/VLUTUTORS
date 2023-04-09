using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Areas.Tutors.Requests.ManagerTeachHistorys;
using VLUTUTORS.Areas.Tutors.Responses.ManagerTeachHistorys;
using VLUTUTORS.Models;
using VLUTUTORS.Responses;
using VLUTUTORS.Responses.ManageTeachHistorys;

namespace VLUTUTORS.Areas.Tutors.Controllers

{
    [Area("Tutors")]
    public class ManageTeachHistoryController : Controller
    {
        private readonly CP25Team01Context _db = new();

        #region View

        [HttpGet]
        public async Task<IActionResult> Index(GetAllManagerTeachHistoryRequest request)
        {
            IReadOnlyCollection<GetAllTeachHistoryResponse> result = await GetAllTeachHistoryByFilter(request);
            return View(result);
        }

        /// <summary>
        /// Thêm thông tin đánh giá người học của gia sư.
        /// </summary>
        /// <param name="request">CreateLearnerEvaluationRequest</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLearnerEvaluation(CreateLearnerEvaluationRequest request)
        {
            int? userId = await IsExistUser();
            if (userId is null)
            {
                TempData["Message"] = "Bạn không có quyền thực hiện chức năng này !";
                TempData["MessageType"] = "error";

                return RedirectToAction("Index", "ManageTeachHistory");
            }

            if (ModelState.IsValid)
            {
                var existEvaluation = await (from caDay in _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay.Date <= DateTime.Now.Date) // Ca dạy đã hoàn tất

                                             join danhGiaNguoiHoc in _db.Danhgianguoihocs on caDay.Id equals danhGiaNguoiHoc.IdCaDay into nhomNguoiHoc
                                             from danhGiaNguoiHoc in nhomNguoiHoc.DefaultIfEmpty()

                                             where caDay.Id == request.IdCaDay && caDay.IdnguoiDay == userId
                                             select new { danhGiaNguoiHoc, caDay }).FirstOrDefaultAsync();

                if (existEvaluation is null)
                {
                    TempData["Message"] = "Không tìm thấy thông tin yêu cầu !";
                    TempData["MessageType"] = "error";

                    return RedirectToAction("Index", "ManageTeachHistory");
                }

                if (existEvaluation.caDay is null || existEvaluation.danhGiaNguoiHoc is not null)
                {
                    TempData["Message"] = "Đã đánh giá người học với buổi dạy này rồi.";
                    TempData["MessageType"] = "error";

                    return RedirectToAction("Index", "ManageTeachHistory");
                }

                Danhgianguoihoc model = new()
                {
                    Diem = request.Diem ?? 0,
                    IdCaDay = request.IdCaDay,
                    IdGiaSu = existEvaluation.caDay.IdnguoiDay,
                    IdNguoiDung = existEvaluation.caDay.IdnguoiHoc.Value,
                    DanhGia = request.DanhGia ?? string.Empty,
                    TieuChi = string.Join(";", request.TieuChi)
                };

                await _db.Danhgianguoihocs.AddAsync(model);
                bool result = await _db.SaveChangesAsync() > 0;

                if (result)
                {
                    TempData["Message"] = "Đánh giá người học thành công.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("Index", "ManageTeachHistory");
                }
            }

            TempData["Message"] = "Thực hiện yêu cầu không thành công.";
            TempData["MessageType"] = "error";
            return RedirectToAction("Index", "ManageTeachHistory");
        }

        [HttpGet]
        public async Task<IActionResult> DetailLearnerEvaluation(GetOneLearnerEvaluationRequest request)
        {
            GetLearnerEvaluationResponse result = await GetOneLearnerEvaluation(request);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> DetailTutorEvaluation(GetOneLearnerEvaluationRequest request)
        {
            GetTutorEvaluationReponse result = await GetOneTutorEvaluation(request);
            return View(result);
        }

        #endregion

        #region Method 

        /// <summary>
        /// Xem danh sách lịch sử dạy của gia sư.
        /// </summary>
        /// <param name="request">GetAllManagerTeachHistoryRequest</param>
        /// <returns>IReadOnlyCollection -> GetAllTeachHistoryResponse </returns>
        public async Task<IReadOnlyCollection<GetAllTeachHistoryResponse>> GetAllTeachHistoryByFilter(GetAllManagerTeachHistoryRequest request)
        {
            const string zeroDefault = "00";

            int? userId = await IsExistUser();
            if (userId is null)
            {
                return new List<GetAllTeachHistoryResponse>();
            }

            IQueryable<GetAllTeachHistoryResponse> query = from caday in _db.Cadays
                                                           join mongiasu in _db.Mongiasus on caday.IdmonDay equals mongiasu.IdmonGiaSu
                                                           join cahoc in _db.Cahocs on caday.IdloaiCaDay equals cahoc.IdCaHoc

                                                           join user in _db.Taikhoannguoidungs on caday.IdnguoiHoc equals user.Id into userGroup
                                                           from user in userGroup.DefaultIfEmpty()

                                                           join danhGiaGiaSu in _db.Danhgiagiasus on caday.Id equals danhGiaGiaSu.IdCaDay into nhomGiaSu
                                                           from danhGiaGiaSu in nhomGiaSu.DefaultIfEmpty()

                                                           join danhGiaNguoiHoc in _db.Danhgianguoihocs on caday.Id equals danhGiaNguoiHoc.IdCaDay into nhomNguoiHoc
                                                           from danhGiaNguoiHoc in nhomNguoiHoc.DefaultIfEmpty()

                                                           where caday.IdnguoiDay == userId
                                                           orderby caday.Id descending

                                                           let thoiGianBatDau =  $"{caday.GioBatDau}:{(caday.PhutBatDau > 0 ? caday.PhutBatDau.ToString(zeroDefault) : zeroDefault)}"
                                                           let thoiGianKetThuc = $"{caday.GioKetThuc}:{(caday.PhutKetThuc > 0 ? caday.PhutKetThuc.ToString(zeroDefault) : zeroDefault)}"

                                                           select new GetAllTeachHistoryResponse()
                                                           {
                                                               IdCaDay = caday.Id,
                                                               TenMonGiaSu = mongiasu.TenMonGiaSu,
                                                               IdNguoiDay = caday.IdnguoiDay,
                                                               IdNguoiHoc = caday.IdnguoiHoc,
                                                               TenNguoiHoc = user.HoTen,
                                                               NgayDay = caday.NgayDay.Date,
                                                               ThoiGianBatDau = thoiGianBatDau,
                                                               ThoiGianKetThuc = thoiGianKetThuc,
                                                               GiaTien = cahoc.GiaTien > 0 ? cahoc.GiaTien.ToString("#,##0.##") : "0",
                                                               TrangThai = caday.TrangThai,

                                                               TenTrangThai = (caday.TrangThai == true) ? "Hoàn thành" : ((caday.TrangThai == false) ? "Đã hủy" : "Không ai đăng ký"), //TODO: Not rule clear this

                                                               IsExistTutorEvaluation = danhGiaGiaSu != null, // Trường kiểm tra ca học đã được gia sư đánh giá hay chưa
                                                               IsExistLearnerEvaluation = danhGiaNguoiHoc != null // Trường kiểm tra ca học đã được người học đánh giá chưa
                                                           };

            if (request.FromDate is not null)
            {
                query = query.Where(x => x.NgayDay <= request.FromDate.Value.Date);
            }
            if (request.ToDate is not null)
            {
                query = query.Where(x => x.NgayDay >= request.ToDate.Value.Date);
            }

            if (!string.IsNullOrEmpty(request.Search))
            {
                request.Search = request.Search.Trim().ToLower();
                query = query.Where(x => x.TenMonGiaSu.ToLower().Contains(request.Search) || x.TenNguoiHoc.ToLower().Contains(request.Search) || x.IdCaDay.ToString().Contains(request.Search));
            }

            query = query?.Skip((request.PageNumber - 1) * request.PageSize) // Bỏ qua số lượng sản phẩm của các trang trước                                                                  
                ?.Take(request.PageSize); // Lấy số lượng sản phẩm của trang hiện tại

            return await query.ToListAsync();
        }

        /// <summary>
        /// Lấy thông tin chi tiết đánh giá người học của gia sư.
        /// </summary>
        /// <param name="request">GetOneTutorEvaluationRequest</param>
        /// <returns>GetTutorEvaluationReponse</returns>
        private async Task<GetLearnerEvaluationResponse> GetOneLearnerEvaluation(GetOneLearnerEvaluationRequest request)
        {
            int? userId = await IsExistUser();
            if (userId is null || request.IdCaDay is 0)
            {
                return null;
            }

            GetLearnerEvaluationResponse result = await (from danhNguoiHoc in _db.Danhgianguoihocs
                                                      join caday in _db.Cadays on danhNguoiHoc.IdCaDay equals caday.Id
                                                      join mongiasu in _db.Mongiasus on caday.IdmonDay equals mongiasu.IdmonGiaSu

                                                      join nguoiHoc in _db.Taikhoannguoidungs on caday.IdnguoiHoc equals nguoiHoc.Id into nhomGiaSu
                                                      from nguoiHoc in nhomGiaSu.DefaultIfEmpty()

                                                      where danhNguoiHoc.IdCaDay == request.IdCaDay && caday.IdnguoiDay == userId
                                                      select new GetLearnerEvaluationResponse
                                                      {
                                                          NgayDay = caday.NgayDay,
                                                          Diem = danhNguoiHoc.Diem,
                                                          DanhGia = danhNguoiHoc.DanhGia,
                                                          NgayTao = danhNguoiHoc.NgayTao,
                                                          IdNguoiDay = caday.IdnguoiDay,
                                                          IdNguoiHoc = nguoiHoc.Id,
                                                          TenNguoiHoc = nguoiHoc.HoTen,
                                                          TenMonGiaSu = mongiasu.TenMonGiaSu,
                                                          IdCaDay = caday.Id,
                                                          IdDanhGiaNguoiHoc = danhNguoiHoc.Id,
                                                      }).FirstOrDefaultAsync();

            if (result is not null)
            {
                Danhgianguoihoc danhGiaGiaSu = await _db.Danhgianguoihocs.FirstOrDefaultAsync(x => x.Id == result.IdDanhGiaNguoiHoc);
                var tieuChiCuaGiaSus = danhGiaGiaSu.TieuChi?.Split(";").Select(tieuChiId => int.TryParse(tieuChiId, out int output) ? output : 0);

                if (tieuChiCuaGiaSus is not null)
                {
                    result.CriteriaEvaluations = await _db.Tieuchidanhgias.Where(x => tieuChiCuaGiaSus.Contains(x.IdTieuChi))?
                        .Select(x => new CriteriaEvaluationModel()
                        {
                            IdTieuChi = x.IdTieuChi,
                            TenTieuChi = x.TieuChi
                        }).ToListAsync();
                }
            }

            return result;
        }

        /// <summary>
        /// Lấy thông tin chi tiết đánh giá gia sư của người học.
        /// </summary>
        /// <param name="request">GetOneTutorEvaluationRequest</param>
        /// <returns>GetTutorEvaluationReponse</returns>
        private async Task<GetTutorEvaluationReponse> GetOneTutorEvaluation(GetOneLearnerEvaluationRequest request)
        {
            int? userId = await IsExistUser();
            if (userId is null || request.IdCaDay is 0)
            {
                return null;
            }

            GetTutorEvaluationReponse result = await (from danhGiaGiaSu in _db.Danhgiagiasus
                                                      join caday in _db.Cadays on danhGiaGiaSu.IdCaDay equals caday.Id
                                                      join mongiasu in _db.Mongiasus on caday.IdmonDay equals mongiasu.IdmonGiaSu

                                                      join nguoiHoc in _db.Taikhoannguoidungs on caday.IdnguoiHoc equals nguoiHoc.Id into nhomNguoiHoc
                                                      from nguoiHoc in nhomNguoiHoc.DefaultIfEmpty()

                                                      where danhGiaGiaSu.IdCaDay == request.IdCaDay && caday.IdnguoiDay == userId
                                                      select new GetTutorEvaluationReponse
                                                      {
                                                          NgayDay = caday.NgayDay,
                                                          Diem = danhGiaGiaSu.Diem,
                                                          DanhGia = danhGiaGiaSu.DanhGia,
                                                          NgayTao = danhGiaGiaSu.NgayTao,
                                                          IdNguoiHoc = nguoiHoc.Id,
                                                          TenNguoiHoc = nguoiHoc.HoTen,
                                                          TenMonGiaSu = mongiasu.TenMonGiaSu,
                                                          IdCaDay = caday.Id,
                                                          IdDanhGiaGiaSu = danhGiaGiaSu.Id,
                                                      }).FirstOrDefaultAsync();

            if (result is not null)
            {
                Danhgiagiasu danhGiaGiaSu = await _db.Danhgiagiasus.FirstOrDefaultAsync(x => x.Id == result.IdDanhGiaGiaSu);
                var tieuChiCuaNguoiHocs = danhGiaGiaSu.TieuChi?.Split(";").Select(tieuChiId => int.TryParse(tieuChiId, out int output) ? output : 0);

                if (tieuChiCuaNguoiHocs is not null)
                {
                    result.CriteriaEvaluations = await _db.Tieuchidanhgias.Where(x => tieuChiCuaNguoiHocs.Contains(x.IdTieuChi))?
                        .Select(x => new CriteriaEvaluationModel
                        {
                            IdTieuChi = x.IdTieuChi,
                            TenTieuChi = x.TieuChi
                        }).ToListAsync();
                }
            }

            return result;
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
