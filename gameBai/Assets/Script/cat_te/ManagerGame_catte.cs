using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ManagerGame_catte : MonoBehaviour
{
    public int IDLocalPlayer;
    public int AmountCardPerPlayer;
    public GameObject localPlayer;//đói tượng nội bộ      

    public List<GameObject> remotePlayers;//danh sách người chơi trừ người chơi nội bộ
    public GameObject ani;

    public GameObject ModelCard;//đối tượng card sẽ được tạo
    public CardModel13 currentCard;//card hiện tại đang trên bàn 
    public GameObject selectCard;//card đang chọn
    public Player preTurnPlayer;//người chơi trước đó
    public Player nextTurnPlayer;//người chơi kế tiếp
    public EDirection direction;//hướng đi của lượt đánh
    public Player currentPlayer;//người chơi đang đến lượt đi
    public Player lastWinner;//người chiến thắng gần nhất
    public Player currentWinner;//người chiến thắng của round
    public int postionPlayer;

    public List<CardModel13> cardModels = new List<CardModel13>();
    public List<int> currentPosCard = new List<int>();

    UI_manager uI_Manager;
    Controller_NetWork netWork;

    public bool isReady;//thuộc tính chỉ xuất hiện khi đối tượng k phải chủ phòng
    public bool isPlaying;//true nếu đang chơi. false nếu đang chờ
    public bool Uno;//sự win :))
    public bool isguest;//sự chờ đợi :((
    public int  score;

    private DataReadyOrStart dataReady;
    [SerializeField]
    private int ID_card;
    void Start()
    {

        uI_Manager = GetComponent<UI_manager>();
        netWork = GetComponent<Controller_NetWork>();
        LoadCard();
        //XaoBai();
        FindPostionRemotePlayer();
        IDLocalPlayer = Login.mnhandata.data.id;
    }
    private void FixedUpdate()
    {
        if (currentPlayer.player_id == IDLocalPlayer)
        {
            uI_Manager._message.SetText("");
        }
        UServer uServerStart = Login.connect.GetUServer("start_game");
        if (uServerStart.value != "" && uServerStart.value != null && uServerStart.isNew)
        {
            PlayerModel playerModel = new PlayerModel();
            try
            {
                playerModel = JsonUtility.FromJson<PlayerModel>(uServerStart.value);
                //Debug.Log(playerModel.message);
                dataReady = JsonUtility.FromJson<DataReadyOrStart>(playerModel.message);

            }
            catch
            {
                //đây bỏ trống
            }
            if (dataReady != null)
            {
                if (dataReady.isStart && !isPlaying)
                {
                    isPlaying = dataReady.isPlaying;
                    currentPlayer = dataReady.currentPlayer;
                    nextTurnPlayer = dataReady.nextTurnPlayer;
                    preTurnPlayer = dataReady.preTurnPlayer;
                    foreach (var item in dataReady.listCards)
                    {
                        if (item.IDplayer == IDLocalPlayer)
                        {
                            spawmBai(item.card);
                        }
                    }
                }
            }
        }
        //update trạng thái của player
        UServer uServer = Login.connect.GetUServer("ready");
        if (uServer.value != "" && uServer.value != null && uServer.isNew)
        {
            PlayerModel playerModel = new PlayerModel();
            try
            {
                playerModel = JsonUtility.FromJson<PlayerModel>(uServer.value);
                //Debug.Log(playerModel.message);
                dataReady = JsonUtility.FromJson<DataReadyOrStart>(playerModel.message);

            }
            catch
            {
                //đây bỏ trống
            }
            if (dataReady != null)
            {
                if (dataReady.isStart && !isPlaying)
                {
                    isPlaying = dataReady.isPlaying;
                    currentPlayer = dataReady.currentPlayer;
                    nextTurnPlayer = dataReady.nextTurnPlayer;
                    preTurnPlayer = dataReady.preTurnPlayer;
                    foreach (var item in dataReady.listCards)
                    {
                        if (item.IDplayer == IDLocalPlayer)
                        {
                            spawmBai(item.card);
                        }
                    }
                }
                else
                {
                    //Debug.Log(uServer.ID);
                    foreach (var item in remotePlayers)
                    {
                        if (item.GetComponent<ControllerRemotePlayer>().player.player_id == playerModel.ID_player)
                        {
                            item.GetComponent<ControllerRemotePlayer>().isReady = dataReady.isReady;
                        }
                    }
                }
            }
        }
        //update lượt đi của player
        DataPackageEndTurnCatte packageEndTurn = new DataPackageEndTurnCatte();
        UServer uServerv2 = Login.connect.GetUServer("end");
        if (uServerv2 != null)
        {
            PlayerModel playerModel = new PlayerModel();
            if (uServerv2.value != "" && uServerv2.value != null && uServerv2.isNew)
            {
                try
                {
                    playerModel = JsonUtility.FromJson<PlayerModel>(uServerv2.value);
                    //Debug.Log(uServerv2.value);
                    packageEndTurn = JsonUtility.FromJson<DataPackageEndTurnCatte>(playerModel.message);
                    if (packageEndTurn != null && packageEndTurn.isSkip == false)
                    {                        
                        currentCard = cardModels[packageEndTurn.currentCard];
                        currentPlayer = packageEndTurn.currentPlayer;
                        nextTurnPlayer = packageEndTurn.nextTurnPlayer;
                        preTurnPlayer = packageEndTurn.preTurnPlayer;
                        currentWinner = packageEndTurn.currentWinner;
                    }
                    if (packageEndTurn.endRound == true)
                    {
                        newRound();
                    }
                    //nếu tới lượt người chơi cuối cùng thì                  
                }
                catch (System.Exception)
                {
                     //địt mẹ
                }
                uI_Manager.properties_V13 = currentCard;
            }
        }
        //update trạng thái thắng
        UServer uServerv3 = Login.connect.GetUServer("win");
        if (uServerv3 != null)
        {
            PlayerModel playerModel = new PlayerModel();
            if (uServerv3.value != "" && uServerv3.value != null && uServerv3.isNew)
            {
                try
                {
                    playerModel = JsonUtility.FromJson<PlayerModel>(uServerv3.value);
                    if (bool.Parse(playerModel.message))
                    {
                        foreach (var item in netWork.players)
                        {
                            try
                            {
                                if (item.player_id == playerModel.ID_player)
                                {
                                    lastWinner = item;
                                    uI_Manager.ShowWin(item.nickname.ToString() + " thắng");
                                    break;
                                }
                            }
                            catch (System.Exception e)
                            {
                                Debug.LogError(e);
                            }
                        }
                        NewGame();
                    }
                }
                catch (System.Exception e)
                {
                    //đây bỏ trống nhé
                }
            }
        }      
    }
    private void LateUpdate()
    {
        if (ani)
        {
            if (isPlaying)
            {
                ani.SetActive(true);
            }
            else
            {
                ani.SetActive(false);
            }
        }
        if (ani.GetComponent<Animator>())
        {
            switch (direction)
            {
                case EDirection.left:
                    ani.GetComponent<Animator>().SetInteger("direction", 0);
                    break;
                case EDirection.right:
                    ani.GetComponent<Animator>().SetInteger("direction", 1);
                    break;
                default:
                    break;
            }
        }
        UServer uServerUpdate = Login.connect.GetUServer("update");
        if (uServerUpdate.value != "" && uServerUpdate.value != null && uServerUpdate.isNew)
        {
            netWork.UpdatePlayer();
            PlayerUpdateMoneyModel playerModel = new PlayerUpdateMoneyModel();
            try
            {
                //Debug.Log(uServerUpdate.value);
                playerModel = JsonUtility.FromJson<PlayerUpdateMoneyModel>(uServerUpdate.value);
                //Debug.Log(playerModel.message.Length);
            }
            catch
            {
                //đây bỏ trống
            }
            if (playerModel != null)
            {
                foreach (var item in playerModel.message)
                {
                    if (item.ID_player == IDLocalPlayer)
                    {
                        uI_Manager.UpdateMoney(item.money.ToString());
                    }
                    else
                    {
                        foreach (var remotePlayer in remotePlayers)
                        {
                            if (remotePlayer.GetComponent<ControllerRemotePlayer>().player.player_id == item.ID_player)
                            {
                                remotePlayer.GetComponent<ControllerRemotePlayer>().player.money = item.money;
                                break;
                            }
                        }
                    }
                }
            }
        }


        UServer uServerEnd = Login.connect.GetUServer("end_game");
        if (uServerEnd.value != "" && uServerEnd.value != null && uServerEnd.isNew)
        {
            NewGame();
            netWork.UpdatePlayer();
        }
        if (isPlaying)
        {
            uI_Manager.btnStart.SetActive(false);
            if (netWork.players.Count < 2)
            {
                //PlayerModel playerModel = new PlayerModel();
                //playerModel.ID_player = IDLocalPlayer;
                //playerModel.ID_room = netWork.ID_Room;
                //playerModel.cmd = "win";
                //playerModel.message = "true";
                //Login.connect.Send(playerModel);

                //uI_Manager.ShowWin("Bạn thắng");
            }
        }
        else
        {
            uI_Manager.btnStart.SetActive(true);
        }

    }

    /*                _..gggggppppp.._                       
                      _.gd$$$$$$$$$$$$$$$$$$bp._                  
                   .g$$$$$$P^^""j$$b""""^^T$$$$$$p.               
                .g$$$P^T$$b    d$P T;       ""^^T$$$p.            
              .d$$P^"  :$; `  :$;                "^T$$b.          
            .d$$P'      T$b.   T$b                  `T$$b.        
           d$$P'      .gg$$$$bpd$$$p.d$bpp.           `T$$b       
          d$$P      .d$$$$$$$$$$$$$$$$$$$$bp.           T$$b      
         d$$P      d$$$$$$$$$$$$$$$$$$$$$$$$$b.          T$$b     
        d$$P      d$$$$$$$$$$$$$$$$$$P^^T$$$$P            T$$b    
       d$$P    '-'T$$$$$$$$$$$$$$$$$$bggpd$$$$b.           T$$b   
      :$$$      .d$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$p._.g.     $$$;  
      $$$;     d$$$$$$$$$$$$$$$$$$$$$$$P^"^T$$$$P^^T$$$;    :$$$  
     :$$$     :$$$$$$$$$$$$$$:$$$$$$$$$_    "^T$bpd$$$$,     $$$; 
     $$$;     :$$$$$$$$$$$$$$bT$$$$$P^^T$p.    `T$$$$$$;     :$$$ 
    :$$$      :$$$$$$$$$$$$$$P `^^^'    "^T$p.    lb`TP       $$$;
    :$$$      $$$$$$$$$$$$$$$              `T$$p._;$b         $$$;
    $$$;      $$$$$$$$$$$$$$;                `T$$$$:Tb        :$$$
    $$$;      $$$$$$$$$$$$$$$                        Tb    _  :$$$
    :$$$     d$$$$$$$$$$$$$$$.                        $b.__Tb $$$;
    :$$$  .g$$$$$$$$$$$$$$$$$$$p...______...gp._      :$`^^^' $$$;
     $$$;  `^^'T$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$p.    Tb._, :$$$ 
     :$$$       T$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$b.   "^"  $$$; 
      $$$;       `$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$b      :$$$  
      :$$$        $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$;     $$$;  
       T$$b    _  :$$`$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$;   d$$P   
        T$$b   T$g$$; :$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$  d$$P    
         T$$b   `^^'  :$$ "^T$$$$$$$$$$$$$$$$$$$$$$$$$$$ d$$P     
          T$$b        $P     T$$$$$$$$$$$$$$$$$$$$$$$$$;d$$P      
           T$$b.      '       $$$$$$$$$$$$$$$$$$$$$$$$$$$$P       
            `T$$$p.   bug    d$$$$$$$$$$$$$$$$$$$$$$$$$$P'        
              `T$$$$p..__..g$$$$$$$$$$$$$$$$$$$$$$$$$$P'          
                "^$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$^"            
                   "^T$$$$$$$$$$$$$$$$$$$$$$$$$$P^"               
                       """^^^T$$$$$$$$$$P^^^"""*/
    //tải tài nguyên các lá bài
    void LoadCard()
    {
        CardModel13[] temp = Resources.LoadAll<CardModel13>("Properties/");
        foreach (var item in temp)
        {
            cardModels.Add(item);
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
    public List<DicListcard> Chiabai()
    {

        XaoBai();
        Dictionary<int, List<int>> listCards = new Dictionary<int, List<int>>();
        int sum = netWork.players.Count * AmountCardPerPlayer;
        if (currentPosCard.Count - sum < 0)
        {
            return null;
        }
        for (int i = 0; i < AmountCardPerPlayer; i++)
        {
            foreach (var item in netWork.players)
            {
                if (!listCards.ContainsKey(item.player_id))
                {
                    //Debug.Log("ôi vãi lồn");
                    List<int> temp = new List<int>();
                    temp.Add(currentPosCard[0]);
                    listCards.Add(item.player_id, temp);
                    currentPosCard.RemoveAt(0);
                }
                else
                {
                    // Debug.Log("ôi vãi cặc");
                    listCards[item.player_id].Add(currentPosCard[0]);
                    currentPosCard.RemoveAt(0);
                }
            }
        }
        List<DicListcard> result = new List<DicListcard>();
        foreach (var item in listCards)
        {
            result.Add(new DicListcard(item.Key, item.Value));
        }
        return result;
    }
    public void spawmBai(List<int> listcard)
    {
        foreach (var item in listcard)
        {
            GameObject temp = ModelCard;
            temp.GetComponent<ControllerCard>().Properties_V13 = cardModels[item];
            temp.GetComponent<ControllerCard>().id = item;
            localPlayer.GetComponent<ControllerPlayer>().AddCard(ModelCard);
        }
    }
    public void NewGame()
    {
        ani.GetComponent<Image>().color = Color.white;
        currentCard = null;
        uI_Manager.properties = null;
        XaoBai();
        if (localPlayer.GetComponent<ControllerPlayer>())
        {
            localPlayer.GetComponent<ControllerPlayer>().RemoveAllCard();
        }
        isPlaying = false;
        uI_Manager.btnStart.SetActive(true);
        uI_Manager.UIselectColor.SetActive(false);
        foreach (var item in remotePlayers)
        {
            item.GetComponent<ControllerRemotePlayer>().isReady = false;
        }
        isReady = false;
        DataReadyOrStart data = new DataReadyOrStart();
        data.isReady = isReady;

        PlayerModel playerModel = new PlayerModel();
        playerModel.ID_player = IDLocalPlayer;
        playerModel.ID_room = netWork.ID_Room;
        playerModel.cmd = "ready";
        playerModel.message = JsonUtility.ToJson(data);

        Login.connect.Send(playerModel);
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
        int id = 0;
        if (currentPlayer.player_id != IDLocalPlayer)
        {
            return;
        }
        if (!currentCard)
        {
            id = selectCard.GetComponent<ControllerCard>().id;
            ID_card = id;
            currentCard = selectCard.GetComponent<ControllerCard>().Properties_V13;
            localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
            currentWinner = localPlayer.GetComponent<ControllerPlayer>().player;
            EndTurn(false);
        }
        else
        {
            id = selectCard.GetComponent<ControllerCard>().id;
            ID_card = id;            
            if (selectCard.GetComponent<ControllerCard>().Properties_V13.chat_Bai == currentCard.chat_Bai)
            {
                if (selectCard.GetComponent<ControllerCard>().Properties_V13.number > currentCard.number || selectCard.GetComponent<ControllerCard>().Properties_V13.kihieu > currentCard.kihieu)
                {
                    localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                    currentWinner = localPlayer.GetComponent<ControllerPlayer>().player;
                    currentCard = selectCard.GetComponent<ControllerCard>().Properties_V13;
                    EndTurn(false);                    
                }
            }
            else
            {
                localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                EndTurn(true);
            }
           
        }      
        
    }
    /// <summary>
    /// chọn màu
    /// </summary>
    /// <param name="color_"></param>
    public void SelectColor(Color_Card color_)
    {       

    }
    /// <summary>
    /// kiểm tra tất cả người chơi có sản sàng
    /// </summary>
    public bool CheckReady()
    {
        foreach (var item in remotePlayers)
        {
            if (!item.GetComponent<ControllerRemotePlayer>().isEmty && !item.GetComponent<ControllerRemotePlayer>().isReady)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// bắt đầu vòng mới
    /// </summary>
    public void newRound()
    {
        Debug.Log("vòng mới");
        currentPlayer = currentWinner;
        int pos = 0;
        GetPostionPlayer(); 
        if (postionPlayer < netWork.players.Count - 1)
        {
            pos = postionPlayer + 1;
        }
        nextTurnPlayer = netWork.players[pos];
        if (postionPlayer == 0)
        {
            pos = netWork.players.Count - 1;
        }
        else
        {
            pos = postionPlayer - 1;
        }
        preTurnPlayer = netWork.players[pos];
        currentCard = null;
        selectCard = null;
    }
    /// <summary>
    /// sản sàng hoặc bắt đầu
    /// </summary>
    public void onReadyOrStart()
    {
        if (IDLocalPlayer == netWork.ID_owner && CheckReady() && netWork.players.Count > 0)
        {
            if (lastWinner.player_id != 0)
            {
                currentPlayer = lastWinner;
            }
            else
            {
                currentPlayer = localPlayer.GetComponent<ControllerPlayer>().player;

            }
            uI_Manager.btnStart.SetActive(false);

            int pos = 0;
            GetPostionPlayer();
            currentPlayer = netWork.players[postionPlayer];
            if (postionPlayer < netWork.players.Count - 1)
            {
                pos = postionPlayer + 1;
            }
            nextTurnPlayer = netWork.players[pos];
            if (postionPlayer == 0)
            {
                pos = netWork.players.Count - 1;
            }
            else
            {
                pos = postionPlayer - 1;
            }
            preTurnPlayer = netWork.players[pos];
            NewGame();
            List<DicListcard> listCards = Chiabai();
            DataReadyOrStart data = new DataReadyOrStart();
            data.currentPlayer = currentPlayer;
            data.nextTurnPlayer = nextTurnPlayer;
            data.preTurnPlayer = preTurnPlayer;
            data.isPlaying = true;
            data.isStart = true;
            data.listCards = listCards;
            data.currentPosCard = currentPosCard;

            PlayerModel playerModel = new PlayerModel();
            playerModel.ID_player = IDLocalPlayer;
            playerModel.ID_room = netWork.ID_Room;
            playerModel.cmd = "start_game";
            playerModel.message = JsonUtility.ToJson(data);

            Login.connect.Send(playerModel);

            //Debug.Log(listCards[IDLocalPlayer].Count);
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
            isReady = !isReady;
            DataReadyOrStart data = new DataReadyOrStart();
            data.isReady = isReady;

            PlayerModel playerModel = new PlayerModel();
            playerModel.ID_player = IDLocalPlayer;
            playerModel.ID_room = netWork.ID_Room;
            playerModel.cmd = "ready";
            playerModel.message = JsonUtility.ToJson(data);

            Login.connect.Send(playerModel);

        }

    }
    //KẾT THÚC LƯỢT ĐI 
    public void EndTurn(bool skip)
    {
        bool isEndTurn = false;
        int numberDir = 0;
        if (currentPlayer.player_id == preTurnPlayer.player_id)
        {
            isEndTurn = true;
        }
        switch (direction)
        {
            case EDirection.left:
                //preTurnPlayer = currentPlayer;
                currentPlayer = nextTurnPlayer;
                GetPostionPlayer();
                if (postionPlayer == netWork.players.Count - 1)
                {
                    postionPlayer = 0;
                }
                else
                {
                    postionPlayer++;
                    if (postionPlayer > netWork.players.Count - 1)
                    {
                        postionPlayer = 0;
                    }
                }
                numberDir = 0;
                break;
            case EDirection.right:
                //preTurnPlayer = currentPlayer;
                currentPlayer = nextTurnPlayer;
                GetPostionPlayer();
                if (postionPlayer == 0)
                {
                    postionPlayer = netWork.players.Count - 1;
                }
                else
                {
                    postionPlayer--;
                    if (postionPlayer < 0)
                    {
                        postionPlayer = netWork.players.Count - 1;
                    }
                }
                numberDir = 1;
                break;
            default:
                break;
        }
        nextTurnPlayer = netWork.players[postionPlayer];
        //Debug.Log(currentPlayer.player_id);
        DataPackageEndTurnCatte endTurn = new DataPackageEndTurnCatte(currentPlayer, preTurnPlayer, nextTurnPlayer, currentPosCard, ID_card);        
        endTurn.direction = numberDir;
        endTurn.currentWinner = currentWinner;
        endTurn.isSkip = skip;
        if (isEndTurn)
        {
            endTurn.endRound = true;
        }
        PlayerModel playerModel = new PlayerModel();
        playerModel.cmd = "end";
        playerModel.ID_player = IDLocalPlayer;
        playerModel.ID_room = netWork.ID_Room;
        playerModel.message = JsonUtility.ToJson(endTurn);

        Login.connect.Send(playerModel);
        checkWin();
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

    public void DrawmCard(int amount)
    {
        if (currentPosCard.Count > 0 && currentPosCard.Count >= amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject temp = ModelCard;
                temp.GetComponent<ControllerCard>().Properties_V13 = cardModels[currentPosCard[0]];
                temp.GetComponent<ControllerCard>().id = currentPosCard[0];
                localPlayer.GetComponent<ControllerPlayer>().AddCard(temp);
                currentPosCard.RemoveAt(0);
            }
        }
    }
    //bỏ lượt
    public void SkipTurn()
    {
        if (currentPlayer.player_id != IDLocalPlayer)
        {
            return;
        }
        DrawmCard(1);
        EndTurn(true);
    }
    /// <summary>
    /// kiểm tra chiến thắng
    /// </summary>
    public void checkWin()
    {
        if (localPlayer.GetComponent<ControllerPlayer>().cardsOnHand.Count < 1)
        {
            PlayerModel playerModel = new PlayerModel();
            playerModel.ID_player = IDLocalPlayer;
            playerModel.ID_room = netWork.ID_Room;
            playerModel.cmd = "win";
            playerModel.message = "true";
            Login.connect.Send(playerModel);

            uI_Manager.ShowWin("Bạn thắng");
        }             
    }
}

