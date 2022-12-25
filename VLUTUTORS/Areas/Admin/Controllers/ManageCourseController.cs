using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Quản trị viên hệ thống")]

    public class ManageCourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DetailCourse()
        {
            return View();
        }
        public IActionResult AddCourse()
        {
            return View();
        }
        public IActionResult EditCourse()
        {
            return View();
        }
        public IActionResult AddLesson()
        {
            return View();
        }
        public IActionResult EditLesson()
        {
            return View();
        }
    }
}
