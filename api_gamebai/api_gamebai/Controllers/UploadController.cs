using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using api_gamebai.Models;

namespace api_gamebai.Controllers
{
    public class UploadController : ApiController
    {
        Databasegamebai db = new Databasegamebai();
        public ResponseMessage Avartar(int id)
        {
            try
            {
                player mPlayer = db.players.Find(id);
                if (!Directory.Exists("Upload"))
                {
                    Directory.CreateDirectory("Upload");
                }
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var test = "";
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var subname = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var main = postedFile.FileName.Replace(subname, ".jpg");
                        test = main;
                        var filePath = HttpContext.Current.Server.MapPath("~/Upload/" + main);                                          
                        postedFile.SaveAs(filePath);
                        docfiles.Add(filePath);
                        mPlayer.avartar = main;                     
                        db.Entry(mPlayer).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    return new ResponseMessage("tải thành cong",test, 1);
                }
                else {
                    return new ResponseMessage("tải thành cong",null,0);
                }               
            }
            catch (Exception e)
            {
                return new ResponseMessage("lỗi", e);                
            }
           
        }
    }      
}
