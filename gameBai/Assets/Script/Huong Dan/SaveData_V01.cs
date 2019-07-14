using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveData_V01 : MonoBehaviour
{
   public InputField input;
   public void Save()
    {
        if (input)
        {
            PlayerPrefs.SetString("name",input.text);           
        }
    }
    public void Load()
    {
        if (input)
        {
            input.text = PlayerPrefs.GetString("namesadasd");
        }
    }
}
