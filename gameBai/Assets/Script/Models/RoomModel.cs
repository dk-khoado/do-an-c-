using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class RoomModel
{
    public List<Player_InRoomModels> room_listplayer;
    public int id;
    public int owner_id;
    public int limit_player;
    public string password;
    public int current_player;
    public string room_name;
}

