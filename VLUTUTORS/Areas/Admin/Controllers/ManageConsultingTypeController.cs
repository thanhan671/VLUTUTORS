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
    public class ManageConsultingTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddType()
        {
            return View();
        }
        public IActionResult EditType()
        {
            return View();
        }
    }
}
