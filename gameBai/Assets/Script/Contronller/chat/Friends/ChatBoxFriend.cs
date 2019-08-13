using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChatBoxFriend : MonoBehaviour
{
    public GameObject item_message;    
    void Start()
    {
        
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (gameObject.active)
        {

        }
    }

    [System.Obsolete]
    IEnumerator LoadChatBox(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("apikey","123456789");
            yield return webRequest.SendWebRequest();
            if (webRequest.error != null || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                if (webRequest.isDone)
                {
                    List<MessageItem> messageItem = JsonUtility.FromJson<List<MessageItem>>(webRequest.downloadHandler.text);
                    yield return webRequest.downloadProgress;
                    if (webRequest.downloadHandler.isDone)
                    {
                        
                    }                    
                }
            }
        }
    }
}
