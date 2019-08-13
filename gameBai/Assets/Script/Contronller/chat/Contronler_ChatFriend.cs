using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Contronler_ChatFriend : MonoBehaviour
{
    public GameObject itemChatFriend;
    public Transform positionContent;
    public GameObject chatBox;
    private List<GameObject> friendList = new List<GameObject>();

    //tìm các item friend
    [System.Obsolete]
    private void Start()
    {
        StartCoroutine(GetListFriend(InternetConfig.basePath + "/api/FriendList/GetListfriends/" + Login.mnhandata.data.id));
    }
    void findItem()
    {
        friendList.Clear();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("FriendItem");
        foreach (var item in temp)
        {
            friendList.Add(item);
        }
    }
    void ShowFriend(List<FriendModel> friendModels)
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (var room in friendList)
        {
            bool check = false;
            foreach (var newFriend in friendModels)
            {
                if (room.GetComponent<ItemChatFriend>().getID() == newFriend.friend_id)
                {
                    check = true;
                    room.GetComponent<ItemChatFriend>().SetData(newFriend);
                    friendModels.Remove(newFriend);
                    break;
                }
            }
            if (check == false)
            {
                temp.Add(room);
            }
        }
        if (friendModels.Count > 0)
        {
            foreach (var item in friendModels)
            {
                GameObject tempRoom = Instantiate(itemChatFriend, positionContent);
                tempRoom.GetComponent<ItemChatFriend>().SetData(item);
                tempRoom.GetComponent<Button>().onClick.AddListener(()=>chatBox.SetActive(true));
            }
        }
        foreach (var item in temp)
        {
            Destroy(item);
        }
    }
   
    [System.Obsolete]
    IEnumerator GetListFriend(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url,UnityWebRequest.kHttpVerbPOST))
        {
            webRequest.SetRequestHeader("apiKey", "123456789");
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                if (webRequest.isDone)
                {
                    
                    string data = webRequest.downloadHandler.text;
                    //Debug.Log(data);
                    yield return null;
                    if (webRequest.downloadHandler.isDone)
                    {
                        yield return null;
                        ResponseFriend response = JsonUtility.FromJson<ResponseFriend>(data);
                        findItem();
                        ShowFriend(response.data);
                    }
                    else
                    {
                        yield return null;
                    }              
                }
            }
        }
    }
}
