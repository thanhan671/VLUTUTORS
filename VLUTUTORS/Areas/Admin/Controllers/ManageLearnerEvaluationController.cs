using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Areas.Admin.Requests.ManageLearnerEvaluations;
using VLUTUTORS.Areas.Admin.Responses.ManageLearnerEvaluations;
using VLUTUTORS.Models;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Quản trị viên người học")]
    public class ManageLearnerEvaluationController : Controller
    {
        private readonly CP25Team01Context _db = new();

        #region View 

        public async Task<IActionResult> Index(GetAllLearnerEvaluationRequest request)
        {
            IReadOnlyCollection<GetAllLearnerEvaluationResponse> result = await GetAllLearnerEvaluation(request);
            return View(result);
        }

        public async Task<IActionResult> DetailEvaluation(int idNguoiHoc, GetAllDetailLearnerEvaluationRequest request)
        {
            IReadOnlyCollection<GetAllDetailLearnerEvaluationResponse> result = await GetAllDetailLearnerEvaluationByIdTutor(idNguoiHoc, request);
            return View(result);
        }

        #endregion

        #region  Method

        /// <summary>
        /// Xem danh sách đánh giá người học của gia sư.
        /// </summary>
        /// <param name="request">GetAllLearnerEvaluationRequest</param>
        /// <returns>IReadOnlyCollection -> GetAllLearnerEvaluationResponse </returns>
        private async Task<IReadOnlyCollection<GetAllLearnerEvaluationResponse>> GetAllLearnerEvaluation(GetAllLearnerEvaluationRequest request)
        {
            const int KHONG_XET_DUYET = 5;
            List<GetAllLearnerEvaluationResponse> result = new();

            List<Taikhoannguoidung> nguoiHocs = await _db.Taikhoannguoidungs.Where(x => x.IdxetDuyet == KHONG_XET_DUYET)?.Skip((request.PageIndex - 1) * request.PageSize)?.Take(request.PageSize)?.ToListAsync();

            var query = from danhGiaNguoiHoc in await _db.Danhgianguoihocs.ToListAsync()
                        join caDay in await _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay.Date <= DateTime.Now.Date).ToListAsync()
                        on danhGiaNguoiHoc.IdCaDay equals caDay.Id

                        join nguoiHoc in nguoiHocs on caDay.IdnguoiHoc equals nguoiHoc.Id

                        select new
                        {
                            DanhGia = danhGiaNguoiHoc.Diem,
                            IdNguoiHoc = caDay.IdnguoiHoc,
                            TenNguoiHoc = nguoiHoc.HoTen
                        };

            if (!string.IsNullOrEmpty(request.Search))
            {
                request.Search = request.Search?.Trim().ToLower();
                query = query.Where(x => x.TenNguoiHoc.ToLower().Contains(request.Search));
            }

            if (query is null)
            {
                return result;
            }

            foreach (var nguoiHoc in nguoiHocs)
            {
                var soLuongCaDay = query.Where(x => x.IdNguoiHoc == nguoiHoc.Id).ToList();

                if (soLuongCaDay.Count > 0)
                {
                    double tongDanhGia = soLuongCaDay.Average(x => x.DanhGia);
                    GetAllLearnerEvaluationResponse model = new()
                    {
                        IdNguoiHoc = nguoiHoc.Id,
                        TenNguoiHoc = nguoiHoc.HoTen,
                        TongDiem = tongDanhGia
                    };
                    result.Add(model);
                }
            }
            return result;
        }

        /// <summary>
        /// Xem danh sách đánh giá người học của gia sư.
        /// </summary>
        /// <param name="idNguoiHoc">Int</param>
        /// <param name="request">GetAllDetailLearnerEvaluationRequest</param>
        /// <returns>IReadOnlyCollection -> GetAllDetailLearnerEvaluationResponse</returns>
        private async Task<IReadOnlyCollection<GetAllDetailLearnerEvaluationResponse>> GetAllDetailLearnerEvaluationByIdTutor(int idNguoiHoc, GetAllDetailLearnerEvaluationRequest request)
        {
            const int DA_XET_DUYET = 5;

            List<GetAllDetailLearnerEvaluationResponse> result = new();

            if (idNguoiHoc is 0)
            {
                return result;
            }

            var query = from caDay in _db.Cadays.Where(x => x.TrangThai == true && x.NgayDay.Date <= DateTime.Now.Date)
                        join giaSu in _db.Taikhoannguoidungs.Where(x => x.IdxetDuyet == DA_XET_DUYET)
                        on caDay.IdnguoiDay equals giaSu.Id
                        join danhGiaNguoiHoc in _db.Danhgianguoihocs
                        on caDay.Id equals danhGiaNguoiHoc.IdCaDay
                        where caDay.IdnguoiHoc == idNguoiHoc

                        orderby danhGiaNguoiHoc.Id descending

                        select new GetAllDetailLearnerEvaluationResponse()
                        {
                            IdCaDay = caDay.Id,
                            IdGiaSu = giaSu.Id,
                            TenGiaSu = giaSu.HoTen,
                            Diem = danhGiaNguoiHoc.Diem,
                            DanhGia = danhGiaNguoiHoc.DanhGia,
                            TieuChi = danhGiaNguoiHoc.TieuChi,
                        };

            if (!string.IsNullOrEmpty(request.Search))
            {
                request.Search = request.Search?.Trim().ToLower();

                query = query.Where(x => x.TenGiaSu.ToLower().Contains(request.Search));
            }

            result = await query?.Skip((request.PageIndex - 1) * request.PageSize)?.Take(request.PageSize)?.ToListAsync();

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
