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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string HoTen, string Email, string MatKhau)
        {
            string conString = @"Server=tuleap.vanlanguni.edu.vn,18082;Database=CP25Team01;User Id=CP25Team01; Password=Cap25t01;Integrated Security=True;Trusted_Connection=False;ApplicationIntent=ReadWrite;MultipleActiveResultSets=False";
            SqlConnection con = new SqlConnection(conString);
            try
            {
                string query = "insert into Taikhoannguoidung(HoTen,Email,MatKhau) values('" + HoTen + "', '" + Email + "','" + MatKhau + "')";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                con.Open();
                da.SelectCommand.ExecuteNonQuery();
                con.Close();
            }
            catch
            {
                con.Close();
            }
            return View();
        }
    }
}
