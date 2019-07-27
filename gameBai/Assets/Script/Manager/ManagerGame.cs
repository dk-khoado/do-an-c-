using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(UI_manager))]
public class ManagerGame : MonoBehaviour
{
    public int IDLocalPlayer;
    public int AmountCardPerPlayer;
    public GameObject localPlayer;//đói tượng nội bộ      

    public List<GameObject> remotePlayers;//danh sách người chơi trừ người chơi nội bộ

    public GameObject ModelCard;//đối tượng card sẽ được tạo
    public CardModel currentCard;//card hiện tại đang trên bàn 
    public GameObject selectCard;//card đang chọn
    public Player preTurnPlayer;//người chơi trước đó
    public Player nextTurnPlayer;//người chơi kế tiếp
    public EDirection direction;//hướng đi của lượt đánh
    public Player currentPlayer;//người chơi đang đến lượt đi
    public Player lastWinner;//người chiến thắng gần nhất
    public int postionPlayer;

    public List<CardModel> cardModels = new List<CardModel>();
    public List<int> currentPosCard = new List<int>();

    UI_manager uI_Manager;
    Controller_NetWork netWork;

    public bool isReady;//thuộc tính chỉ xuất hiện khi đối tượng k phải chủ phòng
    public bool isPlaying;//true nếu đang chơi. false nếu đang chờ
    void Start()
    {

        uI_Manager = GetComponent<UI_manager>();
        netWork = GetComponent<Controller_NetWork>();
        LoadCard();
        //XaoBai();
        FindPostionRemotePlayer();
        IDLocalPlayer = PlayerPrefs.GetInt("id");
    }
    //tải tài nguyên các lá bài
    void LoadCard()
    {
        CardModel[] temp = Resources.LoadAll<CardModel>("uno/");
        foreach (var item in temp)
        {
            if (item.color != Color_Card.Black)
            {
                if (item.number == 0 && !item.isChucNang)
                {
                    cardModels.Add(item);
                }
                else
                {
                    cardModels.Add(item);
                    cardModels.Add(item);
                }
            }
            else
            {
                cardModels.Add(item);
                cardModels.Add(item);
                cardModels.Add(item);
                cardModels.Add(item);
            }
        }
    }
    public void XaoBai()
    {
        currentPosCard.Clear();
        int i = 0;
        while (i < cardModels.Count)
        {
            int randowm = Random.Range(0, cardModels.Count);
            if (!currentPosCard.Contains(randowm))
            {
                i++;
                currentPosCard.Add(randowm);
            }
        }
    }
    public void ChiaBai()
    {
        //XaoBai();
        if (currentPosCard.Count < 1)
        {
            return;
        }
        for (int i = 0; i < AmountCardPerPlayer; i++)
        {
            if (currentPosCard.Count < 1)
            {
                break;
            }
            GameObject temp = ModelCard;
            temp.GetComponent<ControllerCard>().Properties = cardModels[currentPosCard[0]];
            localPlayer.GetComponent<ControllerPlayer>().AddCard(ModelCard);
            currentPosCard.RemoveAt(0);
            foreach (var item in remotePlayers)
            {
                if (currentPosCard.Count < 1)
                {
                    break;
                }
                if (item.GetComponent<ControllerPlayer>() && !item.GetComponent<ControllerPlayer>().isEmty)
                {
                    ModelCard.GetComponent<ControllerCard>().Properties = cardModels[currentPosCard[0]];
                    item.GetComponent<ControllerPlayer>().AddCard(ModelCard);
                    currentPosCard.RemoveAt(0);
                }
            }
        }
    }
    public void NewGame()
    {
        currentCard = null;
        uI_Manager.properties = null;
        XaoBai();
        if (localPlayer.GetComponent<ControllerPlayer>())
        {
            localPlayer.GetComponent<ControllerPlayer>().RemoveAllCard();
        }
        foreach (var item in remotePlayers)
        {
            if (item.GetComponent<ControllerPlayer>())
            {
                item.GetComponent<ControllerPlayer>().RemoveAllCard();
            }
        }
    }
    private void FindPostionRemotePlayer()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("remote");
        if (remotePlayers.Count > 0)
        {
            return;
        }
        foreach (var item in objects)
        {
            remotePlayers.Add(item);
        }
    }
    /// <summary>
    /// đánh bài
    /// </summary>
    public void Attack()
    {
        if (currentPlayer.player_id != IDLocalPlayer)
        {
            return;
        }
        if (!currentCard)
        {
            if (selectCard.GetComponent<ControllerCard>().Properties.color == Color_Card.Black)
            {
                currentCard = selectCard.GetComponent<ControllerCard>().Properties;
                //localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                uI_Manager.ShowSelectColor();
            }
            else
            {
                currentCard = selectCard.GetComponent<ControllerCard>().Properties;
                localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
            }
        }
        if (!selectCard.GetComponent<ControllerCard>())
        {
            return;
        }
        ControllerCard controllerCard = selectCard.GetComponent<ControllerCard>();
        if (selectCard.GetComponent<ControllerCard>().Properties.color == Color_Card.Black)
        {
            currentCard = selectCard.GetComponent<ControllerCard>().Properties;
            //localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
            uI_Manager.ShowSelectColor();
        }
        else
        {
            if (!controllerCard.Properties.isChucNang)
            {
                if (controllerCard.Properties.number == currentCard.number || selectCard.GetComponent<ControllerCard>().Properties.color == currentCard.color)
                {
                    currentCard = selectCard.GetComponent<ControllerCard>().Properties;
                    localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                }
            }
            else
            {
                if (controllerCard.Properties.color == currentCard.color || (controllerCard.Properties.skill == currentCard.skill && currentCard.isChucNang))
                {
                    currentCard = selectCard.GetComponent<ControllerCard>().Properties;
                    localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                }
            }
        }
        uI_Manager.properties = currentCard;
        EndTurn();
    }
    /// <summary>
    /// chọn màu
    /// </summary>
    /// <param name="color_"></param>
    public void SelectColor(Color_Card color_)
    {
        CardModel cardModel = new CardModel();
        switch (color_)
        {
            case Color_Card.Red:
                cardModel.color = Color_Card.Red;
                cardModel.image = currentCard.image;
                cardModel.isChucNang = false;
                cardModel.number = 99;//để người chơi chỉ đánh được màu thui
                currentCard = cardModel;
                localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                break;
            case Color_Card.Blue:
                cardModel.isChucNang = false;
                cardModel.color = Color_Card.Blue;
                cardModel.image = currentCard.image;
                cardModel.number = 99;//để người chơi chỉ đánh được màu thui
                currentCard = cardModel;
                localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                break;
            case Color_Card.Green:
                cardModel.isChucNang = false;
                cardModel.color = Color_Card.Green;
                cardModel.image = currentCard.image;
                cardModel.number = 99;//để người chơi chỉ đánh được màu thui
                currentCard = cardModel;
                localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                break;
            case Color_Card.Yellow:
                cardModel.isChucNang = false;
                cardModel.color = Color_Card.Yellow;
                cardModel.image = currentCard.image;
                cardModel.number = 99;//để người chơi chỉ đánh được màu thui
                currentCard = cardModel;
                localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                break;
            case Color_Card.Black:
                break;
            default:
                break;
        }
        uI_Manager.properties = currentCard;
    }
    /// <summary>
    /// sản sàng hoặc bắt đầu
    /// </summary>
    public void onReadyOrStart()
    {
        if (lastWinner.player_id != 0)
        {
            currentPlayer = lastWinner;
        }
        if (IDLocalPlayer == PlayerPrefs.GetInt("id_owner"))
        {
            currentPlayer = localPlayer.GetComponent<ControllerPlayer>().player;
            NewGame();
            ChiaBai();

            uI_Manager.btnStart.SetActive(false);

            int pos = 0;
            GetPostionPlayer();
            currentPlayer = netWork.players[postionPlayer];
            if (postionPlayer < netWork.players.Count-1)
            {
                pos = postionPlayer + 1;
            }            
            nextTurnPlayer = netWork.players[pos];
            if (postionPlayer ==0)
            {
                pos = netWork.players.Count-1;
            }
            else
            {
                pos = postionPlayer - 1;
            }
            preTurnPlayer = netWork.players[pos];
        }
        else
        {
            //foreach (var item in remotePlayers)
            //{
            //    if (item.GetComponent<ControllerPlayer>().player.player_id == netWork.ID_owner)
            //    {
            //        firtsTurnPlayer = localPlayer.GetComponent<ControllerPlayer>().player;
            //    }
            //}
            isReady = true;
        }
    }
    //KẾT THÚC LƯỢT ĐI
    public void EndTurn()
    {
        switch (direction)
        {
            case EDirection.left:
                preTurnPlayer = currentPlayer;
                currentPlayer = nextTurnPlayer;
                GetPostionPlayer();
                if (postionPlayer == netWork.players.Count - 1)
                {
                    postionPlayer = 0;
                }
                else
                {
                    postionPlayer++;
                    if (postionPlayer >= netWork.players.Count - 1)
                    {
                        postionPlayer = 0;
                    }
                }                
                break;
            case EDirection.right:
                preTurnPlayer = currentPlayer;
                currentPlayer = nextTurnPlayer;
                GetPostionPlayer();
                if (postionPlayer == 0)
                {
                    postionPlayer = netWork.players.Count-1;
                }
                else
                {
                    postionPlayer--;
                    if (postionPlayer ==0)
                    {
                        postionPlayer = netWork.players.Count - 1;                       
                    }
                }               
                break;
            default:
                break;
        }
        nextTurnPlayer = netWork.players[postionPlayer];
    }
    /// <summary>
    /// lây vị trí của player trong mảng
    /// </summary>
    public void GetPostionPlayer()
    {
        int pos = 0;
        foreach (var item in netWork.players)
        {
            if (item.player_id == currentPlayer.player_id)
            {
                postionPlayer = pos;
                break;
            }
            else
            {
                pos++;
                postionPlayer = pos;
            }
        }
    }
}
