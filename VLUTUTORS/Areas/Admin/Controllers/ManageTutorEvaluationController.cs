using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Areas.Admin.Requests.ManageTutorEvaluations;
using VLUTUTORS.Areas.Admin.Responses.ManageTutorEvaluations;
using VLUTUTORS.Models;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Quản trị viên gia sư")]
    public class ManageTutorEvaluationController : Controller
    {
        private readonly CP25Team01Context _db = new();

        #region View 

        [HttpGet]
        public async Task<IActionResult> Index(GetAllTutorEvaluationRequest request)
        {
            IReadOnlyCollection<GetAllTutorEvaluationResponse> result = await GetAllTutorEvaluation(request);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> DetailEvaluation(int idGiaSu, GetAllDetailTutorEvaluationRequest request)
        {
            IReadOnlyCollection<GetAllDetailTutorEvaluationResponse> result = await GetAllDetailTutorEvaluationByIdTutor(idGiaSu,request);
            return View(result);
        }

        #endregion 

        #region Method 

        /// <summary>
        /// Xem danh sách đánh giá gia sư của người học.
        /// </summary>
        /// <param name="request">GetAllTutorEvaluationRequest</param>
        /// <returns>IReadOnlyCollection -> GetAllTutorEvaluationResponse </returns>
        private async Task<IReadOnlyCollection<GetAllTutorEvaluationResponse>> GetAllTutorEvaluation(GetAllTutorEvaluationRequest request)
        {
            const int DA_XET_DUYET = 5;
            List<GetAllTutorEvaluationResponse> result = new();

            List<Taikhoannguoidung> giaSus = await _db.Taikhoannguoidungs.Where(x => x.IdxetDuyet == DA_XET_DUYET)?.Skip((request.PageNumber - 1) * request.PageSize)?.Take(request.PageSize)?.ToListAsync();

            var query = from danhGiaGiaSu in await _db.Danhgiagiasus.ToListAsync()
                        join caDay in await _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay.Date <= DateTime.Now.Date).ToListAsync()
                        on danhGiaGiaSu.IdCaDay equals caDay.Id
                        join giaSu in giaSus on caDay.IdnguoiDay equals giaSu.Id
                        select new
                        {
                            DanhGia = danhGiaGiaSu.Diem,
                            IdGiaSu = caDay.IdnguoiDay,
                            TenGiaSu = giaSu.HoTen
                        };

            if (!string.IsNullOrEmpty(request.Search))
            {
                request.Search = request.Search?.Trim().ToLower();
                query = query.Where(x => x.TenGiaSu.ToLower().Contains(request.Search));
            }

            if (query is null)
            {
                return result;
            }

            foreach (var giaSu in giaSus)
            {
                var soLuongCaDay = query.Where(x => x.IdGiaSu == giaSu.Id).ToList();

                if (soLuongCaDay.Count > 0)
                {
                    double tongDanhGia = soLuongCaDay.Average(x => x.DanhGia);
                    GetAllTutorEvaluationResponse model = new()
                    {
                        IdGiaSu = giaSu.Id,
                        TenGiaSu = giaSu.HoTen,
                        TongDiem = tongDanhGia
                    };
                    result.Add(model);
                }
            }
            return result;
        }

        /// <summary>
        /// Xem danh sách đánh giá gia sư của người học.
        /// </summary>
        /// <param name="idGiaSu">Int</param>
        /// <param name="request">GetAllDetailTutorEvaluationRequest</param>
        /// <returns>IReadOnlyCollection -> GetAllDetailTutorEvaluationResponse</returns>
        private async Task<IReadOnlyCollection<GetAllDetailTutorEvaluationResponse>> GetAllDetailTutorEvaluationByIdTutor(int idGiaSu, GetAllDetailTutorEvaluationRequest request)
        {
            const int KHONG_XET_DUYET = 6;

            List<GetAllDetailTutorEvaluationResponse> result = new();

            if (idGiaSu is 0)
            {
                return result;
            }

            var query = from caDay in _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay.Date <= DateTime.Now.Date)
                        join nguoiHoc in _db.Taikhoannguoidungs.Where(x => x.IdxetDuyet == KHONG_XET_DUYET)
                        on caDay.IdnguoiHoc equals nguoiHoc.Id
                        join danhGiaGiaSu in _db.Danhgiagiasus
                        on caDay.Id equals danhGiaGiaSu.IdCaDay
                        where caDay.IdnguoiDay == idGiaSu

                        orderby danhGiaGiaSu.Id descending

                        select new GetAllDetailTutorEvaluationResponse()
                        {
                            IdCaDay = caDay.Id,
                            IdHocVien = nguoiHoc.Id,
                            TenHocVien = nguoiHoc.HoTen,
                            Diem = danhGiaGiaSu.Diem,
                            DanhGia = danhGiaGiaSu.DanhGia,
                            TieuChi = danhGiaGiaSu.TieuChi,
                        };

            if (!string.IsNullOrEmpty(request.Search))
            {
                request.Search = request.Search?.Trim().ToLower();

                query = query.Where(x => x.TenHocVien.ToLower().Contains(request.Search));
            }

            result = await query?.Skip((request.PageNumber - 1) * request.PageSize)?.Take(request.PageSize)?.ToListAsync();

            if (result is not null)
            {
                var tieuChis = await _db.Tieuchidanhgias.ToListAsync();

                result.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.TieuChi))
                    {
                        var tieuChiDanhGia = x.TieuChi?.Split(";").Select(tieuChiId => int.TryParse(tieuChiId, out int output) ? output : 0);
                        if (tieuChiDanhGia is not null)
                        {
                            var noiDungTieuChi = tieuChis.Where(x => tieuChiDanhGia.Contains(x.IdTieuChi)).Select(x => x.TieuChi).ToList();
                            x.TieuChi = string.Join(", ", noiDungTieuChi);
                        }
                    }
                });
            }

            return result;
        }

        #endregion
    }
}
