using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CreateRoom : MonoBehaviour
{
    RoomByIDModel data = new RoomByIDModel();
    public TMP_InputField tenphong;
    public TMP_InputField matkhau;
    public TMP_Dropdown sotien;
    public TMP_Dropdown soNguoiChoi;
    public TMP_Dropdown typeGame;
    public MessageBox messageBox;

    public Controller_Lobby controller_Lobby;

    private void Start()
    {
        controller_Lobby = GetComponent<Controller_Lobby>();
    }
    public void Create()
    {        
        TaoPhongModel dataphong = new TaoPhongModel(Login.mnhandata.data.id, 
            int.Parse(soNguoiChoi.options[soNguoiChoi.value].text), 
            matkhau.text, tenphong.text, 
            int.Parse(sotien.options[sotien.value].text),
            controller_Lobby.typeGames.data[typeGame.value].id);       
        string json = JsonUtility.ToJson(dataphong);
        Debug.Log(json);
        messageBox.Setting("Thông Báo","Đang Tạo Phòng...",btnType.type_03);
        messageBox.Show();
        StartCoroutine(GetRequestCreate(InternetConfig.basePath+"/api/RoomManager/CreateRoom", json));

    }
    IEnumerator LoadScene()
    {
        AsyncOperation operating = SceneManager.LoadSceneAsync("Room_play");
        if (operating.isDone)
        {

            yield return null;
        }
    }
    IEnumerator GetRequestCreate(string uri, string postdata)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, postdata))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("apikey", "123456789");
            byte[] raw = Encoding.UTF8.GetBytes(postdata);
            webRequest.uploadHandler = new UploadHandlerRaw(raw);
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                try
                {
                    if (webRequest.isDone)
                    {
                        data = JsonUtility.FromJson<RoomByIDModel>(webRequest.downloadHandler.text);
                        Debug.Log(webRequest.downloadHandler.text);
                        if (data.result > 0)
                        {
                            messageBox.SetContent(data.message);
                            int id_Room = data.data.id;
                            int id_owner = data.data.owner_id;
                            InternetConfig.ID_Room = data.data.id;
                            //InternetConfig.ID_Room = data.data.owner_id;
                            //PlayerPrefs.SetInt("id_room", id_Room);
                            //PlayerPrefs.SetInt("id_owner", id_owner);

                            PlayerModel player = new PlayerModel();
                            player.ID_player = id_owner;
                            player.ID_room = id_Room;
                            player.cmd = "Join_room";
                            Login.connect.Send(player);

                            StartCoroutine(LoadScene());
                            Debug.Log(data.message);
                        }
                        else
                        {
                            messageBox.SetContent("");
                            Debug.Log(data.message);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }
    }
}

