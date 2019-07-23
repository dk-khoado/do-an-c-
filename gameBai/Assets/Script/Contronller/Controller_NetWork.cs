using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Controller_NetWork : MonoBehaviour
{
    public string URL_GETPlayerList;
    public string address;
    public int ID_owner;
    public int ID_Room;
    public List<Player> players = new List<Player>();
    [SerializeField]
    private ManagerGame manager;
    private void Awake()
    {
        manager = GetComponent<ManagerGame>();
    }
    void Start()
    {        
        StartCoroutine(GetInfoPlayer());
    }
    void Update()
    {
        
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
        string url = address + "api/RoomManager/GetPlayerInRoom?ID_room="+ID_Room;
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
        foreach (var player in players)
        {
            foreach (var removePlayer in manager.remotePlayers)
            {

            }
        }
    }
}
