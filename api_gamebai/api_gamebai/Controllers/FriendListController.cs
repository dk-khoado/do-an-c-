using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api_gamebai.Models;

namespace api_gamebai.Controllers
{
    public class FriendListController : ApiController
    {
        DatabaseGameBai_friend db = new DatabaseGameBai_friend();
        [HttpPost]
        public ResponseMessage AddFriend([FromBody]AddFriendModel player)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseMessage("fail!!!");
            }

            if (player == null)
            {
                return new ResponseMessage("dữ liệu trống!!!");
            }
            //khi đã là bạn rồi thì không thêm đc nữa
            foreach (player_listfriend item in db.player_listfriend)
            {
                if (player.friend_id == item.friend_id)
                {
                    return new ResponseMessage(player.player_id + " đã là bạn với " + player.friend_id);
                }
            }
            //chỉ thêm bạn khi isban = false
            if (player.isban == false)
            {
                player_listfriend player_ = new player_listfriend();
                player_.player_id = player.player_id;
                player_.friend_id = player.friend_id;
                db.player_listfriend.Add(player_);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return new ResponseMessage("fail");
                }
                return new ResponseMessage(player.player_id + " là bạn của " + player.friend_id);
            }
            else
            {

                return new ResponseMessage("Bạn đã từ chối lời mời kết bạn của " + player.friend_id);
            }


        }

        /// <summary>
        /// Đây là hàm xử dụng khi đã thêm bạn thành công và bạn có quyền cấm anh ta
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseMessage Ban([FromBody]player_listfriend player)
        {


            if (!ModelState.IsValid)
            {
                return new ResponseMessage("fail!!!");
            }

            if (player == null)
            {
                return new ResponseMessage("dữ liệu trống!!!");
            }


            if (player.isban == false && (db.player_listfriend.Count(e => e.friend_id == player.friend_id) > 0))
            {
                string query = "Update player_listfriend set isban = 'false' where player_id = " + player.player_id + " and friend_id = " + player.friend_id;
                db.Database.ExecuteSqlCommand(query);
                return new ResponseMessage(player.player_id + " đã gỡ chặn " + player.friend_id);
            }

            else if (player.isban == true && (db.player_listfriend.Count(e => e.friend_id == player.friend_id) > 0))
            {
                string query = "Update player_listfriend set isban = 'true' where player_id = " + player.player_id + " and friend_id = " + player.friend_id;
                db.Database.ExecuteSqlCommand(query);
                return new ResponseMessage(player.player_id + " đã chặn " + player.friend_id);
            }

            else if (player.isban == true)
            {
                string query = "Insert into player_listfriend values ('" + player.player_id + "' , '" + player.friend_id + "' , '" + player.isban + "')";
                db.Database.ExecuteSqlCommand(query);

                return new ResponseMessage(player.player_id + " đã chặn " + player.friend_id);

            }
            else
            {
                return new ResponseMessage("Unknown !!!");
            }
        }
        /// <summary>
        /// lấy danh sách bạn của player
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseMessage GetListfriends(int id)
        {
            ResponseMessage response = new ResponseMessage("Succes", db.vListFriends.Where(e => e.player_id == id).ToList());
            return response;
        }        
    }
}
