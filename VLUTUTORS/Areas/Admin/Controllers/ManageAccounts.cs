using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VLUTUTORS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageAccounts : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddNewAccounts()
        {
            Taikhoanadmin taikhoanadmin = new Taikhoanadmin();

            taikhoanadmin.Quyens = new SelectList(_context.Quyens, "IdQuyen", "TenQuyen", taikhoanadmin.IdQuyen);

            return View(taikhoanadmin);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewAccounts([Bind(include:"Id,TaiKhoan,MatKhau,IdQuyen")] Taikhoanadmin taikhoanadmin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(taikhoanadmin);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Index");
            }
            return View(taikhoanadmin);
        }
        public IActionResult EditAccounts()
        {
            return View();
        }
    }
}
