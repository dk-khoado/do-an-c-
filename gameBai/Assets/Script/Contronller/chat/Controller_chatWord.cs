using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Controller_chatWord : MonoBehaviour
{
    public GameObject content;
    public GameObject _message;
    public TMP_InputField inputMessage;
    public ScrollRect chatBox;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inputMessage.isFocused && inputMessage.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            PlayerModel player = Login.connect.player;
            player.cmd = "chat_all";
            player.message = inputMessage.text;
            Login.connect.Send(player);
            inputMessage.ActivateInputField();
            inputMessage.text = "";
        }
        if (inputMessage.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            PlayerModel player = Login.connect.player;
            player.cmd = "chat_all";
            player.message = inputMessage.text;
            Login.connect.Send(player);
            inputMessage.ActivateInputField();
            inputMessage.text = "";
        }
        if (inputMessage.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            inputMessage.ActivateInputField();
        }
        if (Login.connect.isNew)
        {
            UServer data = Login.connect.GetUServer("chat_all");
            if (data.value != "")
            {
                try
                {
                    GameObject temp = Instantiate(_message, content.transform);
                    PlayerModel mMessage = JsonUtility.FromJson<PlayerModel>(data.value);
                    temp.GetComponent<TMP_Text>().text = mMessage.ID_player + ":" + mMessage.message;
                    chatBox.verticalNormalizedPosition = 0;
                }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                }
                chatBox.verticalNormalizedPosition = 0;
            }
        }
    }
}
