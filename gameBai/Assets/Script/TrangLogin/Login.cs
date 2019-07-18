using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
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
    nhanData mnhandata = new nhanData();
    public void onLogin()
    {
        sendData dataLogin = new sendData(username_login.text, password_login.text);
        string json = JsonUtility.ToJson(dataLogin);
        StartCoroutine(GetRequestLogin("http://26.60.150.44/api/User/Login", json));
        Debug.Log(json);
       
    }
    public void onRegister()
    {

        sendData2 data2 = new sendData2(username_register.text,password_register.text,email.text);
        string json = JsonConvert.SerializeObject(data2);
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
                   string var = JsonConvert.SerializeObject(mnhandata);
                    Debug.Log(var);
                    if(mnhandata.result > 0)
                    {
                        PlayerPrefs.SetInt("id",int.Parse(mnhandata.response));
                        Debug.Log(mnhandata.data);
                        //StartCoroutine(LoadYourAsyncScene());
                    }
                    else
                    {
                        Debug.Log(mnhandata.message);
                        Debug.Log(mnhandata.data);
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
                    var mnhandata = JsonUtility.FromJson<nhanData>(postdata);
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
public class sendData
{
    public string username;
    public string password;
    public sendData() { }
    public sendData(string muser,string mpass)
    {
        this.username = muser;
        this.password = mpass;
    }
}
public class nhanData
{
    public string response;
    public string message;
    public int result;
    public Object data = new Object();
    public nhanData() { }
}
public class sendData2
{
    public string username;
    public string password;
    public string email;
    public sendData2() { }
    public sendData2(string muser,string mpassword,string memail)
    {
        this.username = muser;
        this.password = mpassword;
        this.email = memail;
    }
}
public class Object
{
    public int id;
    public string username;
    public string password;
    public double money;
    public string email;
    public string nickname;
    public int status;
    public string avartar;
   
}
