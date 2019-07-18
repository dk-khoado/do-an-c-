﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.SceneManagement;
using System;

public class Login : MonoBehaviour
{
    public TMP_InputField username_login;
    public TMP_InputField password_login;
    public TMP_InputField username_register;
    public TMP_InputField password_register;
    public TMP_InputField repassword;
    public TMP_InputField email;
    DataFromLogin mnhandata = new DataFromLogin();
    public void onLogin()
    {
        sendDataLogin dataLogin = new sendDataLogin(username_login.text, password_login.text);
        string json = JsonUtility.ToJson(dataLogin);
        StartCoroutine(GetRequestLogin("http://26.60.150.44/api/User/Login", json));
        Debug.Log(json);
       
    }
    public void onRegister()
    {

        SendResgiter data2 = new SendResgiter(username_register.text,password_register.text,email.text);
        string json = JsonUtility.ToJson(data2);
        if (password_register.text == repassword.text)
        {
            StartCoroutine(GetRequestRegister("http://26.60.150.44/api/User/Register", json));
            GetComponent<SetActive>().ChangeRegister();
        }
        else
        {
            Debug.Log("Can't not be register");
           
        }
        
        Debug.Log(json);
    }
    IEnumerator GetRequestLogin(string uri,string postdata)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri,postdata))
        {       
            webRequest.SetRequestHeader("Content-Type", "application/json");
            byte[] raw = Encoding.UTF8.GetBytes(postdata);
            webRequest.uploadHandler = new UploadHandlerRaw(raw);            
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
                    mnhandata = JsonUtility.FromJson<DataFromLogin>(webRequest.downloadHandler.text);
                    Debug.Log(webRequest.downloadHandler.text);
                    if(mnhandata.result > 0)
                    {
                        PlayerPrefs.SetInt("id",mnhandata.data.id);
                       //ebug.Log(JsonUtility.ToJson(mnhandata));
                        StartCoroutine(LoadYourAsyncScene());
                    }
                    else
                    {
                        Debug.Log(mnhandata.message);
                        //Debug.Log(mnhandata.data);
                    }
                }
                catch(Exception e)
                {
                    Debug.Log(e);
                }
            }
        }
    }
    IEnumerator GetRequestRegister(string uri, string postdata)
    {
        
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, postdata))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            byte[] raw = Encoding.UTF8.GetBytes(postdata);
            webRequest.uploadHandler = new UploadHandlerRaw(raw);
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
                    var mnhandata = JsonUtility.FromJson<DataFromLogin>(postdata);
                    if (mnhandata.result > 0)
                    {
                        Debug.Log(mnhandata.message);
                        Debug.Log(mnhandata.data);
                    }
                    else
                    {
                        Debug.Log(mnhandata.message);
                        Debug.Log(mnhandata.data);
                        
                    }
                }
                catch
                {

                }
            }
        }
    }
    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ChangeThePage");
        PlayerPrefs.GetInt("id");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
public class sendDataLogin
{
    public string username;
    public string password;   
    public sendDataLogin(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}
[Serializable]
public class DataFromLogin
{
    public string response;
    public string message;
    public int result;   
    public Data data; 
}
public class SendResgiter
{
    public string username;
    public string password;
    public string email;    
    public SendResgiter(string username, string password, string email)
    {
        this.username = username;
        this.password = password;
        this.email = email;
    }
}
[Serializable]
public class Data
{
    public int id;
    public string username;  
    public double money;
    public string email;
    public string avartar;
    public string nickname;
}