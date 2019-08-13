using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class FriendModel
{
    public string nickname;
    public bool isban;
    public string avartar;
    public int player_id;
    public int friend_id;
}
[Serializable]
public class ResponseFriend
{
    public List<FriendModel> data;
    public int result;
}

