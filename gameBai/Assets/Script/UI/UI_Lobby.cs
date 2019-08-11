﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Lobby : MonoBehaviour
{
    public TMP_Text TenPlayer;
    public TMP_Text Tien;
    public TMP_Text Rank;
    public RawImage avartar_lobby;
    public GameObject Phongcho;
    public GameObject thongtin;
    public GameObject create;
    private void Start()
    {
        
    }
    public void SetInfoPlayer(DataFromLogin data)
    {
        TenPlayer.text = data.data.nickname;
        Tien.text = data.data.money.ToString();
    }
    public void BatThongTinPlayer()
    {
        //Phongcho.SetActive(true);
        thongtin.SetActive(true);
    }
    public void TatThongTinPlayer()
    {

        thongtin.SetActive(false);
    }
    public void BatTaophong()
    {
        create.SetActive(true);
    }
    public void TatTaophong()
    {
        create.SetActive(false);
    }
}