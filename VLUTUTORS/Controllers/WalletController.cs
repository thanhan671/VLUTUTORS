﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using VLUTUTORS.Models;
using VLUTUTORS.Support.Services;
using System.Linq;

namespace VLUTUTORS.Controllers
{
    public class WalletController : Controller
    {
        private CP25Team01Context _db = new CP25Team01Context();

        public IActionResult Index()
        {
            string user = HttpContext.Session.GetString("LoginId");
            if (user == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            Phiday phiday = _db.Phidays.Where(ph => ph.Id == 1).FirstOrDefault();

            TempData["STK"] = phiday.SoTaiKhoan.ToString();
            TempData["NH"] = phiday.NganHang.ToString();
            TempData["NguoiNhan"] = phiday.NguoiNhan.ToString();

            var userId = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
            Taikhoannguoidung taikhoannguoidung = _db.Taikhoannguoidungs.Find(userId.Id);

            TempData["SoDu"] = taikhoannguoidung.SoDuVi.Value.ToString("#,##0.###");

            List <MoneyServiceHistory> moneyServiceHistories = SumaryDepositAndWithdrawal();
            Tuple<Taikhoannguoidung, IEnumerable<MoneyServiceHistory>> turple = new Tuple<Taikhoannguoidung, IEnumerable<MoneyServiceHistory>>(taikhoannguoidung, moneyServiceHistories);
            return View(turple);
        }

        [HttpPost]
        public IActionResult Deposit(int depositMoney)
        {
            var userId = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));

            if(depositMoney <= 0 || depositMoney>5000000)
            {
                TempData["Message"] = "Hãy nhập một số tiền lớn hơn 0 và nhỏ hơn 5.000.000 !";
                TempData["MessageType"] = "error";

                return RedirectToAction("Index", "Wallet");
            }
            Naptien deposit = new Naptien();

            deposit.IdNguoiNap = (int)userId.Id;
            deposit.MaNapTien = MoneyServices.AutoGenCodeWhenDeposit();
            deposit.SoTienNap = depositMoney;
            deposit.TrangThai = false;
            deposit.ThoiGianNapTien = DateTime.Now;

            _db.Add(deposit);
            _db.SaveChangesAsync();

            TempData["Message"] = "Yêu cầu gửi thành công, vui lòng đợi xét duyệt!";
            TempData["MessageType"] = "success";

            return RedirectToAction("Index", "Wallet");
        }

        [HttpPost]
        public IActionResult Withdrawal(int withdrawalMoney)
        {
            if (withdrawalMoney <= 0 || withdrawalMoney>5000000)
            {
                TempData["Message"] = "Hãy nhập một số tiền lớn hơn 0 và nhỏ hơn 5.000.000 !";
                TempData["MessageType"] = "error";

                return RedirectToAction("Index", "Wallet");
            }
            var userId = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
            Taikhoannguoidung taikhoannguoidung = _db.Taikhoannguoidungs.Find(userId.Id);
            int balanceMoney = (int)taikhoannguoidung.SoDuVi;

            if (balanceMoney < withdrawalMoney)
            {
                TempData["Message"] = "Số dư không đủ, vui lòng thử lại!";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index", "Wallet");
            }

            Ruttien withdrawal = new Ruttien();

            withdrawal.IdNguoiRut = (int)userId.Id;
            withdrawal.MaRutTien = MoneyServices.AutoGenCodeWhenDeposit();
            withdrawal.SoTienRut = withdrawalMoney;
            withdrawal.TrangThai = false;
            withdrawal.ThoiGianRutTien = DateTime.Now;

            MoneyServices.SubtractMoney(withdrawalMoney, userId.Id, _db);

            _db.Add(withdrawal);
            _db.SaveChangesAsync();

            TempData["Message"] = "Yêu cầu gửi thành công, vui lòng đợi xét duyệt!";
            TempData["MessageType"] = "success";

            return RedirectToAction("Index", "Wallet");
        }

        private List<MoneyServiceHistory> SumaryDepositAndWithdrawal()
        {
            var user = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));

            List<Naptien> depositList = _db.Naptiens.Where(w => w.IdNguoiNap.Equals(user.Id)).ToList();
            List<Ruttien> withdrawalList = _db.Ruttiens.Where(w => w.IdNguoiRut.Equals(user.Id)).ToList();

            List<MoneyServiceHistory> moneyServiceHistories = new List<MoneyServiceHistory>();
            foreach (var item in depositList)
            {
                MoneyServiceHistory moneyServiceHistory = new MoneyServiceHistory
                {
                    id = item.Id,
                    serviceName = "Nạp Tiền",
                    serviceCode = item.MaNapTien,
                    dateTime = item.ThoiGianNapTien.ToString("dd/MM/yyyy"),
                    money = item.SoTienNap,
                    status = item.TrangThai ? "Đã xử lý" : "Đang chờ duyệt"
                };

                moneyServiceHistories.Add(moneyServiceHistory);
            }

            foreach (var item in withdrawalList)
            {
                MoneyServiceHistory moneyServiceHistory = new MoneyServiceHistory
                {
                    id = item.Id,
                    serviceName = "Rút Tiền",
                    serviceCode = item.MaRutTien,
                    dateTime = item.ThoiGianRutTien.ToString("dd/MM/yyyy"),
                    money = item.SoTienRut,
                    status = item.TrangThai ? "Đã xử lý" : "Đang chờ duyệt"
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
        public string status { get; set; }
    }
}
