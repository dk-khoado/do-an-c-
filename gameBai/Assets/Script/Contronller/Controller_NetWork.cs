using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Controller_NetWork : MonoBehaviour
{
    public string URL_GETPlayerList;
    //public string InternetConfig.basePath;
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
        ID_Room = InternetConfig.ID_Room;
        //ID_owner = PlayerPrefs.GetInt("id_owner");
        StartCoroutine(GetInfoPlayer());
    }
    void Update()
    {        
        if (Login.mnhandata.data.id == ID_owner)
        {
            uI_Manager.btnStart.GetComponentInChildren<TMP_Text>().SetText("Bắt Đầu");
        }
        else
        {
            if (manager.isReady)
            {
                uI_Manager.btnStart.GetComponentInChildren<TMP_Text>().SetText("Bỏ Sẵn sàng");
            }
            else
            {
                uI_Manager.btnStart.GetComponentInChildren<TMP_Text>().SetText("Sẵn sàng");
            }
        }
    }
    private void LateUpdate()
    {
        if (Login.connect.isNew)
        {
            UServer data = Login.connect.GetUServer("join_room");
            if (data.value != "" && data.isNew)
            {

                StartCoroutine(GetInfoPlayer());
                Debug.Log(data.value + "đã tham gia phòng");
            }
            data = Login.connect.GetUServer("leave_room");
            if (data.value != "" && data.isNew)
            {
                PlayerModel player = JsonUtility.FromJson<PlayerModel>(data.value);
                if (player != default)
                {
                    RemoteDataPlayer(player.ID_player);
                }
                StartCoroutine(GetInfoPlayer());
                Debug.Log(data.value + "đã rời phòng");
            }
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
        string url = InternetConfig.basePath + "/api/RoomManager/GetPlayerInRoom?ID_room=" + ID_Room;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("apikey","123456789");
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
                StartCoroutine(GetInfoPlayer());
            }
            else
            {
                if (webRequest.isDone)
                {
                    //string json = "{\"key\":" + webRequest.downloadHandler.text + "}";
                    DataRoomPlayer data = JsonUtility.FromJson<DataRoomPlayer>(webRequest.downloadHandler.text);
                    Debug.Log(webRequest.downloadHandler.text);
                    players = data.data;
                    ID_owner = data.result;
                    SetDataPlayer();
                }
            }
        }
    }
    IEnumerator APILeaveRoom(WWWForm form)
    {
        string url = InternetConfig.basePath + "/api/RoomManager/LeaveRoom";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            webRequest.SetRequestHeader("apikey", "123456789");
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                if (webRequest.isDone)
                {

                    //Debug.Log(webRequest.downloadHandler.text);
                    PlayerModel player = new PlayerModel();
                    player.ID_player = Login.mnhandata.data.id;
                    player.ID_room = ID_Room;
                    player.cmd = "leave_room";
                    Login.connect.Send(player);
                    SceneManager.LoadScene("Lobby");
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
        int pos=0;
        bool findLCP = false;//đã tìm tháy vị trí localplayer
        foreach (var player in players)
        {
            if (player.player_id == manager.IDLocalPlayer)
            {
                manager.localPlayer.GetComponent<ControllerPlayer>().player = player;
                uI_Manager.GetComponent<UI_manager>().ShowDataPlayer(player);                                
                findLCP = true;
                pos = i;
                if (manager.localPlayer.GetComponent<ControllerPlayer>().player.player_id == ID_owner)
                {
                    pos = 0;
                }
            }           
            else
            {
                if (pos==0)
                {
                    manager.remotePlayers[i].GetComponent<ControllerRemotePlayer>().player = player;
                    manager.remotePlayers[i].GetComponent<ControllerRemotePlayer>().GetAvartar();
                    i++;
                }
                else
                {
                    int sum = i - pos;
                    Player temp = manager.remotePlayers[sum-1].GetComponent<ControllerRemotePlayer>().player;
                    manager.remotePlayers[sum - 1].GetComponent<ControllerRemotePlayer>().player = player;
                    manager.remotePlayers[sum-1].GetComponent<ControllerRemotePlayer>().GetAvartar();
                    manager.remotePlayers[sum].GetComponent<ControllerRemotePlayer>().player = temp;
                    manager.remotePlayers[sum].GetComponent<ControllerRemotePlayer>().GetAvartar();
                }            
            }
        }       
    }
    public void RemoteDataPlayer(int ID_player)
    {
        for (int i = 0; i < manager.remotePlayers.Count; i++)
        {
            if (manager.remotePlayers[i].GetComponent<ControllerRemotePlayer>().player.player_id == ID_player)
            {
                players.Remove(manager.remotePlayers[i].GetComponent<ControllerRemotePlayer>().player);
                manager.remotePlayers[i].GetComponent<ControllerRemotePlayer>().player = default;
                return;
            }
        }
    }
    /// <summary>
    /// thoát phòng
    /// </summary>
    public void OutRoom()
    {
        WWWForm form = new WWWForm();
        form.AddField("room_id", ID_Room);
        form.AddField("player_id", Login.mnhandata.data.id);
        StartCoroutine(APILeaveRoom(form));
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
        OutRoom();
        Login.connect.Disconnected();
    }
}
