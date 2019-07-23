using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerCarduno : MonoBehaviour
{
    public cardModel ThuocTinh;
    [SerializeField]
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponentInChildren<Image>();
        image = ThuocTinh.image;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
