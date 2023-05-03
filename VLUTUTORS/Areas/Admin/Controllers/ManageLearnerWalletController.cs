using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using VLUTUTORS.Support.Services;
using ZoomNet.Resources;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "3")]
    public class ManageLearnerWalletController : Controller
    {
        private CP25Team01Context _db = new CP25Team01Context();
        public IActionResult ManageDeposit()
        {
            List<Naptien> depositPending = _db.Naptiens.Where(m => m.TrangThai == false).ToList();
            foreach (var item in depositPending)
            {
                item.taiKhoan = _db.Taikhoannguoidungs.Find(item.IdNguoiNap).IdxetDuyet.ToString();
                item.tenNguoiNap = _db.Taikhoannguoidungs.Find(item.IdNguoiNap).HoTen.ToString();
                item.trangThai = item.TrangThai ? "Đã xử lý" : "Đang chờ duyệt";
            }
            List<Naptien> depositDone = _db.Naptiens.Where(m => m.TrangThai == true).ToList();
            foreach (var item in depositDone)
            {
                item.taiKhoan = _db.Taikhoannguoidungs.Find(item.IdNguoiNap).IdxetDuyet.ToString();
                item.tenNguoiNap = _db.Taikhoannguoidungs.Find(item.IdNguoiNap).HoTen.ToString();
                item.trangThai = item.TrangThai ? "Đã xử lý" : "Đang chờ duyệt";
            }
            Tuple<IEnumerable<Naptien>, IEnumerable<Naptien>> turple = new Tuple<IEnumerable<Naptien>, IEnumerable<Naptien>>(depositPending, depositDone);

            return View(turple);
        }
        public IActionResult ManageWithdrawal()
        {
            List<Ruttien> withDrawalPending = _db.Ruttiens.Where(m => m.TrangThai == false).ToList();
            foreach (var item in withDrawalPending)
            {
                item.taiKhoan = _db.Taikhoannguoidungs.Find(item.IdNguoiRut).IdxetDuyet.ToString();
                item.tenNguoiRut = _db.Taikhoannguoidungs.Find(item.IdNguoiRut).HoTen.ToString();
                item.trangThai = item.TrangThai ? "Đã xử lý" : "Đang chờ duyệt";
            }
            List<Ruttien> withDrawalDone = _db.Ruttiens.Where(m => m.TrangThai == true).ToList();
            foreach (var item in withDrawalDone)
            {
                item.taiKhoan = _db.Taikhoannguoidungs.Find(item.IdNguoiRut).IdxetDuyet.ToString();
                item.tenNguoiRut = _db.Taikhoannguoidungs.Find(item.IdNguoiRut).HoTen.ToString();
                item.trangThai = item.TrangThai ? "Đã xử lý" : "Đang chờ duyệt";
            }
            Tuple<IEnumerable<Ruttien>, IEnumerable<Ruttien>> turple = new Tuple<IEnumerable<Ruttien>, IEnumerable<Ruttien>>(withDrawalPending, withDrawalDone);

            return View(turple);
        }
        public IActionResult ConfirmLearnerDeposit([Bind(include: "Id,MaNapTien,IdNguoiNap,SoTienNap,TrangThai,ThoiGianNapTien")] Naptien napTien)
        {
            int money = napTien.SoTienNap;
            int userId = napTien.IdNguoiNap;

            napTien.TrangThai = true;
            napTien.ThoiGianNapTien = DateTime.Now;

            MoneyServices.AddMoney(money, userId, _db);

            _db.Naptiens.Update(napTien);
            _db.SaveChangesAsync();

            TempData["Message"] = "Xác nhận nạp tiền thành công!";
            TempData["MessageType"] = "success";
            return RedirectToAction("ManageDeposit", "ManageLearnerWallet");
        }
        public IActionResult ConfirmLearnerWithdrawal([Bind(include: "Id,MaRutTien,IdNguoiRut,SoTienRut,TrangThai,ThoiGianRutTien")] Ruttien rutTien)
        {
            rutTien.TrangThai = true;
            rutTien.ThoiGianRutTien = DateTime.Now;

            _db.Ruttiens.Update(rutTien);
            _db.SaveChangesAsync();

            TempData["Message"] = "Xác nhận rút tiền thành công!";
            TempData["MessageType"] = "success";
            return RedirectToAction("ManageWithdrawal", "ManageLearnerWallet");
        }
    }
}
