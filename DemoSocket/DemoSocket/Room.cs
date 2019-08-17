using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocket
{
    public class Room
    {
        public List<int> listPlayer = new List<int>();
        public bool isPlaying;
        public Room()
        {

        }
        public Room(List<int> listPlayer)
        {
            this.listPlayer = listPlayer;
        }
        public bool Empty()
        {
            if (listPlayer.Count <1)
            {
                return true;
            }
            return false;
        }
    }
}
