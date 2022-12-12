using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageAccounts : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddNewAccounts()
        {
            return View();
        }
        public IActionResult EditAccounts()
        {
            return View();
        }
    }
}
