using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Library
{
    [Serializable]
    public class DataPackage
    {



    }
    [Serializable]
    public class DataReadyOrStart
    {
        public Player currentPlayer;//người chơi đang đến lượt đi
        public Player preTurnPlayer;//người chơi trước đó
        public Player nextTurnPlayer;//người chơi kế tiếp

        public Dictionary<int, List<int>> listCards;
        public List<int> currentPosCard = new List<int>();

        public bool isReady;//thuộc tính chỉ xuất hiện khi đối tượng k phải chủ phòng
        public bool isPlaying;//true nếu đang chơi. false nếu đang chờ
    }


}
