using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_manager : MonoBehaviour
{
    public CardModel properties;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Image colorCurrent;
    public GameObject UIselectColor;
    public GameObject btnStart;

    public TMP_Text _message;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (properties)
        {
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
        }
        if (properties)
        {
            image.sprite = properties.image;
            switch (properties.color)
            {
                case Color_Card.Red:
                    colorCurrent.color = Color.red;
                    break;
                case Color_Card.Blue:
                    colorCurrent.color = Color.blue;
                    break;
                case Color_Card.Green:
                    colorCurrent.color = Color.green;
                    break;
                case Color_Card.Yellow:
                    colorCurrent.color = Color.yellow;
                    break;
                default:
                    break;
            }
        }
    }

    public void ShowSelectColor()
    {
        UIselectColor.SetActive(true);
    }
    public void CloseSelectColor()
    {
        UIselectColor.SetActive(false);
    }
}
