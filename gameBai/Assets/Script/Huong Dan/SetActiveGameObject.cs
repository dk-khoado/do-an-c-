using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetActiveGameObject : MonoBehaviour
{
    public GameObject username;
    public GameObject password;
    public void TurnOn()
    {
        if (username.active)
        {
            username.SetActive(false);
            password.SetActive(true);
        }
        else
        {
            username.SetActive(true);
            password.SetActive(false);
        }
    }
}
