using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using Assets.Script.Models;
using Assets.Script.shop_and_inventory;

public class Shop : MonoBehaviour
{
    
    DataShop data = new DataShop();
    public UI_Lobby ui_Lobby;
    DataFromLogin mdata = new DataFromLogin();
    public GameObject ShopItem;
    public Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(GetRequestMoney(InternetConfig.basePath + "/api/User/Get/" + 2011));
        StartCoroutine(GetRequestShop(InternetConfig.basePath + "/api/ItemManager/GetAllInShop"));
         
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onCard()
    {

    }

    public void onTable()
    {

    }

    public void onAvatar()
    {

    }

    public void onBack()
    {

    }

   

    IEnumerator GetRequestShop(string uri)
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
                data = JsonUtility.FromJson<DataShop>(webRequest.downloadHandler.text);
                Debug.Log(data.data.Count);
                foreach (var item in data.data)
                {
                    GameObject tempShop = Instantiate(ShopItem, parent);
                    tempShop.GetComponent<Shop_Lobby>().shop = item;
                }
                
                yield return null;

            }
        }
    }

    IEnumerator GetRequestMoney(string uri)
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
                yield return new WaitForSeconds(0.1f);
                mdata = JsonUtility.FromJson<DataFromLogin>(webRequest.downloadHandler.text);
                
                GetComponent<UI_Lobby>().GetMoney(mdata);
                yield return null;
            }
        }
    }

    
}
