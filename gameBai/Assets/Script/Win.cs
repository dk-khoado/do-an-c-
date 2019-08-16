using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Win : MonoBehaviour
{
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetText(string text)
    {
        GetComponentInChildren<TMP_Text>().SetText(text);
    }
    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
       
        if (gameObject.active)
        {
            time += Time.deltaTime;
            if (time > 2f)
            {
                gameObject.SetActive(false);
                time = 0;
            }
        }
        else
        {
            time = 0;
        }
    }
}
