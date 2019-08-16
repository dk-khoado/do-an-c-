using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class Controller_Lobby : MonoBehaviour
{
    public string BaseURL;
    string path;
    DataFromLogin mdata = new DataFromLogin();
    public static GetRoomModel roomModel = new GetRoomModel();
    public GameObject itemRoom;
    public GameObject parentItemRoom;
    public bool isNew;
    public UI_Lobby ui_Lobby;
    [SerializeField]
    private List<GameObject> roomList;
    public  TypeGameModel typeGames;
    private void Awake()
    {
        StartCoroutine(GetRequestHoso(InternetConfig.basePath + "/api/User/Get/" + Login.mnhandata.data.id));
        StartCoroutine(GetRequestDowloadAvartar(InternetConfig.basePath + "/upload/" + Login.mnhandata.data.avartar));      

    }
    private void Start()
    {        
        StartCoroutine(GetTypeGame(InternetConfig.basePath + "/api/TypeGame/Get"));
    }
    public void FindAllRoomList()
    {
        roomList.Clear();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("room_select");
        foreach (var item in temp)
        {
            roomList.Add(item);
        }
    }
    /// <summary>
    /// cập nhập thông tin phòng hoặc thêm mới nếu không có
    /// </summary>
    public void UpdateOrAddRoomList(List<RoomModel> roomModels)
    {
        List<GameObject> temp = new List<GameObject>();
        List<RoomModel> tempRM = new List<RoomModel>();
        foreach (var room in roomList)
        {
            bool check = false;
            foreach (var newRoom in roomModels)
            {
                if (room.GetComponent<Controller_ItemRoom>().GetIdRoom() == newRoom.id)
                {
                    check = true;
                    room.GetComponent<Controller_ItemRoom>().SetData(newRoom);
                    roomModels.Remove(newRoom);
                    break;
                }
            }
            if (check == false)
            {
                temp.Add(room);
            }
        }
        if (roomModels.Count > 0)
        {
            foreach (var item in roomModels)
            {
                GameObject tempRoom = Instantiate(itemRoom, parentItemRoom.transform);
                tempRoom.GetComponent<Controller_ItemRoom>().SetData(item);
            }
        }
        foreach (var item in temp)
        {
            Destroy(item);
        }

    }
    private void FixedUpdate()
    {
        UServer uServer = Login.connect.GetUServer("refresh_listroom");
        if (uServer.value != "" && uServer.value != null && uServer.isNew)
        {
            StartCoroutine(GetRequestPhongcho(InternetConfig.basePath + "/api/RoomManager/GetRoomList"));
        }
    }
    private void LateUpdate()
    {
        isNew = Login.connect.isNew;
        //StartCoroutine(GetRequestPhongcho(BaseURL + "/api/RoomManager/GetRoomList"));
        //GetComponent<UI_Lobby>().SetInfoPlayer(mdata);
    }

    [System.Obsolete]
    public void Upload()
    {
        //path = EditorUtility.OpenFilePanel("Chon hinh di may", "*", "JPG");
        StartCoroutine(Upload(File.ReadAllBytes(path)));
        StartCoroutine(GetRequestDowloadAvartar(InternetConfig.basePath + "/upload/" + Login.mnhandata.data.avartar));
    }
    public void CloseGame()
    {
        Login.connect.Disconnected();
        SceneManager.LoadScene("Login");
    }
    public void LoadRoom()
    {
        StartCoroutine(GetRequestPhongcho(InternetConfig.basePath + "/api/RoomManager/GetRoomList"));
    }
    IEnumerator GetRequestDowloadAvartar(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri))
        {
            // Debug.Log(uri);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("apiKey", "123456789");
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
                StartCoroutine(GetRequestPhongcho(InternetConfig.basePath + "/api/RoomManager/GetRoomList"));
                ui_Lobby.SetProcess(webRequest.downloadProgress);
                yield return null;
            }
        }
    }

    [System.Obsolete]
    IEnumerator Upload(byte[] data)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("apikey", "123456789");
        WWWForm form = new WWWForm();
        form.AddBinaryData(MD5(Login.mnhandata.data.id.ToString()), data);
        form.headers.Add("apikey", "123456789");
        WWW www = new WWW(InternetConfig.basePath + "/api/Upload/Avartar/" + Login.mnhandata.data.id.ToString(), form);
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
        StartCoroutine(GetRequestHoso(InternetConfig.basePath + "/api/User/Get/" + Login.mnhandata.data.id.ToString()));
        //GetComponent<Phongchoset>().Batphongcho();
    }
    IEnumerator GetRequestPhongcho(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, UnityWebRequest.kHttpVerbPOST))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("apiKey", "123456789");
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                if (webRequest.isDone)
                {
                    FindAllRoomList();
                    yield return new WaitForSeconds(0.1f);
                    roomModel = JsonUtility.FromJson<GetRoomModel>(webRequest.downloadHandler.text);
                    UpdateOrAddRoomList(roomModel.data);

                    ui_Lobby.SetProcess(webRequest.downloadProgress);
                    yield return null;
                }
            }
        }
    }

    IEnumerator GetTypeGame(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("apiKey", "123456789");
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                if (webRequest.isDone)
                {                   
                    yield return new WaitForSeconds(0.1f);
                    typeGames = JsonUtility.FromJson<TypeGameModel>(webRequest.downloadHandler.text);
                    //Debug.Log(webRequest.downloadHandler.text);
                    ui_Lobby.SetTypeGameDropDown(typeGames.data);
                    yield return null;
                }
            }
        }
    }

    IEnumerator GetRequestHoso(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("apiKey", "123456789");
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                if (webRequest.isDone)
                {
                    yield return new WaitForSeconds(0.1f);
                    mdata = JsonUtility.FromJson<DataFromLogin>(webRequest.downloadHandler.text);
                    ui_Lobby.SetProcess(webRequest.downloadProgress);
                    GetComponent<UI_Lobby>().SetInfoPlayer(mdata);
                    yield return null;
                }
                //GetComponent<UI_Lobby>().BatThongTinPlayer();   
            }
        }
    }

}
