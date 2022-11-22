using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;

namespace VLUTUTORS.Controllers
{
    public class AccountsController : Controller
    {

        private readonly CP25Team01Context _context;

        public AccountsController(CP25Team01Context context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string HoTen, string Email, string MatKhau)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Taikhoannguoidung taikhoannguoidung = new Taikhoannguoidung
                    {
                        HoTen = HoTen,
                        Email = Email,
                        MatKhau = MatKhau
                    };
                    try
                    {
                        _context.Add(taikhoannguoidung);
                        await _context.SaveChangesAsync();
                    }
                    catch
                    {
                        return RedirectToAction("Login", "Accounts");
                    }
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();

            }
            return View();
        }
    }
}
