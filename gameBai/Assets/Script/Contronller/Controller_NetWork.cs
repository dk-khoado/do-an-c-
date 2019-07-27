using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Controller_NetWork : MonoBehaviour
{
    public string URL_GETPlayerList;
    public string address;
    public int ID_owner;
    public int ID_Room;
    public List<Player> players = new List<Player>();
    [SerializeField]
    private ManagerGame manager;
    private UI_manager uI_Manager;
    private void Awake()
    {
        manager = GetComponent<ManagerGame>();
        uI_Manager = GetComponent<UI_manager>();

    }
    void Start()
    {
        ID_Room = PlayerPrefs.GetInt("id_room");
        ID_owner = PlayerPrefs.GetInt("id_owner");
        StartCoroutine(GetInfoPlayer());
    }
    void Update()
    {
        if (PlayerPrefs.GetInt("id") == ID_owner)
        {
            uI_Manager.btnStart.GetComponentInChildren<TMP_Text>().SetText("Bắt Đầu");
        }
        else
        {
            uI_Manager.btnStart.GetComponentInChildren<TMP_Text>().SetText("Sẳn sàng");
        }
    }
    //láy danh sách người chơi trong phòng
    IEnumerator GetPlayerRoom()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(URL_GETPlayerList))
        {
            yield return webRequest.SendWebRequest();
            
        }
    }
    IEnumerator GetInfoPlayer()
    {
        string url = address + "/api/RoomManager/GetPlayerInRoom?ID_room="+ID_Room;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {            
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                if (webRequest.isDone)
                {
                    //string json = "{\"key\":" + webRequest.downloadHandler.text + "}";
                    DataRoomPlayer data= JsonUtility.FromJson<DataRoomPlayer>(webRequest.downloadHandler.text);
                    players = data.data;
                    SetDataPlayer();
                }
            }
        }
    }
    /// <summary>
    /// cập nhật dữ liệu của người chơi
    /// </summary>
    public void SetDataPlayer()
    {
        int i = 0;
        foreach (var player in players)
        {
            if (player.player_id == manager.IDLocalPlayer)
            {
                manager.localPlayer.GetComponent<ControllerPlayer>().player = player;
            }
            else
            {
                manager.remotePlayers[i].GetComponent<ControllerPlayer>().player = player;
                i++;
            }
        }
    }
}
