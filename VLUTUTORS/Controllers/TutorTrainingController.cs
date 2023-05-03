using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;

namespace VLUTUTORS.Controllers
{
    public class TutorTrainingController : Controller
    {
        private CP25Team01Context _db = new CP25Team01Context();
        private readonly ILogger<TutorTrainingController> _logger;
        private IHostingEnvironment _environment;

        public TutorTrainingController(ILogger<TutorTrainingController> logger, IHostingEnvironment environment)
        {
            _logger = logger;
            this._environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string courseName, bool justDoTest)
        {
            if (HttpContext.Session.GetString("LoginId") == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            int checkUser = (int)_db.Taikhoannguoidungs.Where(m => m.Id == HttpContext.Session.GetInt32("LoginId")).First().IdxetDuyet;
            if (checkUser == 1 || checkUser == 2 || checkUser == 3)
            {
                List<Khoadaotao> lesson = _db.Khoadaotaos.ToList();

                Khoadaotao baihoc = new Khoadaotao();
                baihoc.courses = (from l in lesson
                                  select l.TenBaiHoc).ToList();

                courseName = courseName == null ? _db.Khoadaotaos.First<Khoadaotao>().TenBaiHoc : courseName; // use this instead of above code line
                Console.WriteLine("ten bai hoc la gi: " + courseName);

                string videoListInJson = _db.Khoadaotaos.Where(l => l.TenBaiHoc == courseName).First().LinkVideo;

                if (videoListInJson != null)
                {
                    baihoc.courseLink = JsonConvert.DeserializeObject<List<string>>(videoListInJson);
                }
                else
                {
                    baihoc.courseLink = null;
                }

                baihoc.TaiLieu = _db.Khoadaotaos.Where(l => l.TenBaiHoc == courseName).First().TaiLieu;
                baihoc.IdBaiHoc = _db.Khoadaotaos.Where(l => l.TenBaiHoc == courseName).First().IdBaiHoc;

                var userInfo = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
                Taikhoannguoidung taikhoannguoidung = _db.Taikhoannguoidungs.Find(userInfo.Id);

                baihoc.currentScore = taikhoannguoidung.DiemBaiTest;
                Console.WriteLine("moi lam bai kiem tra index: " + justDoTest);
                if (baihoc.currentScore <= 7 && justDoTest)
                {
                    TempData["message"] = "Điểm của bạn: " + baihoc.currentScore + " chưa đủ để xét duyệt vui lòng làm lại bài kiểm tra";
                }

                return View(baihoc);
            }
            return RedirectToAction("Index", "Home");
        }

        public FileResult DownloadFile(string courseName, string fileName)
        {
            //string courseFilePath = id == 1 ? Path.Combine("certificates", tutorId.ToString(), "cer1") : Path.Combine("certificates", tutorId.ToString(), "cer2");

            string path = Path.Combine(this._environment.WebRootPath, "Files", courseName, fileName);

            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", fileName);
        }

        [HttpPost]
        public IActionResult Index(string courseName, int id = 1)
        {
            Console.WriteLine("lesson: " + courseName);
            return RedirectToAction("Index", new { courseName = courseName, justDoTest = false });
        }

        [HttpGet]
        public async Task<IActionResult> DoTest()
        {
            if (HttpContext.Session.GetString("LoginId") == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            int checkUser = (int)_db.Taikhoannguoidungs.Where(m => m.Id == HttpContext.Session.GetInt32("LoginId")).First().IdxetDuyet;
            if (checkUser == 1)
            {
                Baikiemtra baikiemtra = new Baikiemtra();
                baikiemtra.quizes = _db.Baikiemtras.ToList();

                return View(_db.Baikiemtras.ToList());
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DoTest(List<Baikiemtra> baikiemtras)
        {

            // get answer 
            List<string> allAnswers = new List<string>();
            foreach (var item in baikiemtras)
            {
                string answerInItem = "";
                if (item.aChecked != "")
                {
                    answerInItem += item.aChecked;
                }
                if (item.bChecked != "")
                {
                    answerInItem += item.bChecked;
                }
                if (item.cChecked != "")
                {
                    answerInItem += item.cChecked;
                }
                if (item.dChecked != "")
                {
                    answerInItem += item.dChecked;
                }
                allAnswers.Add(answerInItem);
            }

            // get score per question and init user score
            List<string> rightAnswers = _db.Baikiemtras.Select(q => q.DapAnDung).ToList();
            decimal scorePerAnswer = (decimal)10 / (decimal)rightAnswers.Count();
            decimal userScore = 0;

            // grading quiz
            for (int i = 0; i < allAnswers.Count; i++)
            {
                //Console.WriteLine("answer: " + rightAnswers[i] + " choose: " + allAnswers[i] + " score per question: " + scorePerAnswer);
                if (allAnswers[i].Equals(rightAnswers[i]))
                {
                    userScore += scorePerAnswer;
                }
            }

            var userInfo = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
            Taikhoannguoidung taikhoannguoidung = _db.Taikhoannguoidungs.Find(userInfo.Id);
            taikhoannguoidung.DiemBaiTest = Math.Round(Convert.ToDouble(userScore), 2);
            if (taikhoannguoidung.DiemBaiTest > 7)
                taikhoannguoidung.IdxetDuyet = 2;

            TempData["Message"] = "Bạn đã hoàn thành khóa đào tạo, vui lòng theo dõi mail!";
            TempData["MessageType"] = "success";
            _db.Taikhoannguoidungs.Attach(taikhoannguoidung).Property(x => x.DiemBaiTest).IsModified = true;
            _db.SaveChanges();

            return RedirectToAction("Index", new { courseName = "", justDoTest = true });
        }

    }
}