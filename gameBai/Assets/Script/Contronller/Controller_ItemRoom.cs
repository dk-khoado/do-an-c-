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
    public GameObject manager;
    RoomModel data = new RoomModel();
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("manager");
    }
    public void SetData(RoomModel index)
    {
        data = index;
        number.text = data.id.ToString();
        name.text = data.room_name;
        currentPlayer.text = data.current_player.ToString();
    }
    public void JoinRoom()
    {
        //DontDestroyOnLoad(gameObject);
        WWWForm form = new WWWForm();
        form.AddField("room_id", data.id);
        form.AddField("player_id", PlayerPrefs.GetInt("id"));
        StartCoroutine(ApiJoinRoom(InternetConfig.basePath + "/api/RoomManager/JoinRoom", form));
    }
    IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("Room_play");
        if (!async.isDone)
        {
            yield return null;
        }
    }
    IEnumerator ApiJoinRoom(string url, WWWForm data)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, data))
        {
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
                    player.ID_player = PlayerPrefs.GetInt("id");
                    player.ID_room = this.data.id;
                    player.cmd = "Join_room";
                    Login.connect.Send(player);
                    StartCoroutine(LoadScene());
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
