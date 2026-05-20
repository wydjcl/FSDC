using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : SingletonMono<MainUIManager>
{
    public GameObject root;
    public GameObject chezhan;
    public GameObject setting;
    public void Start()
    {
        chezhan.SetActive(true);
    }
    public void ClickChezhanButton()
    {
        chezhan.SetActive(true);
        BagUI.Instance.Close();
        setting.SetActive(false);
    }
    public void ClickWarehouseButton()
    {
        chezhan.SetActive(false);
        BagUI.Instance.Open(BagType.Warehouse);
        setting.SetActive(false);
    }
    public void ClickSettingButton()
    {
        chezhan.SetActive(false);
        BagUI.Instance.Close();
        setting.SetActive(true);
    }
    public void ClickSaveButton()
    {
        SaveManager.Instance.SaveTest();
    }

    public void ClickStartBattleButton()
    {
        NSceneManager.Instance.LoadBattleScene();
    }
}
