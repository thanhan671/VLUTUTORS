﻿using System;
using System.Collections.Generic;
using System.Linq;
using VLUTUTORS.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace VLUTUTORS.Support.Services
{
    public static class TutorServices
    {
        public static string SaveUploadFiles(string enviromentPath, string path, List<IFormFile> images)
        {
            List<string> filesName = new List<string>();
            string namesJson;
            string fullPath = Path.Combine(enviromentPath, path);
            if (!Directory.Exists(fullPath))
            {
                Console.WriteLine("directory not exists " + path);
                Directory.CreateDirectory(fullPath);
            }
            else
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in images)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                Console.WriteLine("get file name: " + fileName);
                using (FileStream stream = new FileStream(Path.Combine(enviromentPath, path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    //ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
                //filesName.Add(Path.Combine(path, fileName));
                filesName.Add(fileName);
            }
            namesJson = JsonConvert.SerializeObject(filesName);
            return namesJson;
        }

        public static void SaveFileToFolder(string enviromentPath, string path, List<IFormFile> images) 
        {
            string fullPath = Path.Combine(enviromentPath, path);
            if (!Directory.Exists(fullPath)) {
                Console.WriteLine("directory not exists " + path);
                Directory.CreateDirectory(fullPath);
            }
            else {
                DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
                foreach (FileInfo file in directoryInfo.GetFiles()) {
                    file.Delete();
                }
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in images) {
                string fileName = Path.GetFileName(postedFile.FileName);

                using (FileStream stream = new FileStream(Path.Combine(enviromentPath, path, fileName), FileMode.Create)) {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                }
            }
        }

        public static string SaveFileNameToDb(List<IFormFile> formFiles) 
        {
            List<string> filesName = new List<string>();
            foreach (IFormFile postedFile in formFiles)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                filesName.Add(fileName);
            }
            string namesJson = JsonConvert.SerializeObject(filesName);
            return namesJson;
        }

        public static string SaveAvatar(string enviromentPath, string path, List<IFormFile> images)
        {
            List<string> filesName = new List<string>();
            string namesJson;
            string fullPath = Path.Combine(enviromentPath, path);
            if (!Directory.Exists(fullPath))
            {
                Console.WriteLine("directory not exists " + path);
                Directory.CreateDirectory(fullPath);
            }
            else
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in images)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                Console.WriteLine("get file name: " + fileName);
                using (FileStream stream = new FileStream(Path.Combine(enviromentPath, path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    //ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
                filesName.Add(Path.Combine(path, fileName));
            }
            namesJson = JsonConvert.SerializeObject(filesName);
            return namesJson;
        }

        public static List<string> LoadImages(List<string> imagesNameList)
        {
            List<string> imagesPathList = new List<string>();
            foreach (var imagePath in imagesNameList)
            {
                string path = imagePath.Replace(@"\", @"\\");
                imagesPathList.Add(path);
                Console.WriteLine("certificate path: " + path);
            }

            return imagesPathList;
        }

        public static void UploadFile(IFormFile courseFile)
        {
            string fileName = Path.GetFileName(courseFile.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "CourseFiles", fileName);
            Console.WriteLine("path exist: " + Directory.Exists(path));
            var stream = new FileStream(path, FileMode.Create);
            courseFile.CopyToAsync(stream);
        }

        public static Taikhoannguoidung FindUserAccountByEmail(string email)
        {
            return DataManager.Instance().db().Taikhoannguoidungs.Where(u => u.Email.Equals(email)).FirstOrDefault();
        }

        public static void UpdateUserInfo(Taikhoannguoidung user)
        {
            DataManager.Instance().db().Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            DataManager.Instance().db().SaveChanges();
        }


    }
}