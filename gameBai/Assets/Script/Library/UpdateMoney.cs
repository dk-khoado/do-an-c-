using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class PlayerUpdateMoneyModel
{
    public int ID_player;
    public int ID_room;
    public string cmd;
    public UpdateMoney[] message;
    public PlayerUpdateMoneyModel()
    {

    }
}

[Serializable]
public class UpdateMoney
{
    public int ID_player;
    public int money;
}

