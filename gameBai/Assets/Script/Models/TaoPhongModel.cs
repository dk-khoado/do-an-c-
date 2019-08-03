using System;

[Serializable]
class TaoPhongModel
{
    public int owner_id;
    public int limit_player;
    public string password;
    public string room_name;
    public TaoPhongModel() { }
    public TaoPhongModel(int mower, int limit, string mpass, string room)
    {
        this.owner_id = mower;
        this.limit_player = limit;
        this.password = mpass;
        this.room_name = room;
    }
}

