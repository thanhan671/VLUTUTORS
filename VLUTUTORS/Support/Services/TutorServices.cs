using System;
using System.Collections.Generic;
using System.Linq;
using VLUTUTORS.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Data;
using Newtonsoft.Json;

namespace VLUTUTORS.Support.Services
{
    public static class TutorServices
    {
        public static string SaveUploadImages(string path, List<IFormFile> certificates)
        {
            List<string> filesName = new List<string>();
            string namesJson;
            if (!Directory.Exists(path))
            {
                Console.WriteLine("directory not exists");
                Directory.CreateDirectory(path);
            }
            else
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in certificates)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                Console.WriteLine("get file name: " + fileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    //ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
                filesName.Add(fileName);
                
            }
            namesJson = JsonConvert.SerializeObject(filesName);
            return namesJson;
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
