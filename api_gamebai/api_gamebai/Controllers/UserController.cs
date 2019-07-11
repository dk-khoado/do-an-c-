using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using api_gamebai.Models;
using System.Web;
using System.Diagnostics;
using System.Web.Mvc.Routing;
using System.Security.Policy;

namespace api_gamebai.Controllers
{
    public class UserController : ApiController
    {
        Databasegamebai db = new Databasegamebai();
        ResponseMessage notifi = new ResponseMessage();
        public string Get()
        {
            return Url.Link("RegisterApi", new { controller = "user", action = "Register"});
        }
        [HttpPost]     
        public ResponseMessage Register([FromBody]RegisterUser muser)
        {           
            if (!ModelState.IsValid)
            {
                return new ResponseMessage(BadRequest().ToString(), "Loi");
            }                
            if(muser == null)
            {
               
                return new ResponseMessage(BadRequest().ToString(), "Khong duoc de trong");
            }
            if(muser.username == "" || muser.password ==""||muser.email == "")
            {
                return new ResponseMessage(BadRequest().ToString(), "Khong de trong cac truong nay");
            }            
            foreach (player item in db.players)
            {
                if(muser.username == item.username)
                {
                    return new ResponseMessage(BadRequest().ToString(), "Username da duoc su dung");
                }
            }
            foreach(player item in db.players)
            {
                if(muser.email == item.email)
                {
                    return new ResponseMessage(BadRequest().ToString(), "Email da duoc su dung");
                }
            }           
            try
            {                               
                player save = new player();
                save.username = muser.username;
                save.password = muser.password;
                save.email = muser.email;
                if (muser.nickname == null)
                {
                    save.nickname = muser.username;
                }
                else
                {
                    save.nickname = muser.nickname;
                }
                save.password = Mahoa(muser.password);
                db.players.Add(save);
                //gửi mail
                SendMail(muser.email, muser.username);
                db.SaveChanges();               
            }
            catch
            {
                if (muser.email == null || muser.username == null || muser.password == null)
                {
                    return new ResponseMessage(BadRequest().ToString(), "đối tượng trống");
                }               
                return new ResponseMessage(BadRequest().ToString(), "loi tum lum");
            }
            return new ResponseMessage(Ok().ToString(), "Them thanh cong");
        }
        //login vào server
        [HttpPost]
        public ResponseMessage Login([FromBody]UserLoginModel mlogin)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseMessage(BadRequest().ToString(), "Mot lo loi");
            }

            if (mlogin.username == "" || mlogin.password == "" || mlogin.isNull())
            {
                return new ResponseMessage(BadRequest().ToString(), "Khong duoc de trong");
            }
            if (db.players.Count(e => e.username == mlogin.username) > 0 && db.players.Count(e => e.password == mlogin.password) > 0)
            {
                player mPlayer = db.players.Where(e => e.username == mlogin.username && e.password == mlogin.password).FirstOrDefault();
                if (mPlayer.status == 0)
                {
                    return new ResponseMessage(Ok().ToString(), "tài khoản chưa xát thực", 0);
                }
                return new ResponseMessage(Ok().ToString(), "Dang nhap thanh cong", 1);
            }
            else
            {
                return new ResponseMessage(Ok().ToString(), "Dang nhap that bai", 0);
            }
        }
        private string Mahoa(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        /// <summary>
        /// gửi mail đến địa chỉ
        /// </summary>
        /// <param name="to">địa chỉ mail</param>
        private void SendMail(string to,string name)
        {
            string htmlMail;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Request.MapPath("~/Views/mailTemplate.html")))
            {
                string url = Url.Link("RegisterApi", new { controller = "user", action = "Register" });
                htmlMail = reader.ReadToEnd();
                htmlMail = htmlMail.Replace("{name}",name);
                htmlMail = htmlMail.Replace("{link}",url);
            }
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.googlemail.com");

                mail.From = new MailAddress("khoado29k11@viendong.edu.vn");
                mail.To.Add(to);
                mail.IsBodyHtml = true;
                mail.Subject = "Test Mail";
                mail.Body = htmlMail;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential("khoado29k11@viendong.edu.vn", "khoa958632147");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        
    }
}
