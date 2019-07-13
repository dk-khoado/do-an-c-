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
        Databasegamebai db = new Databasegamebai();

        [HttpPost]
        public ResponseMessage SaveHistory([FromBody]history_login history)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseMessage("fail!!!");
            }

            if (history == null)
            {
                return new ResponseMessage("dữ liệu trống!!!");
            }
            history.login_time = DateTime.Now;

            if(db.history_login.Count(e => e.player_id == history.player_id) > 0)
            {
                db.Entry(history).State = System.Data.Entity.EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return new ResponseMessage("fail");
                }
                return new ResponseMessage("Update thàng công :)))");
            }
            else
            {
                db.history_login.Add(history);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return new ResponseMessage("fail");
                }
                return new ResponseMessage("Đã thêm mới :)))");
            }
           
           
        }
    }
}
