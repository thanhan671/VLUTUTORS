using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;

namespace VLUTUTORS.Controllers
{
    public class TutorTrainingController : Controller
    {
        private CP25Team01Context _db = new CP25Team01Context();

        [HttpGet]
        public async Task<IActionResult> Index(string courseName)
        {
            var noiDung = await _db.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);
            
            ViewData["Slogan"] = noiDung.Slogan;
            ViewData["gtChanTrang"] = noiDung.GioiThieuChanTrang;
            ViewData["diaChi"] = noiDung.DiaChi;
            ViewData["Sdt"] = noiDung.Sdt;
            ViewData["Email"] = noiDung.Email;
            ViewData["Fb"] = noiDung.Facebook;
            ViewData["gioiThieu"] = noiDung.GioiThieu;
            //ViewData["link"] = "https://www.youtube.com/embed/" + "EIwhQLQhlXE";

            //List<Khoadaotao> lesson = new List<Khoadaotao>(); // remove this when function complete
            List<Khoadaotao> lesson = _db.Khoadaotaos.ToList();

            //Khoadaotao bai1 = new Khoadaotao(1, "how to marketing", "https://www.youtube.com/embed/" + "4ti_uK60nLk");
            //Khoadaotao bai2 = new Khoadaotao(2, "how to training", "https://www.youtube.com/embed/" + "Owf8emXuzAk");
            //Khoadaotao bai3 = new Khoadaotao(3, "how to make powerpoint", "https://www.youtube.com/embed/" + "XF34-Wu6qWU");
            //lesson.Add(bai1);
            //lesson.Add(bai2);
            //lesson.Add(bai3);
            // can remove above code lines

            Khoadaotao baihoc = new Khoadaotao();
            baihoc.courses = (from l in lesson
                              select l.TenBaiHoc).ToList();
            // generate course name list (keep this)

            //courseName = courseName == null ? bai1.TenBaiHoc : courseName; // can remove this
            courseName = courseName == null ? _db.Khoadaotaos.FirstOrDefault(kdt => kdt.IdBaiHoc == 1).TenBaiHoc : courseName; // use this instead of above code line
            Console.WriteLine("ten bai hoc la gi: " + courseName);
            //ViewData["link"] = lesson.Find(l => l.TenBaiHoc.Equals(courseName)).Link; // can remove this
            ViewData["link"] = _db.Khoadaotaos.FirstOrDefault(l => l.TenBaiHoc.Equals(courseName)).Link; // use this instead of above code line

            return View(baihoc);
        }

        [HttpPost]
        public IActionResult Index(string courseName, int id = 1)
        {
            Console.WriteLine("lesson: " + courseName);
            return RedirectToAction("Index", new { courseName = courseName});
        }

        [HttpGet]
        public async Task<IActionResult> DoTest()
        {
            var noiDung = await _db.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);
            ViewData["Slogan"] = noiDung.Slogan;
            ViewData["gtChanTrang"] = noiDung.GioiThieuChanTrang;
            ViewData["diaChi"] = noiDung.DiaChi;
            ViewData["Sdt"] = noiDung.Sdt;
            ViewData["Email"] = noiDung.Email;
            ViewData["Fb"] = noiDung.Facebook;
            ViewData["gioiThieu"] = noiDung.GioiThieu;

            Baikiemtra baikiemtra = new Baikiemtra();
            baikiemtra.quizes = _db.Baikiemtras.ToList();
            //foreach(var item in baikiemtra.quizes)
            //{
            //    string option = item.
            //}

            return View(_db.Baikiemtras.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> DoTest(List<Baikiemtra> baikiemtras)
        {
            var noiDung = await _db.Noidungs.FirstOrDefaultAsync(m => m.Id == 1);
            ViewData["Slogan"] = noiDung.Slogan;
            ViewData["gtChanTrang"] = noiDung.GioiThieuChanTrang;
            ViewData["diaChi"] = noiDung.DiaChi;
            ViewData["Sdt"] = noiDung.Sdt;
            ViewData["Email"] = noiDung.Email;
            ViewData["Fb"] = noiDung.Facebook;
            ViewData["gioiThieu"] = noiDung.GioiThieu;

            
            // get answer 
            List<string> allAnswers = new List<string>();
            foreach(var item in baikiemtras)
            {
                string answerInItem = "";
                if(item.aChecked != "")
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
            decimal scorePerAnswer = Math.Round(Convert.ToDecimal(10 / rightAnswers.Count()), 10);
            decimal userScore = 0;

            // grading quiz
            for (int i = 0; i < rightAnswers.Count; i++)
            {
                if (allAnswers[i].Equals(rightAnswers[i]))
                {
                    userScore += scorePerAnswer;
                }
            }

            var userInfo = JsonConvert.DeserializeObject<Taikhoannguoidung>(HttpContext.Session.GetString("SessionInfo"));
            Taikhoannguoidung taikhoannguoidung = _db.Taikhoannguoidungs.Find(userInfo.Id);
            taikhoannguoidung.DiemBaiTest = Convert.ToInt32(userScore);

            _db.Taikhoannguoidungs.Attach(taikhoannguoidung).Property(x => x.DiemBaiTest).IsModified = true;
            _db.SaveChanges();
            Console.WriteLine("Score of user: " + taikhoannguoidung.DiemBaiTest);

            return View(_db.Baikiemtras.ToList());
        }

    }
}
