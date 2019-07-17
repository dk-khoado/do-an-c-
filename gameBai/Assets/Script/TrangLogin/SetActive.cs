using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    public GameObject Login;
    public GameObject Register;
    public void ChangeLogin()
    {
        Login.SetActive(true);
        Register.SetActive(false);
    }
    public void ChangeRegister()
    {
        Login.SetActive(false);
        Register.SetActive(true);
    }
}
