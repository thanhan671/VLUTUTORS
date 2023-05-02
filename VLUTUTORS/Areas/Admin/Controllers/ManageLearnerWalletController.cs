using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using ZoomNet.Resources;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Quản trị viên người học")]
    public class ManageLearnerWalletController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public IActionResult Index()
        {
            var listLearner = _context.Taikhoannguoidungs.Where(m=>m.IdxetDuyet!=5).ToList();
            return View();
        }
    }
}
