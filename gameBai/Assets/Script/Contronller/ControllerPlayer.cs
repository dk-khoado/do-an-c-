using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControllerPlayer : MonoBehaviour
{
    public Player player;
    public bool isLocalPlayer;

    public float turnTime = 15;
    public float currentTime;

    public TMP_Text ui_time;  

    public List<GameObject> cardsOnHand = new List<GameObject>();
    public GameObject manager;
    public bool selectColor;
    [SerializeField]
    private GameObject child;
    //public bool isEmty;
    public bool isTurn;
    //public GameObject UI_select;
    private void Start()
    {
        manager = GameObject.Find("Manager");
        currentTime = turnTime;
    }
    private void LateUpdate()
    {
        if (player.player_id == manager.GetComponent<ManagerGame>().currentPlayer.player_id)
        {
            isTurn = true;
            ui_time.enabled = true;
            currentTime -= Time.deltaTime;
            if (currentTime < 0 && manager.GetComponent<ManagerGame>().isPlaying && isTurn)
            {
                manager.GetComponent<ManagerGame>().DrawmCard(1);
                manager.GetComponent<ManagerGame>().EndTurn();
                isTurn = false;
            }
            
            if (ui_time)
            {
                ui_time.SetText(currentTime.ToString("f0"));
            }
        }
        else
        {
            ui_time.enabled = false;
            currentTime = turnTime;
            isTurn = false;
        }

    }
    /// <summary>
    /// tạo card thui mà
    /// </summary>
    /// <param name="card">bỏ lồn vào đây</param>
    public void AddCard(GameObject card)
    {
        if (!isLocalPlayer)
        {

            return;
        }
        GameObject temp = Instantiate(card, gameObject.transform);
        cardsOnHand.Add(temp);
    }
    public void RemoveAllCard()
    {
        foreach (var item in cardsOnHand)
        {
            Destroy(item);
        }
        cardsOnHand.Clear();
    }
    public void Attack(GameObject card)
    {
        if (card == null)
        {
            return;
        }

        foreach (var item in cardsOnHand)
        {
            //ControllerCard controllerCard;
            if (item.GetInstanceID() == card.GetInstanceID())
            {

                Destroy(item);
                cardsOnHand.Remove(item);

                //card = null;
                return;
            }
        }
        //cardsOnHand.Remove(card);
        //Destroy(card);
    }
    public void Selected(int index)
    {
        switch (index)
        {
            case 0:
                manager.GetComponent<ManagerGame>().SelectColor(Color_Card.Red);
                break;
            case 1:
                manager.GetComponent<ManagerGame>().SelectColor(Color_Card.Blue);
                break;
            case 2:
                manager.GetComponent<ManagerGame>().SelectColor(Color_Card.Green);
                break;
            case 3:
                manager.GetComponent<ManagerGame>().SelectColor(Color_Card.Yellow);
                break;
            default:
                break;
        }
        manager.GetComponent<UI_manager>().CloseSelectColor();
    }
}
