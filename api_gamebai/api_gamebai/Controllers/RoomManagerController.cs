using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api_gamebai.Models;

namespace api_gamebai.Controllers
{
    public class RoomManagerController : ApiController
    {
        DatabaseGameBai_Room db = new DatabaseGameBai_Room();
        

        //Tạo phòng
        [HttpPost]
        public ResponseMessage CreateRoom([FromBody]RoomCreateModel room)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseMessage(BadRequest().ToString(), "Fail!!!");
            }
            if (room == null)
            {
                return new ResponseMessage(BadRequest().ToString(), "Trống !!!");
            }

            if(room.owner_id.ToString() == "" || room.limit_player.ToString() == "" || room.room_name == "" || room.room_name == null)
            {
                return new ResponseMessage(BadRequest().ToString(), "Không đc nhập thiếu");
            }
            
            
            //Nếu chưa có phòng thì tạo, ngược lại nếu có rồi thì thôi
            if (db.room_list.Count(e => e.owner_id == room.owner_id) > 0)
            {
                return new ResponseMessage(BadRequest().ToString(), "Người chơi " + room.owner_id + " đã trong một phòng");
            }
            else
            {
                if (db.room_list.Count(e => e.room_name == room.room_name) > 0)
                {
                    return new ResponseMessage(BadRequest().ToString(), "Tên phòng đã tồn tại !!!");
                }
                else
                {
                    
                    //Nếu người chơi tồn tại thì cho tạo phòng còn không thì thôi
                    if (db.players.Count(e => e.id == room.owner_id) > 0)
                    {
                        db.addRoomList(room.owner_id, room.limit_player, room.password, room.room_name);
                        
                        return new ResponseMessage(Ok().ToString(), "Tạo phòng thành công", db.GetRoomByIDPlayer(room.owner_id).FirstOrDefault(),1);

                    }
                    else
                    {
                        return new ResponseMessage(BadRequest().ToString(), "Người chơi không tồn tại không thể tạo phòng !!!");
                    }
                }
                
            }

        }
        [HttpPost]
        public ResponseMessage JoinRoom([FromBody]RoomJoinModel room)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseMessage(BadRequest().ToString(), "Fail!!!");
            }
            if (room == null)
            {
                return new ResponseMessage(BadRequest().ToString(), "Trống !!!");
            }
            if(room.room_id.ToString() == "" || room.player_id.ToString() == "")
            {
                return new ResponseMessage(BadRequest().ToString(), "Không đc nhập thiếu");
            }
            //Nếu phòng tồn tại thì cho vào, ngược lại thì không thể vào vì làm đéo gì có phòng mà vào
            if (db.room_listplayer.Count(e => e.room_id == room.room_id) > 0)
            {
                //Nếu người chơi tồn tại thì cho vào còn không thì báo lỗi
                if (db.players.Count(e => e.id == room.player_id) > 0)
                {
                    //Nếu người chơi đã trong phòng nào đó thì ko cho vào phòng khác
                    if (db.room_listplayer.Count(e => e.player_id == room.player_id) > 0)
                    {
                        return new ResponseMessage(BadRequest().ToString(), "Người chơi " + room.player_id + " đã trong một phòng");
                        
                    }
                    else
                    {
                        try
                        {
                            db.JoinRoomList(room.room_id, room.player_id);
                            return new ResponseMessage(Ok().ToString(), room.player_id + " đã vào phòng " + room.room_id);
                        }
                        catch(Exception)
                        {
                            return new ResponseMessage("Phòng " + room.room_id + " đã đầy !!!");
                        }
                        
                    }
                }
                else
                {
                    return new ResponseMessage(BadRequest().ToString(), "Người chơi không tồn tại !!!");
                }


            }
            else
            {
                return new ResponseMessage(BadRequest().ToString(), "Phòng không tồn tại");
            }

            
        }


        [HttpPost]
        public ResponseMessage LeaveRoom([FromBody] RoomLeaveModel room)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseMessage(BadRequest().ToString(), "Fail!!!");
            }
            if (room == null)
            {
                return new ResponseMessage(BadRequest().ToString(), "Trống !!!");
            }
            if (room.room_id.ToString() == "" || room.player_id.ToString() == "")
            {
                return new ResponseMessage(BadRequest().ToString(), "Không đc nhập thiếu");
            }

            //Nếu phòng tồn tại thì cho out, ngược lại thì không thể
            if (db.room_listplayer.Count(e => e.room_id == room.room_id) > 0)
            {
                //Nếu người chơi tồn tại thì cho out còn không thì báo lỗi
                if (db.players.Count(e => e.id == room.player_id) > 0)
                {
                    if (db.room_listplayer.Count(e => e.player_id == room.player_id) > 0)
                    {
                        db.OutRoom(room.room_id, room.player_id);
                        return new ResponseMessage(Ok().ToString(), room.player_id + " đã rời phòng !!!");
                    }
                    else
                    {
                        return new ResponseMessage("Người chơi " + room.player_id + " không tồn tại trong phòng " + room.room_id);
                    }
                }
                else
                {
                    return new ResponseMessage(BadRequest().ToString(), "Người chơi không tồn tại !!!");
                }
            }
            else
            {
                return new ResponseMessage(BadRequest().ToString(), "Phòng không tồn tại");
            }
        }

        [HttpPost]
        public ResponseMessage GetRoomList()
        {           
            return new ResponseMessage("ok", db.room_list.ToList());
        }
        public ResponseMessage GetPlayerInRoom(int ID_room)
        {            
            return new ResponseMessage("Lấy dữ liêu thành công", db.GetListPlayerInRoom(ID_room).ToList());
        }       
    }
}
