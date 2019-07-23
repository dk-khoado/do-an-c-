using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class CreateRoom : MonoBehaviour
{
    DataFromLogin data = new DataFromLogin();
    public TMP_InputField tenphong;
    public TMP_InputField matkhau;
    public TMP_Dropdown songuoi;
    public void Create()
    {
        Taophong dataphong = new Taophong(PlayerPrefs.GetInt("id"), int.Parse(songuoi.options[songuoi.value].text), matkhau.text, tenphong.text);
        string json = JsonUtility.ToJson(dataphong);
        StartCoroutine(GetRequestCreate("http://26.60.150.44/api/RoomManager/CreateRoom", json));
        Debug.Log(json);
        GetComponent<CreateActive>().BatTaophong();
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
                    data = JsonUtility.FromJson<DataFromLogin>(webRequest.downloadHandler.text);
                    Debug.Log(webRequest.downloadHandler.text);
                    if (data.result > 0)
                    {
                        Debug.Log(data.message);
                    }
                    else
                    {
                        Debug.Log(data.message);
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
    [Serializable]
    public class Taophong
    {
        public int owner_id;
        public int limit_player;
        public string password;
        public string room_name;
        public Taophong() { }
        public Taophong(int mower,int limit,string mpass,string room)
    {
        this.owner_id = mower;
        this.limit_player = limit;
        this.password = mpass;
        this.room_name = room;
    }
    }

