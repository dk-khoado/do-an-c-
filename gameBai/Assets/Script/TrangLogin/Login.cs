using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public TMP_InputField username_login;
    public TMP_InputField password_login;
    public TMP_InputField username_register;
    public TMP_InputField password_register;
    public TMP_InputField repassword;
    public TMP_InputField email;
    public GameObject loading;
    public static DataFromLogin mnhandata = new DataFromLogin();
    public static ConnectToServer connect = new ConnectToServer();
    private void Start()
    {
        PlayerPrefs.DeleteAll();
        connect = new ConnectToServer();
    }
    public void onLogin()
    {
        loading.SetActive(true);
        sendDataLogin dataLogin = new sendDataLogin(username_login.text, password_login.text);
        string json = JsonUtility.ToJson(dataLogin);
        StartCoroutine(GetRequestLogin("http://26.60.150.44/api/User/Login", json));
        //Debug.Log(json);               
    }
    private void Update()
    {
        if (connect.Connected)
        {

            bool check = false;
            bool.TryParse(connect.GetUServer("succses").value, out check);
            if (check)
            {
                loading.SetActive(false);
                StartCoroutine(LoadYourAsyncScene());
                //Debug.Log("ket noi thanh cong");
            }
            else
            {
                PlayerModel player = new PlayerModel();
                player.ID_player = mnhandata.data.id;
                player.cmd = "Login";
                connect.Send(player);
            }
        }
    }
    public void onRegister()
    {

        SendResgiter data2 = new SendResgiter(username_register.text, password_register.text, email.text);
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
    IEnumerator GetRequestLogin(string uri, string postdata)
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
                loading.SetActive(false);
            }
            else
            {
                try
                {
                    if (webRequest.isDone)
                    {
                        mnhandata = JsonUtility.FromJson<DataFromLogin>(webRequest.downloadHandler.text);
                        //Debug.Log(webRequest.downloadHandler.text);
                        if (mnhandata.result > 0)
                        {
                            connect.Connect("26.60.150.44", 8080);
                            //loading.SetActive(false);
                            //ebug.Log(JsonUtility.ToJson(mnhandata));
                            // StartCoroutine(LoadYourAsyncScene());
                            PlayerModel player = new PlayerModel();
                            player.ID_player = mnhandata.data.id;
                            player.cmd = "Login";
                            connect.Send(player);
                        }
                        else
                        {
                            loading.SetActive(false);
                            Debug.Log(mnhandata.message);
                            //Debug.Log(mnhandata.data);
                        }
                    }
                }
                catch (Exception e)
                {
                    loading.SetActive(false);
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

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lobby");
        PlayerPrefs.GetInt("id");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
