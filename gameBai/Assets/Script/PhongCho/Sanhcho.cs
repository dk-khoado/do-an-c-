using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Sanhcho : MonoBehaviour
{
    public Text ten;
    public Text tien;
    public Text lv;
    DataFromLogin mdata = new DataFromLogin();
    public void Start()
    {
        StartCoroutine(GetRequest("http://26.60.150.44/api/User/Get/" +PlayerPrefs.GetInt("id").ToString()));
    }

    IEnumerator GetRequest(string uri)
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
                    mdata =JsonUtility.FromJson<DataFromLogin>(webRequest.downloadHandler.text);
                    ten.text = mdata.data.nickname;
                    tien.text = mdata.data.money.ToString();
                }
                catch(Exception e)
                {
                    Debug.Log(e);
                }
            }
        }
    }
}



