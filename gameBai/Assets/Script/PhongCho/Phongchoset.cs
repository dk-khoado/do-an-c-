using UnityEngine;

public class Phongchoset : MonoBehaviour
{
    public GameObject Phongcho;
    public GameObject thongtin;
    public void TatPhongcho()
    {
        Phongcho.SetActive(true);
        thongtin.SetActive(false);
    }
    public void Batphongcho()
    {
        Phongcho.SetActive(false);
        thongtin.SetActive(true);
    }
}
