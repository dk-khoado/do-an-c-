using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataMessage : MonoBehaviour
{
    public MessageItem data;
    public TMP_Text _message;    
    // Start is called before the first frame update
    void Start()
    {
        _message = GetComponent<TMP_Text>();
        if (_message)
        {
            _message.text = data.message;
            if (data.player_id_send == Login.mnhandata.data.id)
            {
                _message.alignment = TextAlignmentOptions.Right;
            }
            else
            {
                _message.alignment = TextAlignmentOptions.Left;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (_message)
        //{
        //    _message.text = data.message;
        //    if (data.player_id_send == Login.mnhandata.data.id)
        //    {
        //        _message.alignment = TextAlignmentOptions.Right;
        //    }
        //    else
        //    {
        //        _message.alignment = TextAlignmentOptions.Left;
        //    }
        //}
    }
}
