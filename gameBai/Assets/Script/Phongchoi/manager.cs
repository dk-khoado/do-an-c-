using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour
{
    public int amountCard;
    public int repeat;
    public List<cardModel> cardModels = new List<cardModel>();
    public GameObject parent;
    public GameObject card;
    void Start()
    {
        cardModel[] cards = Resources.LoadAll<cardModel>("uno/");
        foreach (var item in cardModels)
        {
            card.GetComponent<ControllerCarduno>().ThuocTinh = item;
            Instantiate(card, parent.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
