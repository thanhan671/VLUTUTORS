using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageTutorSubjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddSubject()
        {
            return View();
        }
        public IActionResult EditSubject()
        {
            return View();
        }
    }
}
