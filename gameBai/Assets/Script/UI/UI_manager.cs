using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
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

    public GameObject prefabStatusText;
    public Transform prefabStatusParent;
    public TMP_Text _message;
    public GameObject win_message;


    public TMP_Text UI_name;
    public TMP_Text UI_money;
    public RawImage avartar;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void ShowWin(string text)
    {
        if (win_message)
        {
            //GameObject temp = Instantiate(prefabStatusText, prefabStatusParent,true);
            //temp.GetComponent<TMP_Text>().SetText(text);
            //Destroy(temp, 2);
            win_message.GetComponent<TMP_Text>().SetText(text);
            win_message.SetActive(true);
        }
    }
    public void ShowDataPlayer(Player data)
    {
        UI_name.SetText(data.nickname);
        UI_money.SetText(data.money.ToString());
        string url = InternetConfig.basePath + "/upload/" + data.avartar;
        StartCoroutine(GetRequestDowloadAvartar(url));
    }
    IEnumerator GetRequestDowloadAvartar(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri))
        {
            // Debug.Log(uri);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            // Request and wait for the desired page.

            yield return webRequest.SendWebRequest();

            //yield return new WaitForEndOfFrame();
            if (webRequest.error != null)
            {
                Debug.Log(webRequest.error);
            }
            if (webRequest.isDone)
            {
                yield return new WaitForSeconds(0.1f);
                avartar.texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                //StartCoroutine(GetRequestHoso(BaseURL + "/api/User/Get/" + Login.mnhandata.data.id));
            }

        }
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
                    colorCurrent.color = Color.black;
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
