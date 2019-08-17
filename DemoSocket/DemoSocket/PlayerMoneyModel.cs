using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocket
{
    public class PlayerMoneyModel
    {
        public int ID_player;
        public decimal money;
        public PlayerMoneyModel()
        {

        }
        public PlayerMoneyModel(int ID_player, decimal money)
        {
            this.ID_player = ID_player;
            this.money = money;
        }
    }
    public class DataPackedPlayerMoney
    {
        public int ID_player;
        public int ID_room;
        public string cmd;
        public List<PlayerMoneyModel> message;
        public double sumMoneyWin;
    }
}
