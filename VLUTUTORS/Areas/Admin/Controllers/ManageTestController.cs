using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DetailTest()
        {
            return View();
        }
        public IActionResult AddTest()
        {
            return View();
        }
        public IActionResult EditTest()
        {
            return View();
        }
        public IActionResult AddQuestion()
        {
            return View();
        }
        public IActionResult EditQuestion()
        {
            return View();
        }
    }
}
