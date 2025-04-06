using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UIItemImage
{
    CollapseAlarm, PistolNozzle, PortableLift
}

public class UIManager : MonoBehaviour
{
    public  SO_Stage        sO_Stage;

    [Header("UI : Panel")]
    public  GameObject      Panel_Pause;
    public  GameObject      Panel_GameOver;
    public  GameObject      Panel_GameClear;

    [Header("UI : Panel")]
    public  GameObject      ui_PlayerExtendHPBar;
    public  GameObject      ui_RemainCollapseRoom;

    [Header("UI : Bar")]
    public  Image           player_HPBar;
    public  Image           player_ExtendHPBar;
    public  Image           backGround_HPBar;
    public  Image           backGround_ExtendHPBar;
    public  RectTransform   mask_HPBar;

    [Header("UI : Effect")]
    public  Image           effect_Warning;

    [Header("UI : Text")]
    public  TextMeshProUGUI text_FireRemain;
    public  TextMeshProUGUI text_SurvivorRemain;
    public  TextMeshProUGUI text_InGameTime;
    public  TextMeshProUGUI text_CollapseRoomRemain;
    public  TextMeshProUGUI text_CauseOfGameOver;

    [Header("UI : Clear")]
    public  GameObject[]    img_ClearStars;

    [Header("UI : Clear Text")]
    public  TextMeshProUGUI text_GameClear;
    public  TextMeshProUGUI text_UnusedBoostItem;
    public  TextMeshProUGUI text_InTime;
    public  TextMeshProUGUI text_ClearStars;

    [Header("UI : Button")]
    public  GameObject      button_FireAttack;
    public  GameObject      button_Interact;

    [Header("UI : Items")]
    public  GameObject      button_CollapseAlarm;
    public  GameObject      button_PistolNozzle;
    public  GameObject      button_PortableLift;

    private Image[]         uiButton_CollapseAlarm;
    private Image[]         uiButton_PistolNozzle;
    private Image[]         uiButton_PortableLift;

    public  GameObject      Text_Debug;
    public  TextMeshProUGUI Text_DebugHPFreeze;
    public  TextMeshProUGUI Text_DebugIsUsingBoostItem;
    

    private void Awake()
    {
        backGround_HPBar.enabled = true;
        backGround_ExtendHPBar.enabled = false;
        ui_PlayerExtendHPBar.SetActive(false);
        ui_RemainCollapseRoom.SetActive(false);

        //foreach (var img in img_ClearStars) img.SetActive(false);

        uiButton_CollapseAlarm = button_CollapseAlarm.GetComponentsInChildren<Image>();
        uiButton_PortableLift = button_PortableLift.GetComponentsInChildren<Image>();
        uiButton_PistolNozzle = button_PistolNozzle.GetComponentsInChildren<Image>();

        UIButton_ItemIsActive(UIItemImage.CollapseAlarm, true);
        UIButton_ItemIsActive(UIItemImage.PistolNozzle, true);
        UIButton_ItemIsActive(UIItemImage.PortableLift, false);

        //text_FireRemain.text = fires.ToString();

        //Text_Debug.SetActive(false);
        //Text_DebugHPFreeze.gameObject.SetActive(false);
        //Text_DebugIsUsingBoostItem.gameObject.SetActive(false);
        mask_HPBar.sizeDelta = new Vector2(800, 150);
    }

    public void UI_InGameTime(float time)
    {
        text_InGameTime.text = $"Time : {(int)time}";
    }

    public void UI_PlayerHP(float hp)
    {
        player_HPBar.fillAmount         = hp / 100;
        player_ExtendHPBar.fillAmount   = hp / 150;
    }

    public void UI_EnablePlayerExtendHP()
    {
        backGround_HPBar.enabled = false;
        backGround_ExtendHPBar.enabled = true;
        ui_PlayerExtendHPBar.SetActive(true);
        mask_HPBar.sizeDelta = new Vector2(1185, 150);
    }

    public void UI_FireRemains(int count)
    {
        text_FireRemain.text = count.ToString();
        if (count == 0) text_FireRemain.color = Color.green;
    }

    public void UI_SurvovirRemains(int count)
    {
        text_SurvivorRemain.text = count.ToString();
        if (count == 0) text_SurvivorRemain.color = Color.green;
    }

    public void UI_GameClear(bool require_BoostItems, bool require_Time, int rank)
    {
        text_GameClear.color        = Color.green;
        text_UnusedBoostItem.color  = require_BoostItems    ? Color.green : new Color(1, 1, 1, 0.5f);
        text_InTime.color           = require_Time          ? Color.green : new Color(1, 1, 1, 0.5f);

        text_InTime.text = $"2. {sO_Stage.stageClearTime[StageLoader.Stage]}초 내 임무 수행 완료";

        img_ClearStars[rank].SetActive(true);
    }

    public void UI_GameOver(GameOverType type)
    {
        player_HPBar.fillAmount = 0;
        player_ExtendHPBar.fillAmount = 0;

        text_CauseOfGameOver.text = sO_Stage.gameOverComments[type];

        Panel_GameOver.SetActive(true);
    }

    public void UIDebug()
    {
        Text_Debug.SetActive(true);
        Text_DebugIsUsingBoostItem.gameObject.SetActive(true);
    }

    public void UIDebug_HPFreeze(bool isActive)
    {
        Text_DebugHPFreeze.gameObject.SetActive(isActive);
    }

    public void UIDebug_UsedBoostItem(bool isActive)
    {
        Text_DebugIsUsingBoostItem.text = $"Boost {(isActive ? "X" : "O")}";
    }


    public void UIButton_ItemIsActive(UIItemImage imgType, bool isActive, bool isUsedItem = false)
    {
        Image[] ui = null;

        switch (imgType)
        {
            case UIItemImage.CollapseAlarm: ui = uiButton_CollapseAlarm;    break;
            case UIItemImage.PistolNozzle:  ui = uiButton_PistolNozzle;     break;
            case UIItemImage.PortableLift:  ui = uiButton_PortableLift;     break;
        }

        if (isActive)
        {
            ui[0].color = Color.white;
            ui[1].color = Color.white;
        }
        else if (isUsedItem)
        {
            ui[0].color = new Color(1, 0, 0, 1 / 2f);
            ui[1].color = Color.red / 2;
        }
        else
        {
            ui[0].color = new Color(1, 1, 1, 1 / 2f);
            ui[1].color = Color.white / 2;
        }
    }

    public void UIButton_EnableAttack()
    {
        button_FireAttack.gameObject.SetActive(true);
        button_Interact.gameObject.SetActive(false);
    }

    public void UIButton_EnableInteract()
    {
        button_Interact.gameObject.SetActive(true);
        button_FireAttack.gameObject.SetActive(false);
    }
}
