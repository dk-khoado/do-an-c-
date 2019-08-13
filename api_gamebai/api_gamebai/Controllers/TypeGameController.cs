using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api_gamebai.Models;

namespace api_gamebai.Controllers
{
    public class TypeGameController : ApiController
    {
        DatabaseGameBai_Room db = new DatabaseGameBai_Room();
        //lấy tất cả loại bài
        public ResponseMessage Get()
        {
            return new ResponseMessage("ok",db.ListBais);
        }
    }
}
