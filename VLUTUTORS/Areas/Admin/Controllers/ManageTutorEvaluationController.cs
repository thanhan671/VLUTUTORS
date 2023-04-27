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

        [HttpGet]
        public IActionResult Index()
        {
            List<Danhgiagiasu> danhGias = _db.Danhgiagiasus.ToList();

            foreach (var danhgiaItem in danhGias)
            {
                danhgiaItem.tenNguoiDay = _db.Taikhoannguoidungs.Find(danhgiaItem.GiasuId).HoTen.ToString();
                danhgiaItem.tenNguoiHoc = _db.Taikhoannguoidungs.Find(danhgiaItem.NguoidungId).HoTen.ToString();
            }
            return View(danhGias);
        }
    }
}
