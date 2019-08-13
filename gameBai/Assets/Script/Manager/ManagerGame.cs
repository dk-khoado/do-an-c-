using System.Collections.Generic;
using UnityEngine;

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
    public bool Uno;//sự win :))

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
        DataPackageEndTurn packageEndTurn = new DataPackageEndTurn();
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
                    packageEndTurn = JsonUtility.FromJson<DataPackageEndTurn>(playerModel.message);
                    if (packageEndTurn != null && packageEndTurn.isSkip == false)
                    {
                        currentCard = cardModels[packageEndTurn.currentCard];
                        //Debug.Log(playerModel.message);
                        //Debug.Log(packageEndTurn.currentCard);
                        if (packageEndTurn.onlyColor)
                        {
                            CardModel cardModel = new CardModel();
                            cardModel.number = 99;
                            cardModel.image = currentCard.image;
                            cardModel.amounDraw = currentCard.amounDraw;
                            cardModel.isChucNang = currentCard.isChucNang;
                            switch (packageEndTurn.indexColor)
                            {
                                case 0:
                                    cardModel.color = Color_Card.Red;
                                    break;
                                case 1:
                                    cardModel.color = Color_Card.Blue;
                                    break;
                                case 2:
                                    cardModel.color = Color_Card.Green;
                                    break;
                                case 3:
                                    cardModel.color = Color_Card.Yellow;
                                    break;
                                default:
                                    break;
                            }
                            currentCard = cardModel;
                        }
                        currentPlayer = packageEndTurn.currentPlayer;
                        nextTurnPlayer = packageEndTurn.nextTurnPlayer;
                        preTurnPlayer = packageEndTurn.preTurnPlayer;
                        if (packageEndTurn.direction == 0)
                        {
                            direction = EDirection.left;
                        }
                        else
                        {
                            direction = EDirection.right;
                        }
                        currentPosCard = packageEndTurn.currentPosCard;
                        if (currentCard.isChucNang)
                        {
                            if (packageEndTurn.currentPlayer.player_id == IDLocalPlayer)
                            {                                
                                //foreach (var item in remotePlayers)
                                //{
                                //    if (item.GetComponent<ControllerRemotePlayer>().player.player_id == preTurnPlayer.player_id)
                                //    {
                                //        item.GetComponent<ControllerRemotePlayer>().SetMessageStatus("Bốc Bài");
                                //    }
                                //}
                                ActiveSkill(currentCard);
                            }
                            else
                            {
                                if (currentCard.skill == Skill.Draw)
                                {
                                    foreach (var item in remotePlayers)
                                    {
                                        if (item.GetComponent<ControllerRemotePlayer>().player.player_id == currentPlayer.player_id)
                                        {
                                            item.GetComponent<ControllerRemotePlayer>().SetMessageStatus("Rút Bài");
                                        }
                                    }
                                }                               
                            }
                        }
                    }
                    else if (packageEndTurn.isSkip)
                    {
                        currentPlayer = packageEndTurn.currentPlayer;
                        nextTurnPlayer = packageEndTurn.nextTurnPlayer;
                        preTurnPlayer = packageEndTurn.preTurnPlayer;

                        currentPosCard = packageEndTurn.currentPosCard;

                        if (preTurnPlayer.player_id == IDLocalPlayer)
                        {
                            uI_Manager._message.SetText("Bỏ Lượt");
                        }
                        else
                        {
                            uI_Manager._message.SetText("");
                            foreach (var item in remotePlayers)
                            {
                                if (item.GetComponent<ControllerRemotePlayer>().player.player_id == currentPlayer.player_id)
                                {
                                    item.GetComponent<ControllerRemotePlayer>().SetMessageStatus("Bỏ Lượt");
                                }
                            }
                        }                       
                    }
                    uI_Manager.properties = currentCard;
                }
                catch (System.Exception e)
                {
                    //đây bỏ trống    
                    Debug.Log(e);
                }
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
                                    uI_Manager.ShowWin(item.nickname + " thắng ???");
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
                    
                }
            }
        }
    }
    private void LateUpdate()
    {
        
        if (isPlaying)
        {
            uI_Manager.btnStart.SetActive(false);
            if (netWork.players.Count <2)
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
        else
        {
            uI_Manager.btnStart.SetActive(true);
        }

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
                    Debug.Log("ôi vãi lồn");
                    List<int> temp = new List<int>();
                    temp.Add(currentPosCard[0]);
                    listCards.Add(item.player_id, temp);
                    currentPosCard.RemoveAt(0);
                }
                else
                {
                    Debug.Log("ôi vãi cặc");
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
            temp.GetComponent<ControllerCard>().Properties = cardModels[item];
            temp.GetComponent<ControllerCard>().id = item;
            localPlayer.GetComponent<ControllerPlayer>().AddCard(ModelCard);
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
        playerModel.cmd = "update";
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
            if (selectCard.GetComponent<ControllerCard>().Properties.color == Color_Card.Black)
            {
                currentCard = selectCard.GetComponent<ControllerCard>().Properties;
                //localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                uI_Manager.ShowSelectColor();
                ID_card = id;
            }
            else
            {
                currentCard = selectCard.GetComponent<ControllerCard>().Properties;
                localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                EndTurn(id);
            }
            return;
        }
        if (!selectCard.GetComponent<ControllerCard>())
        {
            return;
        }
        ControllerCard controllerCard = selectCard.GetComponent<ControllerCard>();
        id = selectCard.GetComponent<ControllerCard>().id;
        ID_card = id;
        if (selectCard.GetComponent<ControllerCard>().Properties.color == Color_Card.Black)
        {
            currentCard = selectCard.GetComponent<ControllerCard>().Properties;
            //localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
            uI_Manager.ShowSelectColor();
            ID_card = id;
        }
        else
        {
            if (!controllerCard.Properties.isChucNang)
            {
                if (controllerCard.Properties.number == currentCard.number || selectCard.GetComponent<ControllerCard>().Properties.color == currentCard.color)
                {
                    currentCard = selectCard.GetComponent<ControllerCard>().Properties;
                    localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                    EndTurn(id);
                }
            }
            else
            {
                if (controllerCard.Properties.color == currentCard.color || (controllerCard.Properties.skill == currentCard.skill))
                {
                    currentCard = selectCard.GetComponent<ControllerCard>().Properties;
                    localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                    EndTurn(id);
                }
            }
        }

        //EndTurn(id);
    }
    /// <summary>
    /// chọn màu
    /// </summary>
    /// <param name="color_"></param>
    public void SelectColor(Color_Card color_)
    {
        int index = 0;
        CardModel cardModel = new CardModel();
        switch (color_)
        {
            case Color_Card.Red:
                cardModel.color = Color_Card.Red;
                cardModel.image = currentCard.image;
                cardModel.isChucNang = false;
                cardModel.number = 99;//để người chơi chỉ đánh được màu thui                  
                cardModel.amounDraw = selectCard.GetComponent<ControllerCard>().Properties.amounDraw;
                currentCard = cardModel;
                localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                index = 0;
                break;
            case Color_Card.Blue:
                cardModel.isChucNang = false;
                cardModel.color = Color_Card.Blue;
                cardModel.image = currentCard.image;
                cardModel.number = 99;//để người chơi chỉ đánh được màu thui
                cardModel.amounDraw = selectCard.GetComponent<ControllerCard>().Properties.amounDraw;
                currentCard = cardModel;
                localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                index = 1;
                break;
            case Color_Card.Green:
                cardModel.isChucNang = false;
                cardModel.color = Color_Card.Green;
                cardModel.image = currentCard.image;
                cardModel.number = 99;//để người chơi chỉ đánh được màu thui
                cardModel.amounDraw = selectCard.GetComponent<ControllerCard>().Properties.amounDraw;
                currentCard = cardModel;
                localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                index = 2;
                break;
            case Color_Card.Yellow:
                cardModel.isChucNang = false;
                cardModel.color = Color_Card.Yellow;
                cardModel.image = currentCard.image;
                cardModel.number = 99;//để người chơi chỉ đánh được màu thui
                cardModel.amounDraw = selectCard.GetComponent<ControllerCard>().Properties.amounDraw;
                currentCard = cardModel;
                localPlayer.GetComponent<ControllerPlayer>().Attack(selectCard);
                index = 3;
                break;
            case Color_Card.Black:
                break;
            default:
                break;
        }
        uI_Manager.properties = currentCard;
        EndTurnOnlyCard(index);

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
    /// sản sàng hoặc bắt đầu
    /// </summary>
    public void onReadyOrStart()
    {
        if (lastWinner.player_id != 0)
        {
            currentPlayer = lastWinner;
        }
        if (IDLocalPlayer == netWork.ID_owner && CheckReady() && netWork.players.Count > 1)
        {
            currentPlayer = localPlayer.GetComponent<ControllerPlayer>().player;

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
    /// <summary>
    /// kết thúc lượt mà chỉ đồng bộ màu của lá bài
    /// </summary>
    /// <param name="id"></param>
    public void EndTurnOnlyCard(int indexColor)
    {
        int numberDir = 0;
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
                    if (postionPlayer > netWork.players.Count - 1)
                    {
                        postionPlayer = 0;
                    }
                }
                numberDir = 0;
                break;
            case EDirection.right:
                preTurnPlayer = currentPlayer;
                currentPlayer = nextTurnPlayer;
                GetPostionPlayer();
                if (postionPlayer == 0)
                {
                    postionPlayer = netWork.players.Count - 1;
                }
                else
                {
                    postionPlayer--;
                    if (postionPlayer == 0)
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
        Debug.Log(currentPlayer.player_id);
        DataPackageEndTurn endTurn = new DataPackageEndTurn(currentPlayer, preTurnPlayer, nextTurnPlayer, currentPosCard, ID_card);
        endTurn.onlyColor = true;
        endTurn.indexColor = indexColor;
        endTurn.direction = numberDir;

        PlayerModel playerModel = new PlayerModel();
        playerModel.cmd = "end";
        playerModel.ID_player = IDLocalPlayer;
        playerModel.ID_room = netWork.ID_Room;
        playerModel.message = JsonUtility.ToJson(endTurn);

        Login.connect.Send(playerModel);
        checkWin();
    }
    //KẾT THÚC LƯỢT ĐI
    public void EndTurn(int id)
    {
        if (currentCard.isChucNang && currentCard.skill == Skill.Reverse)
        {
            Player temp = nextTurnPlayer;
            nextTurnPlayer = preTurnPlayer;
            preTurnPlayer = nextTurnPlayer;
            if (direction == EDirection.left)
            {
                direction = EDirection.right;
            }
            else
            {
                direction = EDirection.left;
            }
        }
        int numberDir = 0;
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
                    if (postionPlayer > netWork.players.Count - 1)
                    {
                        postionPlayer = 0;
                    }
                }
                direction = 0;
                break;
            case EDirection.right:
                preTurnPlayer = currentPlayer;
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
        Debug.Log(currentPlayer.player_id);
        DataPackageEndTurn endTurn = new DataPackageEndTurn(currentPlayer, preTurnPlayer, nextTurnPlayer, currentPosCard, id);
        endTurn.direction = numberDir;
        PlayerModel playerModel = new PlayerModel();
        playerModel.cmd = "end";
        playerModel.ID_player = IDLocalPlayer;
        playerModel.ID_room = netWork.ID_Room;
        playerModel.message = JsonUtility.ToJson(endTurn);

        Login.connect.Send(playerModel);
        checkWin();
    }
    //KẾT THÚC LƯỢT ĐI 
    public void EndTurn()
    {
        int numberDir = 0;
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
                    if (postionPlayer > netWork.players.Count - 1)
                    {
                        postionPlayer = 0;
                    }
                }
                numberDir = 0;
                break;
            case EDirection.right:
                preTurnPlayer = currentPlayer;
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
        DataPackageEndTurn endTurn = new DataPackageEndTurn(currentPlayer, preTurnPlayer, nextTurnPlayer, currentPosCard, ID_card);
        endTurn.isSkip = true;
        endTurn.direction = numberDir;
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
                temp.GetComponent<ControllerCard>().Properties = cardModels[currentPosCard[0]];
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
        EndTurn();
    }
    //kích hoạt chức năng của lá bài
    public void ActiveSkill(CardModel cardModel)
    {
        switch (cardModel.skill)
        {
            case Skill.Draw:
                if (cardModel.amounDraw > 0)
                {
                    uI_Manager._message.SetText("Cộng " + cardModel.amounDraw +" lá bài");
                    DrawmCard(cardModel.amounDraw);
                    EndTurn();
                }
                break;
            case Skill.Wild:
                break;
            case Skill.Reverse:
                break;
            case Skill.Skip:
                EndTurn();
                break;
            default:
                break;
        }
    }
    public void UNO()
    {
        Uno = true;
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
        if (localPlayer.GetComponent<ControllerPlayer>().cardsOnHand.Count == 1 && !Uno)
        {
            DrawmCard(1);
        }
    }
}
