using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum btnType
{
    type_01, type_02, type_03
}
public class MessageBox : MonoBehaviour
{
   
    public TMP_Text title;
    public TMP_Text content;
    
    public btnType buttonType;
    public string mTitle;
    public string mContent;
    [SerializeField]
    GameObject buttonType01;
    [SerializeField]
    GameObject buttonType02;
    [SerializeField]
    private GameObject messageBox;
    //public bool noTitle;
    private void OnValidate()
    {
        switch (buttonType)
        {
            case btnType.type_01:
                buttonType01.SetActive(true);
                buttonType02.SetActive(false);
                break;
            case btnType.type_02:
                buttonType01.SetActive(false);
                buttonType02.SetActive(true);
                break;
            case btnType.type_03:
                buttonType01.SetActive(false);
                buttonType02.SetActive(false);
                break;
            default:
                break;
        }       
    }
    private void Start()
    {
        //gameObject.SetActive(false);
        GetComponent<Image>().enabled = false;
        if (messageBox)
        {
            messageBox.SetActive(false);
        }
    }
    /// <summary>
    /// thiết lặp dữ liệu hiển thị
    /// </summary>
    /// <param name="title">tiêu đề</param>
    /// <param name="content">nội dung</param>
    /// <param name="type">loại nút nhấn</param>
    public void Setting(string title, string content, btnType type)
    {
        mTitle = title;
        mContent = content;
        buttonType = type;
    }
    public void SetTitle(string title)
    {
        mTitle = title;
    }
    public void SetContent(string content)
    {
        mContent = content;
    }
    public void SetType(btnType type)
    {
        buttonType = type;
    }
    /// <summary>
    /// hiển thị messagebox
    /// </summary>
    public void Show()
    {
        GetComponent<Image>().enabled = true;
        if (messageBox)
        {
            messageBox.SetActive(true);
        }
    }
    public void Close()
    {
        GetComponent<Image>().enabled = false;
        if (messageBox)
        {
            messageBox.SetActive(false);
        }
    }
    private void Update()
    {        
        switch (buttonType)
        {
            case btnType.type_01:
                buttonType01.SetActive(true);
                buttonType02.SetActive(false);
                break;
            case btnType.type_02:
                buttonType01.SetActive(false);
                buttonType02.SetActive(true);
                break;
            case btnType.type_03:
                buttonType01.SetActive(false);
                buttonType02.SetActive(false);
                break;
            default:
                break;
        }
        title.SetText(mTitle);
        content.SetText(mContent);
    }
}

