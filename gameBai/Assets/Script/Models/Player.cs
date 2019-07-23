using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Player
{
    public int room_id;
    public int player_id;
    public string nickname;
    public double money;
    public string avartar;
}
public class DataRoomPlayer
{
    public List<Player> data;
}

