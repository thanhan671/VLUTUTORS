using MailKit.Net.Smtp;
using Microsoft.Data.SqlClient;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VLUTUTORS.SendMail
{
    public class SendEmail
    {
        public void Successful(string Email)
        {

            var sqlStringBuilder = new SqlConnectionStringBuilder();
            sqlStringBuilder["Server"] = "tuleap.vanlanguni.edu.vn,18082";
            sqlStringBuilder["Database"] = "CP25Team01";
            sqlStringBuilder["UID"] = "CP25Team01";
            sqlStringBuilder["PWD"] = "Cap25t01";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Gia Sư Văn Lang", "giasuvanlang@gmail.com"));
            message.To.Add(new MailboxAddress("Gia Sư Văn Lang", Email));
            message.Subject = "Thông báo hoàn thành xét duyệt Gia Sư Văn Lang";
            message.Body = new TextPart("plain")
            {
                Text = "Chúc mừng bạn đã hoàn thành đợt xét duyệt trở thành gia sư của Gia Sư Văn Lang. " +
                "Bây giờ bạn có thể đăng nhập và sử dụng chức năng dành cho gia sư" + '\n' +
                "Trân trọng!"
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("thanhannguyen67@gmail.com", "zepyqmhzacjzgsid");

                client.Send(message);

                client.Disconnect(true);

                client.Dispose();
            }
        }
    }
}
