using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ControllerRemotePlayer : MonoBehaviour
{
    public Player player;
    public GameObject manager;
    public bool isEmty;
    public bool isReady;
    public TMP_Text UI_name;
    public TMP_Text UI_money;
    public RawImage avartar;
    public TMP_Text ui_message;
    public GameObject child;
    public GameObject ui_text_ready;
    public GameObject ani_turn;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager");      
    }
    /// <summary>
    /// hiển thị trạng thái của lượt đi vừa rùi
    /// </summary>
    /// <param name="text"></param>
    public void SetMessageStatus(string text)
    {
        ui_message.SetText(text);
        ui_message.enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
        if (manager.GetComponent<Controller_NetWork>().ID_owner == player.player_id)
        {
            ui_text_ready.GetComponent<TMP_Text>().SetText("Chủ Phòng");
            ui_text_ready.SetActive(true);
        }
        else
        {
            ui_text_ready.GetComponent<TMP_Text>().SetText("Sẵn Sàng");
            if (isReady)
            {
                ui_text_ready.SetActive(true);
            }
            else
            {
                ui_text_ready.SetActive(false);
            }
        }        
        if (ui_message.enabled)
        {
            time += Time.deltaTime;
            if (time > 2)
            {
                ui_message.enabled = false;
                time = 0;
            }
        }
        else
        {
            time = 0;
        }
        if (player == null || player.player_id == 0 || player == default)
        {
            isEmty = true;
            child.SetActive(false);
        }
        else
        {
            isEmty = false;
            child.SetActive(true);
        }
        if (manager.GetComponent<ManagerGame>().currentPlayer.player_id == player.player_id)
        {
            ani_turn.SetActive(true);
        }
        else
        {
            ani_turn.SetActive(false);
        }
    }
    private void LateUpdate()
    {
        if (player != default)
        {
            UI_name.SetText(player.nickname);
            UI_money.SetText(player.money.ToString());
        }
        else
        {
            UI_name.SetText("????");
            UI_money.SetText("????");
            avartar.texture = null;
        }
    }
    public void GetAvartar()
    {
        string url = InternetConfig.basePath + "/Upload/" + player.avartar;
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
}
