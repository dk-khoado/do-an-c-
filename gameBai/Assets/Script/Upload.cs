using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Upload : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Open()
    {
        string path = EditorUtility.OpenFilePanel("Chọn hình đê!!","","PNJ");
        Debug.Log(path);
    }
}
