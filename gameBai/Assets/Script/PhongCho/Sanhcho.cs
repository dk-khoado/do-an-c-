using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Sanhcho : MonoBehaviour
{
    public TMP_Text Ten;
    public TMP_Text Tien;
    public TMP_Text Rank;
    public TMP_InputField ten;
    public TMP_Text id_phongcho;
    public TMP_Text tien;
    public RawImage image;
    public Texture texture;
    string path;
    DataFromLogin mdata = new DataFromLogin();
    public void Start()
    {
        StartCoroutine(GetRequestPhongcho("http://26.60.150.44/api/User/Get/" + PlayerPrefs.GetInt("id").ToString()));
        GetComponent<Phongchoset>().TatPhongcho();
        StartCoroutine(GetRequestDowloadAvartar("http://26.60.150.44/upload/" + PlayerPrefs.GetString("avartar")));
    }
    public void Upload()
    {
        path = EditorUtility.OpenFilePanel("Chon hinh di may", "*", "JPG");
        StartCoroutine(Upload(File.ReadAllBytes(path)));
        StartCoroutine(GetRequestDowloadAvartar("http://26.60.150.44/upload/" + PlayerPrefs.GetString("avartar")));
    }

    IEnumerator GetRequestDowloadAvartar(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri))
        {
            Debug.Log(uri);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.error != null)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                image.texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
            }

        }
    }
    IEnumerator Upload(byte[] data)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData(MD5(PlayerPrefs.GetInt("id").ToString()), data);
        WWW www = new WWW("http://26.60.150.44/api/Upload/Avartar/" + PlayerPrefs.GetInt("id").ToString(), form);
        yield return www;
        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.isDone)
            {
                Debug.Log(www.text);
                StartCoroutine(GetRequestDowloadAvartar("http://26.60.150.44/upload/" + PlayerPrefs.GetString("avartar")));
            }


        }
    }
    public string MD5(string input)
    {
        // step 1, calculate MD5 hash from input
        MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);

        // step 2, convert byte array to hex string
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }
    public void Hoso()
    {
        StartCoroutine(GetRequestHoso("http://26.60.150.44/api/User/Get/" + PlayerPrefs.GetInt("id").ToString()));
        GetComponent<Phongchoset>().Batphongcho();
    }
    IEnumerator GetRequestPhongcho(string uri)
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
                    mdata = JsonUtility.FromJson<DataFromLogin>(webRequest.downloadHandler.text);
                    Ten.text = mdata.data.nickname;
                    Tien.text = mdata.data.money.ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }
    }

    IEnumerator GetRequestHoso(string uri)
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
                    mdata = JsonUtility.FromJson<DataFromLogin>(webRequest.downloadHandler.text);
                    ten.text = mdata.data.nickname;
                    id_phongcho.text = mdata.data.id.ToString();
                    tien.text = mdata.data.money.ToString();
                }

                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }
    }


}





