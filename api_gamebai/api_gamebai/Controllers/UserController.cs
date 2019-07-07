using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using api_gamebai.Models;

namespace api_gamebai.Controllers
{
    public class UserController : ApiController
    {
        DatabaseGameBaiEntities db = new DatabaseGameBaiEntities();
        Notification notifi = new Notification();
        [HttpPost]
        [Route("RegisterApi")]
        public Notification Register([FromBody]player muser)
        {
            muser.password = Mahoa(muser.password);
            if (!ModelState.IsValid)
            {
                return new Notification(BadRequest().ToString(), "Loi");
            }
            if(muser == null)
            {
                return new Notification(BadRequest().ToString(), "Khong duoc de trong");
            }
            if(muser.username == "" || muser.password ==" "||muser.email == "")
            {
                return new Notification(BadRequest().ToString(), "Khong de trong cac truong nay");
            }
            foreach(player item in db.players)
            {
                if(muser.username == item.username)
                {
                    return new Notification(BadRequest().ToString(), "Username da duoc su dung");
                }
            }
            foreach(player item in db.players)
            {
                if(muser.email == item.email)
                {
                    return new Notification(BadRequest().ToString(), "Email da duoc su dung");
                }
            }
            if(muser.nickname == null)
            {
                muser.nickname = muser.username;
            }
            db.players.Add(muser);
            try
            {
                db.SaveChanges();
            }
            catch
            {
                return new Notification(BadRequest().ToString(), "loi tum lum");
            }
            return new Notification(Ok().ToString(), "Them thanh cong");
        }
        [HttpPost]
        public Notification Login([FromBody]player mlogin)
        {
            mlogin.password = Mahoa(mlogin.password);
            if (!ModelState.IsValid)
            {
                return new Notification(BadRequest().ToString(), "Mot lo loi");
            }
            if(mlogin.username == ""||mlogin.password =="")
            {
                return new Notification(BadRequest().ToString(), "Khong duoc de trong");
            }
            if(db.players.Count(e =>e.username == mlogin.username)>0 && db.players.Count(e => e.password == mlogin.password) > 0)
            {
                return new Notification(Ok().ToString(), "Dang nhap thanh cong");
            }
            else
            {
                return new Notification(Ok().ToString(), "Dang nhap that bai");
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
    }
}
