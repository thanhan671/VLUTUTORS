using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Register(string HoTen, string Email, string MatKhau)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var taiKhoan = _context.Taikhoannguoidungs.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());

                    if (taiKhoan != null)
                    {
                        return RedirectToAction("Login", "Accounts");
                    }
                    else
                    {
                        Taikhoannguoidung taiKhoanNguoiDung = new Taikhoannguoidung
                        {
                            HoTen = HoTen,
                            Email = Email,
                            MatKhau = MatKhau
                        };
                        try
                        {
                            _context.Add(taiKhoanNguoiDung);
                            await _context.SaveChangesAsync();
                        }
                        catch
                        {
                            return RedirectToAction("Login", "Accounts");
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Accounts");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Accounts");

            }
            return RedirectToAction("Login", "Accounts");
        }
    }
}