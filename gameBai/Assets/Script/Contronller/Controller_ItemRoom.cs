using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
        PlayerPrefs.SetInt("id_room",data.id);
        PlayerPrefs.SetInt("id_owner", data.owner_id);
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("Room_play");
        if (!async.isDone)
        {
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
            
    }
}
