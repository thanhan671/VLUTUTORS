using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net.Mail;
using System;

namespace VLUTUTORS.Support.Services
{
    public class SendEmail
    {
        public static void ForGotPass(string Email, string mailBody)
        {
            string mailTitle = "Gia Sư Văn Lang";
            string fromMail = "giasuvanlang.thongtin@gmail.com";
            string fromEmailPass = "wwxtjmqczzdgwqke";

            //Email and content
            MailMessage message = new MailMessage(new MailAddress(fromMail, mailTitle), new MailAddress(Email));
            message.Subject = "Khôi phục mật khẩu";
            message.Body = mailBody;
            message.IsBodyHtml = true;

            //Server detail
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            //Credentials
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential();
            credential.UserName = fromMail;
            credential.Password = fromEmailPass;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credential;

            smtp.Send(message);

            return;
        }
    }
}
