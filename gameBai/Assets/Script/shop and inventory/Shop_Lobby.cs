using Assets.Script.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Script.shop_and_inventory
{
    public class Shop_Lobby : MonoBehaviour
    {
        public ShopItem shop;
        public TMP_Text name_item;
        public TMP_Text cost;
        public TMP_Text descript;
      

        public void SetInfoShop(DataShop data)
        {
            //name_item.text = data.data.name_item;
            //cost.text = data.data.cost.ToString() + " Xu";
            //descript.text = data.data.descript;
        }
        public void Update()
        {
            name_item.text = shop.name_item;
            cost.text = shop.cost.ToString() + " Xu";
            descript.text = shop.descript;
        }

        public void Buy()
        {
            string data = "{\"player_id\": "+2011+", \"item_id\": "+shop.id_item+"}";
            Debug.Log(data);
            StartCoroutine(BuySkin(InternetConfig.basePath + "/api/ItemManager/BuySkin" , data));
        }

        IEnumerator BuySkin(string uri, string data)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, data))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
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
                    Debug.Log(webRequest.downloadHandler.text);
                    yield return null;

                }
            }
        }


    }

   
}
