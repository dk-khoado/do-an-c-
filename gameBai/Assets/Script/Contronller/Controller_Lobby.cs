using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class Controller_Lobby : MonoBehaviour
{
    public string BaseURL;
    string path;
    DataFromLogin mdata = new DataFromLogin();  
    public static GetRoomModel roomModel = new GetRoomModel();
    public GameObject itemRoom;
    public GameObject parentItemRoom;
    private void Start()
    {
        StartCoroutine(GetRequestDowloadAvartar(BaseURL+ "/upload/"+ PlayerPrefs.GetString("avartar")));
        StartCoroutine(GetRequestHoso(BaseURL+ "/api/User/Get/" + PlayerPrefs.GetInt("id")));
        StartCoroutine(GetRequestPhongcho(BaseURL+ "/api/RoomManager/GetRoomList"));
    }
    private void LateUpdate()
    {
        //StartCoroutine(GetRequestPhongcho(BaseURL + "/api/RoomManager/GetRoomList"));
        //GetComponent<UI_Lobby>().SetInfoPlayer(mdata);
    }
    public void Upload()
    {
        //path = EditorUtility.OpenFilePanel("Chon hinh di may", "*", "JPG");
        StartCoroutine(Upload(File.ReadAllBytes(path)));
        StartCoroutine(GetRequestDowloadAvartar(BaseURL + "/upload/" + PlayerPrefs.GetString("avartar")));
    }
    IEnumerator GetRequestDowloadAvartar(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri))
        {
            Debug.Log(uri);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.error != null)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                GetComponent<UI_Lobby>().avartar_lobby.texture= ((DownloadHandlerTexture)webRequest.downloadHandler).texture;                
            }

        }
    }
    IEnumerator Upload(byte[] data)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData(MD5(PlayerPrefs.GetInt("id").ToString()), data);
        WWW www = new WWW(BaseURL + "/api/Upload/Avartar/" + PlayerPrefs.GetInt("id").ToString(), form);
        yield return www;
        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.isDone)
            {
                Debug.Log(www.text);
                StartCoroutine(GetRequestDowloadAvartar(BaseURL + "/upload/" + PlayerPrefs.GetString("avartar")));
            }
        }
    }
    public string MD5(string input)
    {
        // step 1, calculate MD5 hash from input
        MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);

        // step 2, convert byte array to hex string
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }
    public void Hoso()
    {
        StartCoroutine(GetRequestHoso(BaseURL + "/api/User/Get/" + PlayerPrefs.GetInt("id").ToString()));
        //GetComponent<Phongchoset>().Batphongcho();
    }
    IEnumerator GetRequestPhongcho(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri,UnityWebRequest.kHttpVerbPOST))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
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
                        roomModel = JsonUtility.FromJson<GetRoomModel>(webRequest.downloadHandler.text);                     
                        foreach (var item in roomModel.data)
                        {                           
                            GameObject temp = Instantiate(itemRoom, parentItemRoom.transform);
                            temp.GetComponent<Controller_ItemRoom>().SetData(item);
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

    IEnumerator GetRequestHoso(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
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
                    //GetComponent<UI_Lobby>().BatThongTinPlayer();
                    mdata = JsonUtility.FromJson<DataFromLogin>(webRequest.downloadHandler.text);
                    GetComponent<UI_Lobby>().SetInfoPlayer(mdata);
                }

                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }
    }
    
}
