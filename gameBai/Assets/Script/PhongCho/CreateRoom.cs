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
    public TMP_Dropdown songuoi;
    public void Create()
    {
        TaoPhongModel dataphong = new TaoPhongModel(Login.mnhandata.data.id, int.Parse(songuoi.options[songuoi.value].text), matkhau.text, tenphong.text);
        string json = JsonUtility.ToJson(dataphong);
        Debug.Log(json);
        StartCoroutine(GetRequestCreate("http://26.60.150.44/api/RoomManager/CreateRoom", json));

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
                            int id_Room = data.data.id;
                            int id_owner = data.data.owner_id;
                            PlayerPrefs.SetInt("id_room", id_Room);
                            PlayerPrefs.SetInt("id_owner", id_owner);

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

