using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    public TMP_Text TenPlayer;
    public TMP_Text Tien;
    public TMP_Text Rank;
    public Text _tien;

    public RawImage avartar_lobby;
    public GameObject Phongcho;
    public GameObject thongtin;
    public GameObject create;   

    public GameObject UI_loading;
    public Slider loadingbar;
    public TMP_Dropdown[] list_dropDownTypeGame;
    private float saveF = 0;
    List<TypeGame> data;

    public void SetTypeGameDropDown(List<TypeGame> data)
    {
        this.data = data;
        if (list_dropDownTypeGame.Length > 0)
        {
            foreach (var typeGame in data)
            {
                foreach (var DDitem in list_dropDownTypeGame)
                {
                    DDitem.options.Add(new TMP_Dropdown.OptionData(typeGame.name_bai));
                    DDitem.RefreshShownValue();
                }
            }
        }
    }
    /// <summary>
    /// hiển thị tiến trình của load avartar, thông tin player
    /// </summary>
    /// <param name="f"></param>    
    public void SetProcess(float f)
    {
        if (loadingbar && loadingbar.value >= saveF)
        {

            if (loadingbar.value < 1 )
            {
                loadingbar.value = f;
                saveF = loadingbar.value;
            }
            else if (loadingbar.value < 2)
            {
                loadingbar.value = 1 + f;
                saveF = loadingbar.value;
            }
            else
            {
                loadingbar.value = 2 + f;
                saveF = loadingbar.value;
            }


        }
    }
    private void LateUpdate()
    {
        if (loadingbar)
        {
            if (loadingbar.value >= loadingbar.maxValue)
            {
                UI_loading.SetActive(false);
            }
        }       
    }
    public void SetInfoPlayer(DataFromLogin data)
    {
        TenPlayer.text = data.data.nickname;
        Tien.text = data.data.money.ToString();
        _tien.text = data.data.money.ToString();
    }

    public void GetMoney(DataFromLogin data)
    {
        
        Tien.text = data.data.money.ToString() + " Xu";
        
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
