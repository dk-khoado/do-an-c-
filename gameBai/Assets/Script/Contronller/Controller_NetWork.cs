using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Controller_NetWork : MonoBehaviour
{
    public string URL;   
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    IEnumerator GetPlayerRoom()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(URL, UnityWebRequest.kHttpVerbPOST))
        {
            yield return webRequest.SendWebRequest();
        }
    }
}
