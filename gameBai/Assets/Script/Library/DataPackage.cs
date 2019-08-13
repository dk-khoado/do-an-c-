using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class DataPackageEndTurn
{
    public bool isSkip = false;
    public bool onlyColor = false;
    public int indexColor;
    public int amountDraw;



    public Player currentPlayer;//người chơi đang đến lượt đi
    public Player preTurnPlayer;//người chơi trước đó
    public Player nextTurnPlayer;//người chơi kế tiếp
    public int direction;

    public List<int> currentPosCard = new List<int>();

    public int currentCard;//card hiện tại đang trên bàn 
    public DataPackageEndTurn()
    {

    }
    public DataPackageEndTurn(Player currentPlayer, Player preTurnPlayer, Player nextTurnPlayer, List<int> currentPosCard, int currentCard)
    {
        this.currentPlayer = currentPlayer;
        this.preTurnPlayer = preTurnPlayer;
        this.nextTurnPlayer = nextTurnPlayer;
        this.currentPosCard = currentPosCard;
        this.currentCard = currentCard;
    }
}
[Serializable]
public class DicListcard
{
    public int IDplayer;
    public List<int> card;
    public DicListcard()
    {

    }
    public DicListcard(int id, List<int> value)
    {
        IDplayer = id;
        card = value;
    }
}
[Serializable]
public class DataReadyOrStart
{   

    public Player currentPlayer;//người chơi đang đến lượt đi
    public Player preTurnPlayer;//người chơi trước đó
    public Player nextTurnPlayer;//người chơi kế tiếp
    
    public List<DicListcard> listCards;
    public List<int> currentPosCard = new List<int>();

    public bool isStart;
    public bool isReady;//thuộc tính chỉ xuất hiện khi đối tượng k phải chủ phòng
    public bool isPlaying;//true nếu đang chơi. false nếu đang chờ
}
[Serializable]
public class DataWin
{

}
