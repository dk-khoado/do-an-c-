using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Sanhcho : MonoBehaviour
{
    public Text ten;
    public Text tien;
    public Text lv;
    nhanData2 mdata = new nhanData2();
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
                    mdata =JsonConvert.DeserializeObject<nhanData2>(webRequest.downloadHandler.text);
                    if (mdata.result > 0)
                    {
                        Debug.Log(mdata.message);
                        Debug.Log(mdata.data);
                    }
                    else
                    {
                        Debug.Log(mdata.message);
                        Debug.Log(mdata.data);
                    }
                }
                catch
                {

                }
            }
        }
    }
}
public class nhanData2
{
    public string response;
    public string message;
    public Object data;
    public int result;
    public nhanData2() { }
}



