using System;

[Serializable]
class TaoPhongModel
{
    public int owner_id;
    public int limit_player;
    public string password;
    public string room_name;
    public int bet_money;
    public int id_bai;
    public TaoPhongModel() { }
    public TaoPhongModel(int mOwer, int limit, string mPass, string room, int bet_money, int id_bai)
    {
        this.owner_id = mOwer;
        this.limit_player = limit;
        this.password = mPass;
        this.room_name = room;
        this.bet_money = bet_money;
        this.id_bai = id_bai;
    }
}

