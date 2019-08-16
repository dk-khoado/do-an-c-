using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Controller_ItemRoom : MonoBehaviour
{
    public TMP_Text number;
    public TMP_Text name;
    public TMP_Text currentPlayer;
    public TMP_Text typeGame;
    public GameObject manager;
    public RoomModel data = new RoomModel();

    public MessageBox messageBox;
    public MessageBox_input messageBox_input;
    public bool isJoining;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("manager");
        messageBox = GameObject.FindGameObjectWithTag("MessageBox").GetComponent<MessageBox>();
        messageBox_input = GameObject.Find("MessageBox_inputPassword").GetComponent<MessageBox_input>();
    }
    public int GetIdRoom()
    {
        return data.id;
    }
    public void SetData(RoomModel index)
    {
        data = index;
    }
    public void JoinRoom()
    {
        Room_Config.bet_money = data.bet_money;
        //DontDestroyOnLoad(gameObject);
        //WWWForm form = new WWWForm();
        //form.AddField("room_id", data.id);
        //form.AddField("player_id", Login.mnhandata.data.id);
        //messageBox.Setting("Thông báo","Đang Vào phòng", btnType.type_03);
        //StartCoroutine(ApiJoinRoom(InternetConfig.basePath + "/api/RoomManager/JoinRoom", form));

        if (data.password.Trim().Equals(""))
        {
            PlayerModel playerModel = new PlayerModel();
            playerModel.cmd = "checkroom";
            playerModel.ID_player = Login.mnhandata.data.id;
            playerModel.ID_room = data.id;
            playerModel.message = "true";
            //Debug.Log("lồn mẹ thiệt");
            Login.connect.Send(playerModel);
            isJoining = true;
        }
        else
        {
            messageBox_input.Show();
            messageBox_input.btnOK.onClick.AddListener(()=>checkPassword(messageBox_input.input.text));
        }
    }
    public void checkPassword(string passwrod)
    {
        if (passwrod.Trim() == data.password.Trim())
        {
            PlayerModel playerModel = new PlayerModel();
            playerModel.cmd = "checkroom";
            playerModel.ID_player = Login.mnhandata.data.id;
            playerModel.ID_room = data.id;
            playerModel.message = "true";
            //Debug.Log("lồn mẹ thiệt");
            Login.connect.Send(playerModel);
            isJoining = true;
            Debug.Log("ĐÚng pass");
        }
        else
        {
            Debug.Log("sai pass");
        }
    }
    IEnumerator LoadScene()
    {
        AsyncOperation operating;
        switch (data.id_bai)
        {
            case 1:
                operating = SceneManager.LoadSceneAsync("Room_play");
                if (operating.isDone)
                {

                    yield return null;
                }
                break;
            case 2:
                operating = SceneManager.LoadSceneAsync("Room_play_01");
                if (operating.isDone)
                {

                    yield return null;
                }
                break;
            case 3:
                operating = SceneManager.LoadSceneAsync("Room_play_catTe");
                if (operating.isDone)
                {

                    yield return null;
                }
                break;
            default:
                break;
        }
    }
    IEnumerator ApiJoinRoom(string url, WWWForm data)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, data))
        {
            webRequest.SetRequestHeader("apikey", "123456789");
            yield return webRequest.SendWebRequest();
            if (webRequest.error != null || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                if (webRequest.isDone)
                {
                    PlayerModel player = new PlayerModel();
                    player.ID_player = Login.mnhandata.data.id;
                    player.ID_room = this.data.id;
                    player.cmd = "Join_room";
                    Login.connect.Send(player);

                    InternetConfig.ID_Room = this.data.id;
                    //PlayerPrefs.SetInt("id_room", this.data.id);
                    //PlayerPrefs.SetInt("id_owner", this.data.owner_id);
                    //Debug.Log(webRequest.downloadHandler.text);
                    StartCoroutine(LoadScene());
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (isJoining)
        {
            Debug.Log("chó để thiệt");
            UServer uServerStart = Login.connect.GetUServer("checkroom");
            if (uServerStart.value != "" && uServerStart.value != null && uServerStart.isNew)
            {
                try
                {
                    bool isJoin = false;
                    bool.TryParse(uServerStart.value, out isJoining);
                    if (isJoin)
                    {
                        InternetConfig.ID_Room = this.data.id;
                        messageBox.Setting("Thông báo", "Đang Vào phòng", btnType.type_03);
                        messageBox.Show();
                        StartCoroutine(LoadScene());
                    }
                    else
                    {
                        InternetConfig.ID_Room = this.data.id;
                        messageBox.Setting("Thông báo", "Đang Vào phòng", btnType.type_03);
                        messageBox.Show();
                        WWWForm form = new WWWForm();
                        form.AddField("room_id", data.id);
                        form.AddField("player_id", Login.mnhandata.data.id);
                        StartCoroutine(ApiJoinRoom(InternetConfig.basePath + "/api/RoomManager/JoinRoom", form));
                    }
                }
                catch(Exception e)
                {
                    //đây bỏ trống
                    Debug.Log(e);
                }
            }          
        }
    }
    // Update is called once per frame
    void Update()
    {
        number.text = data.id.ToString();
        name.text = data.room_name;
        currentPlayer.text = data.current_player.ToString() + "/" + data.limit_player;

    }
}
