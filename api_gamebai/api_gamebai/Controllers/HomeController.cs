using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using api_gamebai.Models;

namespace api_gamebai.Controllers
{
    public class HomeController : Controller
    {
        
        Databasegamebai db = new Databasegamebai();
        [Route("Default")]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }        
        public ActionResult Verify(string key)
        {            
            var player_Key = db.player_key.Where(e => e.keyuser == key).FirstOrDefault();
            var player = db.players.Find(player_Key.playerID);
            if (player.status == 0)
            {
                player.status = 1;
                player_Key.keyStatus = 1;
                db.Entry(player).State = System.Data.Entity.EntityState.Modified;
                db.Entry(player_Key).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return View();
            }
            return RedirectToAction("Index");
        }
    }
}
