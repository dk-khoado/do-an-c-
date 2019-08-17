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
        public ResponseMessage Get(int id)
        {
            return new ResponseMessage("succes", db.players.Find(id), 1);
        }
        [HttpPost]
        public ResponseMessage Register([FromBody]RegisterUser muser)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseMessage(BadRequest().ToString(), "Loi");
            }
            if (muser == null)
            {

                return new ResponseMessage(BadRequest().ToString(), "Khong duoc de trong");
            }
            if (muser.username == "" || muser.password == "" || muser.email == "")
            {
                return new ResponseMessage(BadRequest().ToString(), "Khong de trong cac truong nay");
            }
            foreach (player item in db.players)
            {
                if (muser.username == item.username)
                {
                    return new ResponseMessage(BadRequest().ToString(), "Username da duoc su dung");
                }
            }
            foreach (player item in db.players)
            {
                if (muser.email == item.email)
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
                    muser.nickname = muser.username;
                    save.nickname = muser.nickname;
                }
                else
                {
                    if (muser.nickname.Trim().Equals(""))
                    {
                        save.nickname = muser.username;
                    }
                    else
                    {
                        save.nickname = muser.nickname;
                    }                    
                }
                save.password = Mahoa(muser.password);
                db.players.Add(save);
                db.SaveChanges();
                //gửi mail
                string key = CreateKey(db.getID(save.username,save.password).FirstOrDefault().GetValueOrDefault());
                SendMail(save.email, save.username,key);

            }
            catch
            {
                if (muser.email == null || muser.username == null || muser.password == null)
                {
                    return new ResponseMessage(BadRequest().ToString(), "đối tượng trống");
                }
                throw;
                //return new ResponseMessage(BadRequest().ToString(), "loi tum lum");
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
            string password = Mahoa(mlogin.password);
            if (db.players.Count(e => e.username == mlogin.username) > 0 && db.players.Count(e => e.password == password) > 0)
            {
                var mPlayer = db.LoginData(mlogin.username, Mahoa( mlogin.password)).FirstOrDefault();              
                if (mPlayer== null)
                {
                    return new ResponseMessage(BadRequest().ToString(), "Lỗi 404");
                }
                if (mPlayer.status == 0)
                {
                    return new ResponseMessage(Ok().ToString(), "tài khoản chưa xát thực", 0);
                }
                mPlayer.password = null;
                return new ResponseMessage(mPlayer.id.ToString(),"Dang nhap thanh cong", mPlayer,1);
            }
            else
            {
                return new ResponseMessage(Ok().ToString(), "Dang nhap that bai", 0);
            }
        }
        /// <summary>
        /// tạo key xát thực
        /// </summary>
        /// <returns></returns>
        private string CreateKey(int idPlayer)
        {
            Random random = new Random();
            do
            {
                StringBuilder key = new StringBuilder();
                for (int i = 0; i < 6; i++)
                {
                    
                    int number = random.Next(0, 2);
                    switch (number)
                    {
                        case 0:
                            key.Append(random.Next(0,9));
                            break;
                        case 1:
                            key.Append(Convert.ToChar(random.Next(65, 90)));
                            break;
                        case 2:
                            key.Append(Convert.ToChar(random.Next(97, 122)));
                            break;
                        default:
                            break;
                    }
                }
                string key_ = key.ToString();
                if (db.player_key.Count(e => e.keyuser == key_) < 1)
                {
                    player_key player_Key_ = new player_key();
                    player_Key_.keyuser = key.ToString();
                    player_Key_.playerID = idPlayer;                    
                    db.player_key.Add(player_Key_);
                    db.SaveChanges();
                    return key.ToString();
                }
            } while (true);
        }
        /// <summary>
        /// mã hóa password thành md5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
        private void SendMail(string to, string name, string keyUser)
        {
            string htmlMail;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Request.MapPath("~/Views/mailTemplate.html")))
            {
                string url = Url.Link("Default", new { controller = "Home", action = "Verify" })+ "?key=" + keyUser;
                htmlMail = reader.ReadToEnd();
                htmlMail = htmlMail.Replace("{name}", name);
                htmlMail = htmlMail.Replace("{link}", url);
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
        [HttpGet]
        public IHttpActionResult ForgotPassword(int id)
        {
            
            return RedirectToRoute("Default",new { controller="Home",action="ChangePassword"});
        }


        private void SendMailForgotPassword(string to, string name, string keyUser)
        {
            string htmlMail;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Request.MapPath("~/Views/mailTemplate.html")))
            {
               
                string url = Url.Link("Default", new { controller = "Home", action = "Change" }) + "?key=" + keyUser;
                htmlMail = reader.ReadToEnd();
                htmlMail = htmlMail.Replace("{name}", name);
                htmlMail = htmlMail.Replace("{link}", url);
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
