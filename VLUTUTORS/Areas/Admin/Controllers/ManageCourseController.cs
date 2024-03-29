﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using VLUTUTORS.Support.Services;

namespace VLUTUTORS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "1")]

    public class ManageCourseController : Controller
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        private readonly ILogger<ManageCourseController> _logger;
        private IHostingEnvironment _environment;
        public ManageCourseController(ILogger<ManageCourseController> logger, IHostingEnvironment environment)
        {
            _logger = logger;
            this._environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoginADId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var khoaHocs = await _context.Khoadaotaos.ToListAsync();
            return View(khoaHocs);
        }

        [HttpGet]
        public ActionResult AddLesson()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddLesson(IFormCollection link, [Bind(include: "TenBaiHoc,TaiLieu,LinkVideo")] Khoadaotao khoadaotao, List<IFormFile> tepBaiGiang)
        {
            List<string> listLink = link["LinkVideo"].ToList();
            if (listLink.Contains(""))
            {
                listLink.Remove("");
            }
            for (int i = 0; i < listLink.Count; i++)
            {
                var editLink = listLink[i].Replace(@"https://www.youtube.com/watch?v=", @"https://www.youtube.com/embed/");
                listLink[i] = editLink;
            }

            string linkVideo = JsonConvert.SerializeObject(listLink);
            string filePath;

            if (ModelState.IsValid)
            {
                var checkKhoaDaoTao = _context.Khoadaotaos.AsNoTracking().SingleOrDefault(x => x.TenBaiHoc.ToLower() == khoadaotao.TenBaiHoc.ToLower());
                if (checkKhoaDaoTao != null)
                {
                    TempData["Message"] = "Bài học đã tồn tại, vui lòng kiểm tra lại";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index");
                }
                else
                {
                    try
                    {
                        if (listLink.Count != 0 && tepBaiGiang.Count == 0)
                        {
                            khoadaotao.LinkVideo = linkVideo;
                            khoadaotao.TaiLieu = null;
                            _context.Add(khoadaotao);
                            await _context.SaveChangesAsync();
                            TempData["Message"] = "Thêm thành công!";
                            TempData["MessageType"] = "success";
                        }
                        else if (listLink.Count == 0 && tepBaiGiang.Count != 0)
                        {
                            filePath = Path.Combine("Files", khoadaotao.IdBaiHoc.ToString());

                            khoadaotao.LinkVideo = null;
                            khoadaotao.TaiLieu = TutorServices.SaveUploadFiles(this._environment.WebRootPath, filePath, tepBaiGiang);
                            _context.Add(khoadaotao);
                            await _context.SaveChangesAsync();
                            TempData["Message"] = "Thêm thành công!";
                            TempData["MessageType"] = "success";

                            
                            TutorServices.SaveFileToFolder(this._environment.WebRootPath, filePath, tepBaiGiang);
                            Console.WriteLine("id: " + khoadaotao.IdBaiHoc.ToString());
                        }
                        else if (listLink.Count != 0 && tepBaiGiang.Count != 0)
                        {
                            filePath = Path.Combine("Files", khoadaotao.IdBaiHoc.ToString());
                            khoadaotao.LinkVideo = linkVideo;
                            khoadaotao.TaiLieu = TutorServices.SaveUploadFiles(this._environment.WebRootPath, filePath, tepBaiGiang);
                            _context.Add(khoadaotao);
                            await _context.SaveChangesAsync();
                            TempData["Message"] = "Thêm thành công!";
                            TempData["MessageType"] = "success";

                            Console.WriteLine("id: " + khoadaotao.IdBaiHoc.ToString());
                        }
                        else if (listLink.Count == 0 && tepBaiGiang.Count == 0)
                        {
                            TempData["Message"] = "Vui lòng thêm file hoặc link video!";
                            TempData["MessageType"] = "error";
                        }
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    
                    return RedirectToAction("Index");

                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditLesson(int? id = -1)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<Khoadaotao> lesson = _context.Khoadaotaos.ToList();

            var baihoc = await _context.Khoadaotaos.FirstOrDefaultAsync(m => m.IdBaiHoc == id);

            baihoc.TenBaiHoc = _context.Khoadaotaos.Where(l => l.IdBaiHoc == id).First().TenBaiHoc;

            string videoListInJson = _context.Khoadaotaos.Where(l => l.IdBaiHoc == id).First().LinkVideo;

            if (videoListInJson != null)
            {
                baihoc.courseLink = JsonConvert.DeserializeObject<List<string>>(videoListInJson);
            }
            else
            {
                baihoc.courseLink = null;
            }

            baihoc.TaiLieu = _context.Khoadaotaos.Where(l => l.IdBaiHoc == id).First().TaiLieu;

            return View(baihoc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLesson(int id, [Bind(include: "IdBaiHoc,TenBaiHoc,TaiLieu,LinkVideo")] Khoadaotao khoadaotao, List<IFormFile> tepBaiGiang, IFormCollection link)
        {
            if (id != khoadaotao.IdBaiHoc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var baihoc = await _context.Khoadaotaos.FirstOrDefaultAsync(m => m.IdBaiHoc == id);
                    List<string> listLink = link["LinkVideo"].ToList();
                    if (listLink.Contains(""))
                    {
                        listLink.Remove("");
                    }
                    for (int i = 0; i < listLink.Count; i++)
                    {
                        var editLink = listLink[i].Replace(@"https://www.youtube.com/watch?v=", @"https://www.youtube.com/embed/");
                        listLink[i] = editLink;
                    }

                    string linkVideo = JsonConvert.SerializeObject(listLink);
                    string filePath = Path.Combine("Files", khoadaotao.IdBaiHoc.ToString());
                    baihoc.LinkVideo = linkVideo;
                    baihoc.TaiLieu = tepBaiGiang.Count != 0 ? TutorServices.SaveUploadFiles(this._environment.WebRootPath, filePath, tepBaiGiang) : baihoc.TaiLieu;
                    _context.Update(baihoc);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Cập nhật thành công!";
                    TempData["MessageType"] = "success";
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteLesson([FromForm] int hdInput)
        {
            Khoadaotao khoadaotao = _context.Khoadaotaos.Where(p => p.IdBaiHoc == hdInput).FirstOrDefault();
            if(khoadaotao.TaiLieu !=null)
            {
                string filePath = Path.Combine(this._environment.WebRootPath, "Files", khoadaotao.TenBaiHoc.Trim());
                Directory.Delete(filePath, true);
            }
            _context.Khoadaotaos.Remove(khoadaotao);
            _context.SaveChanges();
            TempData["Message"] = "Xóa thành công!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }

        public FileResult DownloadFile(string courseName, string fileName)
        {
            string path = Path.Combine(this._environment.WebRootPath, "Files", courseName, fileName);

            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", fileName);
        }
    }
}