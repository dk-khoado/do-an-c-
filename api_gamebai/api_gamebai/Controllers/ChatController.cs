using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using api_gamebai.Models;

namespace api_gamebai.Controllers
{
    public class ChatController : ApiController
    {
        DatabaseGameBai_friend db = new DatabaseGameBai_friend();      
       public List<LoadListFriend_Result> Get(int id,[FromBody]UserLoginModel mLogin)
        {           
            return db.LoadListFriend(id).ToList();
        }
        [HttpPost]
        public ResponseMessage SendMessage([FromBody]SendChatModel sendChat)
        {
            try
            {
                if (db.sendmessage(sendChat.id_send, sendChat.id_recive, sendChat.message) > 0)
                {
                    return new ResponseMessage("sucsess", "Đã gửi", 9);
                }
                else
                {
                    return new ResponseMessage("Lỗi :(");
                }
            }
            catch (Exception)
            {

                return new ResponseMessage("lỗi không gửi được");
            }
           
        }
    }
}
