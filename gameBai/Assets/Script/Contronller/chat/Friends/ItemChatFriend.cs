using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// hiển thị danh sách bạn bè
/// </summary>
public class ItemChatFriend : MonoBehaviour
{
    public FriendModel friend;
    public TMP_Text mName;
    public TMP_Text status;
    public RawImage avartar;
    public GameObject ChatBox;

    private void Start()
    {
        ChatBox = GameObject.Find("chatbox");
    }
    public void ShowChatBox()
    {
        ChatBox.SetActive(true);
        ChatBox.SetActive(true);
        ChatBox.GetComponent<ChatBoxFriend>().Show(friend.player_id, friend.friend_id);
       
    }
    public int getID()
    {
        return friend.friend_id;
    }
    public void SetData(FriendModel friendModel)
    {
        friend = friendModel;
    }
    public void SetAvartar()
    {

    }
    // Update is called once per frame
    void LateUpdate()
    {
        mName.SetText(friend.nickname);        
    }
}
