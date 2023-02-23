using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;

namespace VLUTUTORS.Controllers
{
    public class BookTutorController : Controller
    {
        private CP25Team01Context _db = new CP25Team01Context();
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> DetailTutor()
        {
            return View();
        }

        public async Task<IActionResult> HistoryBooking()
        {
            return View();
        }
    }
}
