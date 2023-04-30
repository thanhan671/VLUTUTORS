using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using VLUTUTORS.Support.Services;

namespace VLUTUTORS.Areas.Tutors.Controllers
{
    [Area("Tutors")]
    public class ManageWalletController : Controller
    {
        private CP25Team01Context _db = new CP25Team01Context();

        public async Task<IActionResult> Index()
        {
            //string user = HttpContext.Session.GetString("LoginId");
            //if (user == null)
            //{
            //    return RedirectToAction("Login", "Accounts", new { area = "default" });
            //}
            List<MoneyServiceHistory> moneyServiceHistories = SumaryDepositAndWithdrawal();

            return View(moneyServiceHistories);
        }

        [HttpPost]
        public IActionResult Deposit(int depositMoney)
        {
            var userId = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));

            Naptien deposit = new Naptien();

            deposit.IdNguoiNap = (int)userId.Id;
            deposit.MaNapTien = MoneyServices.AutoGenCodeWhenDeposit();
            deposit.SoTienNap = depositMoney;
            deposit.TrangThai = false;

            _db.Add(deposit);
            _db.SaveChangesAsync();

            return View();
        }

        [HttpPost]
        public IActionResult Withdrawal(int withdrawalMoney)
        {
            var userId = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
            Taikhoannguoidung taikhoannguoidung = _db.Taikhoannguoidungs.Find(userId.Id);
            int balanceMoney = (int) taikhoannguoidung.SoDuVi;

            if(balanceMoney < withdrawalMoney)
            {
                return RedirectToAction("Index", "ManageWallet");
            }

            Ruttien withdrawal = new Ruttien();

            withdrawal.IdNguoiRut = (int) userId.Id;
            withdrawal.MaRutTien = MoneyServices.AutoGenCodeWhenDeposit();
            withdrawal.SoTienRut = withdrawalMoney;
            withdrawal.TrangThai = false;

            _db.Add(withdrawal);
            _db.SaveChangesAsync();

            return RedirectToAction("Index", "ManageWallet");
        }

        private List<MoneyServiceHistory> SumaryDepositAndWithdrawal()
        {
            var user = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));

            List<Naptien> depositList = _db.Naptiens.Where(w => w.IdNguoiNap.Equals(user.Id)).ToList();
            List<Ruttien> withdrawalList = _db.Ruttiens.Where(w => w.IdNguoiRut.Equals(user.Id)).ToList();

            List<MoneyServiceHistory> moneyServiceHistories = new List<MoneyServiceHistory>();
            foreach(var item in depositList)
            {
                MoneyServiceHistory moneyServiceHistory = new MoneyServiceHistory
                {
                    id = item.Id,
                    serviceName = "Nap Tien",
                    serviceCode = item.MaNapTien,
                    // some code for date time
                    money = item.SoTienNap
                };

                moneyServiceHistories.Add(moneyServiceHistory);
            }

            foreach (var item in withdrawalList)
            {
                MoneyServiceHistory moneyServiceHistory = new MoneyServiceHistory
                {
                    id = item.Id,
                    serviceName = "Rut Tien",
                    serviceCode = item.MaRutTien,
                    // some code for date time
                    money = item.SoTienRut
                };

                moneyServiceHistories.Add(moneyServiceHistory);
            }

            return moneyServiceHistories;
        }
        
    }

    public class MoneyServiceHistory
    {
        public int id { get; set; }
        public string serviceName { get; set; }
        public string serviceCode { get; set; }
        public string dateTime { get; set; }
        public int money { get; set; }
    }
}
