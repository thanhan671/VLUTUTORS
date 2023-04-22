using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using VLUTUTORS.Requests.HistoryOfLearnings;
using VLUTUTORS.Responses;
using VLUTUTORS.Responses.HistoryOfLearnings;

namespace VLUTUTORS.Controllers
{
    public class HistoryOfLearningController : Controller
    {
        private readonly CP25Team01Context _db = new();

        #region View

        [HttpGet]
        public IActionResult Index(int id)
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

        [HttpGet]
        public async Task<IActionResult> DetailTutorEvaluation(GetOneTutorEvaluationRequest request)
        {
            GetTutorEvaluationReponse result = await GetOneTutorEvaluation(request);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> DetailLearnerEvaluation(GetOneLearnerEvaluationRequest request)
        {
            GetLearnerEvaluationReponse result = await GetOneLearnerEvaluation(request);
            return View(result);
        }

        #endregion 

        #region Method 



        /// <summary>
        /// Lấy thông tin chi tiết đánh giá gia sư của người học.
        /// </summary>
        /// <param name="request">GetOneTutorEvaluationRequest</param>
        /// <returns>GetTutorEvaluationReponse</returns>
        private async Task<GetTutorEvaluationReponse> GetOneTutorEvaluation(GetOneTutorEvaluationRequest request)
        {
            int? userId = await IsExistUser();
            if (userId is null || request.IdCaDay is 0)
            {
                return null;
            }

            GetTutorEvaluationReponse result = await (from danhGiaGiaSu in _db.Danhgiagiasus
                                                      join caday in _db.Cadays on danhGiaGiaSu.IdCaDay equals caday.Id
                                                      join mongiasu in _db.Mongiasus on caday.IdmonDay equals mongiasu.IdmonGiaSu

                                                      join giaSu in _db.Taikhoannguoidungs on caday.IdnguoiDay equals giaSu.Id into nhomGiaSu
                                                      from giaSu in nhomGiaSu.DefaultIfEmpty()

                                                      where danhGiaGiaSu.IdCaDay == request.IdCaDay && caday.IdnguoiHoc == userId
                                                      select new GetTutorEvaluationReponse
                                                      {
                                                          NgayDay = caday.NgayDay,
                                                          Diem = danhGiaGiaSu.Diem,
                                                          DanhGia = danhGiaGiaSu.DanhGia,
                                                          NgayTao = danhGiaGiaSu.NgayTao,
                                                          TenNguoiDay = giaSu.HoTen,
                                                          IdNguoiDay = giaSu.Id,
                                                          IdNguoiHoc = caday.IdnguoiHoc,
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

        /// <summary>
        /// Lấy danh sách lịch sử học của người học.
        /// </summary>
        /// <param name="request">GetAllHistoryOfLearningRequest</param>
        /// <returns>IReadOnlyCollection -> GetAllHistoryOfLearningResponse</returns>
        private async Task<IReadOnlyCollection<GetAllHistoryOfLearningResponse>> GetAllHistoryOfLearningByFilter(GetAllHistoryOfLearningRequest request)
        {
            const string zeroDefault = "00";
            int? userId = await IsExistUser();
            if (userId is null)
            {
                return new List<GetAllHistoryOfLearningResponse>();
            }

            IQueryable<GetAllHistoryOfLearningResponse> query = from caday in _db.Cadays
                                                                join mongiasu in _db.Mongiasus on caday.IdmonDay equals mongiasu.IdmonGiaSu
                                                                join cahoc in _db.Cahocs on caday.IdloaiCaDay equals cahoc.IdCaHoc

                                                                join user in _db.Taikhoannguoidungs on caday.IdnguoiDay equals user.Id into userGroup
                                                                from user in userGroup.DefaultIfEmpty()

                                                                join danhGiaGiaSu in _db.Danhgiagiasus on caday.Id equals danhGiaGiaSu.IdCaDay into nhomGiaSu
                                                                from danhGiaGiaSu in nhomGiaSu.DefaultIfEmpty()

                                                                join danhGiaNguoiHoc in _db.Danhgianguoihocs on caday.Id equals danhGiaNguoiHoc.IdCaDay into nhomNguoiHoc
                                                                from danhGiaNguoiHoc in nhomNguoiHoc.DefaultIfEmpty()

                                                                where caday.IdnguoiHoc == userId
                                                                orderby caday.Id descending

                                                                let thoiGianBatDau = $"{caday.GioBatDau}:{(caday.PhutBatDau > 0 ? caday.PhutBatDau.ToString(zeroDefault) : zeroDefault)}"
                                                                let thoiGianKetThuc = $"{caday.GioKetThuc}:{(caday.PhutKetThuc > 0 ? caday.PhutKetThuc.ToString(zeroDefault) : zeroDefault)}"

                                                                select new GetAllHistoryOfLearningResponse()
                                                                {
                                                                    IdCaDay = caday.Id,
                                                                    TenMonGiaSu = mongiasu.TenMonGiaSu,
                                                                    IdGiaSu = user.Id,
                                                                    TenGiaSu = user.HoTen,
                                                                    NgayDay = caday.NgayDay.Date,
                                                                    KhoangThoiGian = $"{thoiGianBatDau} h : {thoiGianKetThuc} h",
                                                                    GiaTien = cahoc.GiaTien > 0 ? cahoc.GiaTien.ToString("#,##0.##") : "0",
                                                                    TrangThai = caday.TrangThai,
                                                                    TenTrangThai = (caday.TrangThai == true) ? "Hoàn thành" : ((caday.TrangThai == false) ? "Đã hủy" : "Không ai đăng ký"), //TODO: not Rule clear code
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

                query = query.Where(x => x.TenMonGiaSu.ToLower().Contains(request.Search) || x.TenGiaSu.ToLower().Contains(request.Search) || x.IdCaDay.ToString().Contains(request.Search));
            }

            query = query?.Skip((request.PageIndex - 1) * request.PageSize) // Bỏ qua số lượng sản phẩm của các trang trước                                                                  
                ?.Take(request.PageSize); // Lấy số lượng sản phẩm của trang hiện tại

            return await query.ToListAsync();
        }

        /// <summary>
        /// Xem thông tin chi tiết gia sư đánh giá người học.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetLearnerEvaluationReponse</returns>
        public async Task<GetLearnerEvaluationReponse> GetOneLearnerEvaluation(GetOneLearnerEvaluationRequest request)
        {
            int? userId = await IsExistUser();
            if (userId is null)
            {
                return null;
            }

            GetLearnerEvaluationReponse result = await (from danhGiaNguoiHoc in _db.Danhgianguoihocs
                                                      join caday in _db.Cadays on danhGiaNguoiHoc.IdCaDay equals caday.Id
                                                      join mongiasu in _db.Mongiasus on caday.IdmonDay equals mongiasu.IdmonGiaSu

                                                      join giaSu in _db.Taikhoannguoidungs on caday.IdnguoiDay equals giaSu.Id into nhomGiaSu
                                                      from giaSu in nhomGiaSu.DefaultIfEmpty()

                                                      where danhGiaNguoiHoc.IdCaDay == request.IdCaDay && caday.IdnguoiHoc == userId
                                                      select new GetLearnerEvaluationReponse
                                                      {
                                                          NgayDay = caday.NgayDay,
                                                          Diem = danhGiaNguoiHoc.Diem,
                                                          DanhGia = danhGiaNguoiHoc.DanhGia,
                                                          NgayTao = danhGiaNguoiHoc.NgayTao,
                                                          TenNguoiDay = giaSu.HoTen,
                                                          IdNguoiDay = giaSu.Id,
                                                          IdNguoiHoc = caday.IdnguoiHoc,
                                                          TenMonGiaSu = mongiasu.TenMonGiaSu,
                                                          IdCaDay = caday.Id,
                                                          IdDanhGiaNguoiHoc = danhGiaNguoiHoc.Id,
                                                      }).FirstOrDefaultAsync();

            if (result is not null )
            {
                Danhgianguoihoc danhGiaNguoiHoc = await _db.Danhgianguoihocs.FirstOrDefaultAsync(x => x.Id == result.IdDanhGiaNguoiHoc);
                var tieuChiNguoiDays = danhGiaNguoiHoc.TieuChi?.Split(";").Select(tieuChiId => int.TryParse(tieuChiId, out int output) ? output : 0);

                if (tieuChiNguoiDays is not null)
                {
                    result.CriteriaEvaluations = await _db.Tieuchidanhgias.Where(x => tieuChiNguoiDays.Contains(x.IdTieuChi))?
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

            if (userId is 0)
            {
                return null;
            }

            Taikhoannguoidung result = await _db.Taikhoannguoidungs.FirstOrDefaultAsync(x => x.Id == userId);

            return result?.Id;
        }

        #endregion 
    }
}
