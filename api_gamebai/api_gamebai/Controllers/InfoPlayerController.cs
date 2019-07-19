using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api_gamebai.Models;

namespace api_gamebai.Controllers
{
    public class InfoPlayerController : ApiController
    {
        Databasegamebai db = new Databasegamebai();
        public ResponseMessage Get(int id)
        {            
            return new ResponseMessage("success", db.infoplayers.Find(),1);
        }        
    }
}
