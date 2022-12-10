using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using VLUTUTORS.Models;

namespace VLUTUTORS.Support.Services
{
    public class TutorServices
    {
        public void SaveUploadImages(string path)
        {
            
        }

        public Taikhoannguoidung FindUserAccountByEmail(string email)
        {
            Taikhoannguoidung taikhoannguoidung = new Taikhoannguoidung(); // temp -> update when write new datamanager with singleton pattern and change this to linq find user with email

            return taikhoannguoidung;
        }
    }
}
