using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using api_gamebai.Models;

namespace api_gamebai.Controllers
{
    public class RoomListController : ApiController
    {
        private Databasegamebai db = new Databasegamebai();
        [HttpPost]
        public ResponseMessage CreateRoom([FromBody] room_list mRoomList)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseMessage(BadRequest().ToString(),"Đã có lỗi");
            }
            if (mRoomList.limit_player == null)
            {
                return new ResponseMessage(BadRequest().ToString(), "Vui lòng điền vào số lượng player");
            }
            if (mRoomList.current_player == null)
            {
                return new ResponseMessage(BadRequest().ToString(), "Vui lòng điền vào số lượng player hiện tại");
            }          
            foreach(room_list item in db.room_list)
            {
                if (mRoomList.current_player == 0)
                {
                    mRoomList.current_player += 1;
                }
            }
            db.room_list.Add(mRoomList);
            db.SaveChanges();
            return new ResponseMessage(Ok().ToString(),"Đã thêm thành công");
        }
    }   
}