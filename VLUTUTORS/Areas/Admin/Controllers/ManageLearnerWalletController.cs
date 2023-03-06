using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    public class ManageLearnerWalletController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
