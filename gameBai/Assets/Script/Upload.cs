using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Upload : MonoBehaviour
{
    RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Open()
    {
        //string path = EditorUtility.OpenFilePanel("Chọn hình đê!!","","PNg");
        //Debug.Log(path);        
        //Debug.Log(File.ReadAllBytes(path).Length);        
        //StartCoroutine(UploadImage(File.ReadAllBytes(path)));
    }

    [System.Obsolete]
    private IEnumerator UploadImage(byte[] data)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("khoa", data);
        WWW www = new WWW("http://localhost//api/Upload/Avartar/19", form);
        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.text);
        }
    }
}
