using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_gamebai.Models
{
    public class RoomCreateModel
    {
        public int owner_id { get; set; }
        public int limit_player { get; set; }
        public string password { get; set; }
        public Nullable<int> current_player { get; set; }
        public string room_name { get; set; }
    }
}