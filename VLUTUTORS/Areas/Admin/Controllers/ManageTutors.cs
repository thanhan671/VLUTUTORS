﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageTutors : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewRegister()
        {
            return View();
        }

        public IActionResult DetailTutor()
        {
            return View();
        }

        public IActionResult UpdateTutor()
        {
            return View();
        }
    }
}
