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
public class sendDataLogin
{
    public string username;
    public string password;
    public sendDataLogin(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}
[Serializable]
public class DataFromLogin
{
    public string response;
    public string message;
    public int result;
    public Data data;
}
public class SendResgiter
{
    public string username;
    public string password;
    public string email;
    public SendResgiter(string username, string password, string email)
    {
        this.username = username;
        this.password = password;
        this.email = email;
    }
}
[Serializable]
public class Data
{
    public int id;
    public string username;
    public double money;
    public string email;
    public string password;
    public string avartar;
    public string nickname;
}

