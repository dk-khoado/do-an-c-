using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageBox_input : MonoBehaviour
{
    public InputField input;
    public TMP_Text _message;
    public TMP_Text title;
    public string mTitle;

    public Button btnOK;

    public GameObject gContent;
    
    public string getInput()
    {
        if (input)
        {
            return input.text;
        }
        else
        {
            return "";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }    
    public void Show()
    {
        if (GetComponent<Image>())
        {
            GetComponent<Image>().enabled = true;
        }
        if (gContent)
        {
            gContent.SetActive(true);
        }
    }           
    public void Close()
    {
        if (GetComponent<Image>())
        {
            GetComponent<Image>().enabled = false;
        }
        if (gContent)
        {
            gContent.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
