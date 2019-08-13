using System;
using System.Collections.Generic;

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
    public int bet_money;
    public int id_bai;
}

