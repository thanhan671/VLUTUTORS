using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Quản trị viên hệ thống")]
    public class ManageEvaluationCriteriaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddCriteria()
        {
            return View();
        }
        public IActionResult EditCriteria()
        {
            return View();
        }
    }
}
