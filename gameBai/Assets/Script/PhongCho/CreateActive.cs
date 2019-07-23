using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateActive : MonoBehaviour
{
    public GameObject create;
    public GameObject khung;
    public void BatTaophong()
    {
        create.SetActive(true);
        khung.SetActive(false);
    }
    public void TatTaophong()
    {
        create.SetActive(true);
        khung.SetActive(false);
    }
}
