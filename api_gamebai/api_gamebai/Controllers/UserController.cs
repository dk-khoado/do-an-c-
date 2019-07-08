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
        Databasegamebai db = new Databasegamebai();
        ResponseMessage notifi = new ResponseMessage();
        [HttpPost]     
        public ResponseMessage Register([FromBody]player muser)
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
            if(muser.nickname == null)
            {
                muser.nickname = muser.username;
            }
            try
            {
                muser.password = Mahoa(muser.password);
                db.players.Add(muser);
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
    }
}
