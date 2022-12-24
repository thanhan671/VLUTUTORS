using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Quản trị viên hệ thống")]

    public class ManageContentController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();

        
        public async Task<IActionResult> Index()
        {
            var noiDung = await _context.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);
            return View(noiDung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index( string GioiThieuChanTrang, string DiaChi, string Sdt, string Email, string Facebook, string GioiThieu, string Slogan)
        {
            var sqlStringBuilder = new SqlConnectionStringBuilder();
            sqlStringBuilder["Server"] = "tuleap.vanlanguni.edu.vn,18082";
            sqlStringBuilder["Database"] = "CP25Team01";
            sqlStringBuilder["UID"] = "CP25Team01";
            sqlStringBuilder["PWD"] = "Cap25t01";

            var sqlStringConnection = sqlStringBuilder.ToString();

            using var connection = new SqlConnection(sqlStringConnection);

            connection.Open();

            using var command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "UPDATE NOIDUNG SET GioiThieuChanTrang = @GioiThieuChanTrang, DiaChi=@DiaChi,Sdt=@Sdt,Email=@Email,Facebook=@Facebook,GioiThieu=@GioiThieu,Slogan=@Slogan WHERE ID=1";

            command.Parameters.AddWithValue("@GioiThieuChanTrang", GioiThieuChanTrang);
            command.Parameters.AddWithValue("@DiaChi", DiaChi);
            command.Parameters.AddWithValue("@Sdt", Sdt);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Facebook", Facebook);
            command.Parameters.AddWithValue("@GioiThieu", GioiThieu);
            command.Parameters.AddWithValue("@Slogan", Slogan);

            command.ExecuteNonQuery();

            connection.Close();

            return RedirectToAction("Index");
        }
    }
}
