using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Text;

public class ChatBoxFriend : MonoBehaviour
{
    public GameObject item_message;
    public Transform _content;
    public TMP_Text mName;
    public TMP_InputField input_message;
    public int id_send;
    public int id_recive;
    public string nameFriend;
    private List<GameObject> dataMessages = new List<GameObject>();
    void Start()
    {

    }
    private void FindAllMessage()
    {
        dataMessages.Clear();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("message_content");
        foreach (var item in temp)
        {
            dataMessages.Add(item);
        }
    }
    private void FindAllMessageAndRemove()
    {
        dataMessages.Clear();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("message_content");
        foreach (var item in temp)
        {
            Destroy(item);
        }
    }
    //kiểm tra tin nhắn đã xuất hiện hay chưa
    private bool hasShow(string date)
    {
        foreach (var item in dataMessages)
        {
            if (item.GetComponent<DataMessage>().data.send_date == date)
            {
                return true;
            }
        }
        return false;
    }
    public void Show(int id_send, int id_recive)
    {
        if (this.id_recive != id_recive)
        {
            FindAllMessageAndRemove();
            this.id_send = id_send;
            this.id_recive = id_recive;
            StartCoroutine(LoadChatBox(InternetConfig.basePath + "/api/Chat/Get/" + id_send));
            StartCoroutine(GetNamePlayer(InternetConfig.basePath + "/api/User/Get/" + id_recive));
        }
    }

    // Update is called once per frame
    [System.Obsolete]
    void FixedUpdate()
    {
        if (mName)
        {
            mName.text = nameFriend;
        }
        if (!gameObject.active)
        {
            return;
        }
        if (input_message && input_message.isFocused && input_message.text.Trim() != "" && Input.GetKey(KeyCode.Return))
        {            
            string data = "{\"message\": \"" + input_message.text + "\", \"id_send\": " + id_send + ",\"id_recive\": " + id_recive + "}";
            Debug.Log(data);
            StartCoroutine(SendMessager(InternetConfig.basePath + "/api/Chat/SendMessage", data));
            input_message.text = "";
            PlayerModel playerModel = new PlayerModel();
            playerModel.cmd = "lby_chatbox";
            playerModel.ID_player = id_send;
            playerModel.ID_room = 0;
            playerModel.message = "true";

            Login.connect.Send(playerModel);
        }
        UServer uServerStart = Login.connect.GetUServer("lby_chatbox");
        if (uServerStart.value != "" && uServerStart.value != null && uServerStart.isNew)
        {
            PlayerModel playerModel = new PlayerModel();
            try
            {
                playerModel = JsonUtility.FromJson<PlayerModel>(uServerStart.value);
                if (playerModel.ID_player == id_recive)
                {
                    //Debug.Log(playerModel.message);               
                    if (bool.Parse(playerModel.message))
                    {
                        StartCoroutine(LoadChatBox(InternetConfig.basePath + "/api/Chat/Get/" + id_send));
                    }
                }                
            }
            catch
            {
                //đây bỏ trống
            }
        }
    }

    IEnumerator LoadChatBox(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("apikey", "123456789");
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();
            if (webRequest.error != null || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                if (webRequest.isDone)
                {
                    string data = "{\"data\":" + webRequest.downloadHandler.text + "}";
                    //Debug.Log(data);
                    MessageItemRequest messageItem = JsonUtility.FromJson<MessageItemRequest>(data);
                    yield return webRequest.downloadProgress;
                    if (webRequest.downloadHandler.isDone)
                    {
                        //FindAllMessage();
                        yield return null;
                        FindAllMessage();
                        //Debug.Log(messageItem.data.Count);
                        foreach (var item in messageItem.data)
                        {
                            if ((item.player_id_send == id_send && item.player_id_receive == id_recive) || (item.player_id_send == id_recive && item.player_id_receive == id_send))
                            {
                                if (!item.isdelete)
                                {
                                    if (!hasShow(item.send_date))
                                    {
                                        GameObject temp = item_message;
                                        temp.GetComponent<DataMessage>().data = item;
                                        Instantiate(temp, _content);
                                    }
                                }
                            }
                        }
                        yield return null;
                    }
                }
            }
        }
    }

    IEnumerator SendMessager(string url, string data)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, data))
        {
            webRequest.SetRequestHeader("apikey", "123456789");
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
            yield return webRequest.SendWebRequest();
            if (webRequest.error != null || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                if (webRequest.isDone)
                {
                    yield return null;
                    Debug.Log(webRequest.downloadHandler.text);
                    StartCoroutine(LoadChatBox(InternetConfig.basePath + "/api/Chat/Get/" + id_send));
                    yield return null;
                }
            }
        }
    }
    IEnumerator GetNamePlayer(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("apikey", "123456789");
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            if (webRequest.error != null || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                if (webRequest.isDone)
                {
                    DataFromLogin player = JsonUtility.FromJson<DataFromLogin>(webRequest.downloadHandler.text);
                    nameFriend = player.data.nickname;
                    StartCoroutine(LoadChatBox(InternetConfig.basePath + "/api/Chat/Get/" + id_send));
                }
            }
        }
    }
}
