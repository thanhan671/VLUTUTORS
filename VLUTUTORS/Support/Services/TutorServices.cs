using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using VLUTUTORS.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace VLUTUTORS.Support.Services
{
    public static class TutorServices
    {
        public static void SaveUploadImages(string path)
        {
            
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
