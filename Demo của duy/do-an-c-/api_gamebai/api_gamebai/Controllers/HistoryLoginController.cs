using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api_gamebai.Models;

namespace api_gamebai.Controllers
{
    public class HistoryLoginController : ApiController
    {
        DbHistoryLogin db = new DbHistoryLogin();

        [HttpPost]
        public Mess SaveHistory([FromBody]history_login history)
        {
            if (!ModelState.IsValid)
            {
                return new Mess("fail!!!");
            }

            if (history == null)
            {
                return new Mess("dữ liệu trống!!!");
            }
            history.login_time = DateTime.Now;

            if(db.history_login.Count(e => e.player_id == history.player_id) > 0)
            {
                db.Entry(history).State = System.Data.Entity.EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return new Mess("fail");
                }
                return new Mess("Update thàng công :)))");
            }
            else
            {
                db.history_login.Add(history);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return new Mess("fail");
                }
                return new Mess("Đã thêm mới :)))");
            }
           
           
        }
    }
}
