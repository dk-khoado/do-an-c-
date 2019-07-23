using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(UI_manager))]
public class ManagerGame : MonoBehaviour
{
    public int AmountCardPerPlayer;
    public GameObject localPlayer;
    public List<GameObject> remotePlayers;
    private Dictionary<int, GameObject> infoPlayer = new Dictionary<int, GameObject>();
    public CardModel currentCard;
    public GameObject selectCard;
    public List<CardModel> cardModels = new List<CardModel>();
    [SerializeField]
    private GameObject ModelCard;
    public List<int> currentPosCard = new List<int>();
    UI_manager uI_Manager;
    // Start is called before the first frame update
    void Start()
    {
        uI_Manager = GetComponent<UI_manager>();
        LoadCard();
        //XaoBai();
        FindPostionRemotePlayer();
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
                if (item.GetComponent<ControllerPlayer>())
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
}
