using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScene : MonoBehaviour
{
    [Header("Page State")]
    [SerializeField] GameObject headerPanel;
    [SerializeField] GameObject homePanel;
    [SerializeField] GameObject weaponPanel;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject missionPanel;
    [SerializeField] GameObject backButtonObj;

    private enum PageState
    {
        Home, Weapon, Shop, Mission
    }
    private void Start()
    {
        SetPageState(PageState.Home);
    }

    private void SetPageState(PageState state)
    {
        switch(state)
        {
            case PageState.Home:
                headerPanel.SetActive(true);
                homePanel.SetActive(true);
                weaponPanel.SetActive(false);
                shopPanel.SetActive(false);
                missionPanel.SetActive(false);
                backButtonObj.SetActive(false);
                break;
            case PageState.Weapon:
                headerPanel.SetActive(true);
                homePanel.SetActive(false);
                weaponPanel.SetActive(true);
                shopPanel.SetActive(false);
                missionPanel.SetActive(false);
                backButtonObj.SetActive(true);
                break;
            case PageState.Shop:
                headerPanel.SetActive(true);
                homePanel.SetActive(false);
                weaponPanel.SetActive(false);
                shopPanel.SetActive(true);
                missionPanel.SetActive(false);
                backButtonObj.SetActive(true);
                break;
            case PageState.Mission:
                headerPanel.SetActive(true);
                homePanel.SetActive(false);
                weaponPanel.SetActive(false);
                shopPanel.SetActive(false);
                missionPanel.SetActive(true);
                backButtonObj.SetActive(true);
                break;
        }
    }
    #region Button
    public void ShopButton()
    {
        SetPageState(PageState.Shop);
    }
    public void WeaponButton()
    {
        SetPageState(PageState.Weapon);
    }
    public void MissionButton()
    {
        SetPageState(PageState.Mission);
    }
    public void BackButton()
    {
        SetPageState(PageState.Home);
    }

    #endregion
}
