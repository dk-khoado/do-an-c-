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
    public static DataFromLogin mnhandata = new DataFromLogin();
    public static ConnectToServer connect = new ConnectToServer();

    public MessageBox messageBox;
    private void Start()
    {
        PlayerPrefs.DeleteAll();
        //connect = new ConnectToServer();
    }
    public void onLogin()
    {
        //connect = new ConnectToServer();
        messageBox.Show();
        messageBox.buttonType = btnType.type_03;
        messageBox.SetContent("Đang Đăng Nhập...");
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
        if (connect.isError)
        {

            connect = new ConnectToServer();
        }
    }
    public void onRegister()
    {
        messageBox.Show();
        messageBox.buttonType = btnType.type_03;
        messageBox.SetContent("Đăng Tạo Tài Khoản");
        SendResgiter data2 = new SendResgiter(username_register.text, password_register.text, email.text);
        string json = JsonUtility.ToJson(data2);
        if (username_register.text.Trim().Equals(""))
        {
            messageBox.buttonType = btnType.type_02;
            messageBox.SetContent("Tên Đăng Nhập không được trống!");
        }
        else
        if (password_register.text != repassword.text && password_register.text.Trim() != "" && repassword.text.Trim() != "")
        {
            messageBox.buttonType = btnType.type_02;
            messageBox.SetContent("Mật khẩu nhập lại không khớp!");
        }
        else if (email.text.Trim() != "")
        {
            messageBox.buttonType = btnType.type_02;
            messageBox.SetContent("Email Không được trống!");
        }
        else
        {
            StartCoroutine(GetRequestRegister(InternetConfig.basePath + "/api/User/Register", json));
        }

    }
    IEnumerator GetRequestLogin(string uri, string postdata)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, postdata))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("apiKey", "123456789");
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
                    if (webRequest.isDone)
                    {
                        mnhandata = JsonUtility.FromJson<DataFromLogin>(webRequest.downloadHandler.text);
                        //Debug.Log(webRequest.downloadHandler.text);
                        if (mnhandata.result > 0)
                        {
                            messageBox.SetContent("Đang kết nối server...");
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
                            messageBox.SetContent("Sai tên đăng nhập hoặc mật khẩu!");
                            messageBox.buttonType = btnType.type_02;
                            Debug.Log(mnhandata.message);
                            //Debug.Log(mnhandata.data);
                        }
                    }
                }
                catch (Exception e)
                {
                    messageBox.SetContent("Lỗi Xin thử lại");
                    messageBox.buttonType = btnType.type_02;
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
            webRequest.SetRequestHeader("apiKey", "123456789");
            byte[] raw = Encoding.UTF8.GetBytes(postdata);
            webRequest.uploadHandler = new UploadHandlerRaw(raw);
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            if (webRequest.isNetworkError)
            {
                messageBox.buttonType = btnType.type_02;
                messageBox.SetContent("Lỗi Xin thử lại!");
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                try
                {
                    var mnhandata = JsonUtility.FromJson<DataFromLogin>(postdata);
                    if (mnhandata.result > 0)
                    {
                        messageBox.buttonType = btnType.type_02;
                        messageBox.SetContent("Tạo Thành Công :)");
                        GetComponent<SetActive>().ChangeRegister();
                    }
                    else
                    {
<<<<<<< HEAD
                        Debug.Log(mnhandata.message);
                        Debug.Log(mnhandata.data);                       
=======
                        messageBox.buttonType = btnType.type_02;
                        messageBox.SetContent("lỗi:" + mnhandata.message);
>>>>>>> master
                    }
                }
                catch
                {
                    messageBox.buttonType = btnType.type_02;
                    messageBox.SetContent("Lỗi Xin thử lại!");
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
