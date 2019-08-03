using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Controller_Lobby : MonoBehaviour
{
    public string BaseURL;
    string path;
    DataFromLogin mdata = new DataFromLogin();
    public static GetRoomModel roomModel = new GetRoomModel();
    public GameObject itemRoom;
    public GameObject parentItemRoom;
    public bool isNew;
    private void Awake()
    {
        StartCoroutine(GetRequestDowloadAvartar(BaseURL + "/upload/" + Login.mnhandata.data.avartar));
    }
    private void LateUpdate()
    {
        isNew = Login.connect.isNew;
        //StartCoroutine(GetRequestPhongcho(BaseURL + "/api/RoomManager/GetRoomList"));
        //GetComponent<UI_Lobby>().SetInfoPlayer(mdata);
    }
    public void Upload()
    {
        //path = EditorUtility.OpenFilePanel("Chon hinh di may", "*", "JPG");
        StartCoroutine(Upload(File.ReadAllBytes(path)));
        StartCoroutine(GetRequestDowloadAvartar(BaseURL + "/upload/" + Login.mnhandata.data.avartar));
    }
    public void CloseGame()
    {
        Login.connect.Disconnected();
        SceneManager.LoadScene("Login");
    }
    public void LoadRoom()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("room_select"))
        {
            Destroy(item);
        }
        StartCoroutine(GetRequestPhongcho(BaseURL + "/api/RoomManager/GetRoomList"));
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
                GetComponent<UI_Lobby>().avartar_lobby.texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                StartCoroutine(GetRequestHoso(BaseURL + "/api/User/Get/" + Login.mnhandata.data.id));
            }

        }
    }
    IEnumerator Upload(byte[] data)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData(MD5(Login.mnhandata.data.id.ToString()), data);
        WWW www = new WWW(BaseURL + "/api/Upload/Avartar/" + Login.mnhandata.data.id.ToString(), form);
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
                StartCoroutine(GetRequestDowloadAvartar(BaseURL + "/upload/" + Login.mnhandata.data.avartar));
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
        StartCoroutine(GetRequestHoso(BaseURL + "/api/User/Get/" + Login.mnhandata.data.id.ToString()));
        //GetComponent<Phongchoset>().Batphongcho();
    }
    IEnumerator GetRequestPhongcho(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, UnityWebRequest.kHttpVerbPOST))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
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
            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                try
                {
                    if (webRequest.isDone)
                    {
                        mdata = JsonUtility.FromJson<DataFromLogin>(webRequest.downloadHandler.text);
                        GetComponent<UI_Lobby>().SetInfoPlayer(mdata);
                        StartCoroutine(GetRequestPhongcho(BaseURL + "/api/RoomManager/GetRoomList"));
                    }
                    //GetComponent<UI_Lobby>().BatThongTinPlayer();                  
                }

                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }
    }

}
