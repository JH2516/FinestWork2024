using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UIItemImage
{
    CollapseAlarm, PistolNozzle, PortableLift
}

public enum PanelType
{
    GameClear, GameOver, GamePause
}

[DefaultExecutionOrder(2)]
public class UIManager : MonoBehaviour
{
    public  static UIManager ui;

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

    [Header("UI : Button")]
    public  GameObject      button_FireAttack;
    public  GameObject      button_Interact;

    [Header("UI : Items")]
    public  Button          button_CollapseAlarm;
    public  Button          button_PistolNozzle;
    public  Button          button_PortableLift;

    public  Image           image_CollapseAlarm;
    public  Image           image_PistolNozzle;
    public  Image           image_PortableLift;

    [Header("UI : Debug")]
    public  GameObject      Text_Debug;
    public  TextMeshProUGUI Text_DebugHPFreeze;
    public  TextMeshProUGUI Text_DebugIsUsingBoostItem;





    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    private void Awake()
    {
        if (ui == null) ui = this;

        backGround_HPBar.enabled = true;
        backGround_ExtendHPBar.enabled = false;
        ui_PlayerExtendHPBar.SetActive(false);
        ui_RemainCollapseRoom.SetActive(false);

        UIButton_ItemIsActive(UIItemImage.CollapseAlarm, true);
        UIButton_ItemIsActive(UIItemImage.PistolNozzle, true);
        UIButton_ItemIsActive(UIItemImage.PortableLift, false);

        mask_HPBar.sizeDelta = new Vector2(800, 150);
    }





    //-----------------< UI. 사용자 인터페이스 갱신 모음 >-----------------//

    /// <summary>
    /// UI - 게임 플레이 시간 갱신
    /// </summary>
    /// <param name="time"> 게임 플레이 시간 </param>
    public void UI_InGameTime(float time)
    {
        text_InGameTime.text = $"Time : {(int)time}";
    }

    /// <summary>
    /// UI - 플레이어 체력 갱신 (체력 값)
    /// </summary>
    /// <param name="hp"> 현재 플레이어 체력 </param>
    public void UI_PlayerHPAmount(float hp)
    {
        player_HPBar.fillAmount         = hp / 100;
        player_ExtendHPBar.fillAmount   = hp / 150;
    }

    /// <summary>
    /// UI - 플레이어 체력 갱신 (게이지 값)
    /// </summary>
    /// <param name="gaugeFill"> 플레이어 체력 회복 게이지 양 </param>
    /// <param name="HPMax"> 플레이어 최대 체력 </param>
    public void UI_PlayerHPGauge(float gaugeFill, float HPMax)
    {
        player_HPBar.fillAmount = gaugeFill * (HPMax / 100);
        player_ExtendHPBar.fillAmount = gaugeFill;
    }

    /// <summary>
    /// UI - 플레이어 추가 체력 활성화
    /// </summary>
    public void UI_EnablePlayerExtendHP()
    {
        backGround_HPBar.enabled = false;
        backGround_ExtendHPBar.enabled = true;
        ui_PlayerExtendHPBar.SetActive(true);
        mask_HPBar.sizeDelta = new Vector2(1185, 150);
    }

    /// <summary>
    /// UI - 스테이지 내 불 개수 갱신
    /// </summary>
    /// <param name="count"> 스테이지 내 불 개수 </param>
    public void UI_FireRemains(int count)
    {
        text_FireRemain.text = count.ToString();
        if (count == 0) text_FireRemain.color = Color.green;
    }

    /// <summary>
    /// UI - 스테이지 내 구조자 인원수 갱신
    /// </summary>
    /// <param name="count"> 스테이지 내 구조자 인원수 </param>
    public void UI_SurvovirRemains(int count)
    {
        text_SurvivorRemain.text = count.ToString();
        if (count == 0) text_SurvivorRemain.color = Color.green;
    }

    /// <summary>
    /// UI - 붕괴물 경보기 잔여 시간 표시
    /// </summary>
    /// <param name="isActive"> 활성화 여부 </param>
    public void UI_CollapseRoomRemain(bool isActive)
    {
        ui_RemainCollapseRoom.SetActive(isActive);
    }

    /// <summary>
    /// UI - 플레이어 산소 고갈 위험 효과 연출
    /// </summary>
    /// <param name="hp"> 현재 플레이어 체력 </param>
    /// <param name="HPMax"> 플레이어 최대 체력 </param>
    public void UI_WarinigEffect(float hp, float HPMax)
    {
        float alpha = 1 - hp / (HPMax / 10);
        effect_Warning.color = new Color(1, 1, 1, alpha);
    }

    /// <summary>
    /// UI - 게임 완료 화면 갱신
    /// </summary>
    /// <param name="require_BoostItems"> 조건 : 부스트 아이템 사용 여부 </param>
    /// <param name="require_Time"> 조건 : 시간 내 진화 완료 여부 </param>
    /// <param name="rank"> 스테이지 별 개수 </param>
    public void UI_GameClear(bool require_BoostItems, bool require_Time, int rank)
    {
        text_GameClear.color        = Color.green;
        text_UnusedBoostItem.color  = require_BoostItems    ? Color.green : new Color(1, 1, 1, 0.5f);
        text_InTime.color           = require_Time          ? Color.green : new Color(1, 1, 1, 0.5f);

        text_InTime.text = $"2. {sO_Stage.stageClearTime[StageLoader.Stage]}초 내 임무 수행 완료";

        img_ClearStars[rank].SetActive(true);
    }

    /// <summary>
    /// UI - 게임 오버 화면 갱신
    /// </summary>
    /// <param name="type"> 게임 오버 사유 타입 </param>
    public void UI_GameOver(GameOverType type)
    {
        player_HPBar.fillAmount = 0;
        player_ExtendHPBar.fillAmount = 0;

        text_CauseOfGameOver.text = sO_Stage.gameOverComments[type];
    }

    /// <summary>
    /// UI - 게임 상태 별 Panel 출현 여부 설정
    /// </summary>
    /// <param name="panelType"> Panel 타입 </param>
    /// <param name="isActive"> 활성화 여부 </param>
    public void UI_Panel(PanelType panelType, bool isActive)
    {
        GameObject panel = null;

        switch (panelType)
        {
            case PanelType.GameClear:   panel = Panel_GameClear;    break;
            case PanelType.GameOver:    panel = Panel_GameOver;     break;
            case PanelType.GamePause:   panel = Panel_Pause;        break;
        }

        panel.SetActive(isActive);
    }



    //-----------------< UI Button. 버튼용 사용자 인터페이스 모음 >-----------------//

    /// <summary>
    /// UI Button - 아이템 버튼 활성화 여부 설정
    /// </summary>
    /// <param name="imgType"> 아이템 타입 </param>
    /// <param name="isActive"> 활성화 여부 </param>
    /// <param name="isUsedItem"> (1회성) 아이템 사용 여부 </param>
    public void UIButton_ItemIsActive(UIItemImage imgType, bool isActive, bool isUsedItem = false)
    {
        Button button = null;
        Image ui = null;

        switch (imgType)
        {
            case UIItemImage.CollapseAlarm:
                button = button_CollapseAlarm; ui = image_CollapseAlarm; break;
            case UIItemImage.PistolNozzle:
                button = button_PistolNozzle; ui = image_PistolNozzle; break;
            case UIItemImage.PortableLift:
                button = button_PortableLift; ui = image_PortableLift; break;
        }

        button.interactable = isActive;
        ui.color =
        isUsedItem ? Color.red / 2 :
        isActive ? Color.white : Color.white / 2;
    }

    /// <summary>
    /// UI Button - 플레이어 메인 버튼 : 공격
    /// </summary>
    public void UIButton_EnableAttack()
    {
        button_FireAttack.gameObject.SetActive(true);
        button_Interact.gameObject.SetActive(false);
    }

    /// <summary>
    /// UI Button - 플레이어 메인 버튼 : 상호작용
    /// </summary>
    public void UIButton_EnableInteract()
    {
        button_Interact.gameObject.SetActive(true);
        button_FireAttack.gameObject.SetActive(false);
    }





    //-----------------< UI Debug. 디버그용 사용자 인터페이스 모음 >-----------------//

    /// <summary>
    /// UI Debug - 디버그 활성화 표시
    /// </summary>
    public void UIDebug()
    {
        Text_Debug.SetActive(true);
        Text_DebugIsUsingBoostItem.gameObject.SetActive(true);
    }

    /// <summary>
    /// UI Debug - 플레이어 체력 동결
    /// </summary>
    /// <param name="isActive"> 활성화 여부 </param>
    public void UIDebug_HPFreeze(bool isActive)
    {
        Text_DebugHPFreeze.gameObject.SetActive(isActive);
    }

    /// <summary>
    /// UI Debug - 스테이지 부스트 아이템 사용 여부 표시
    /// </summary>
    /// <param name="isActive"> 활성화 여부 </param>
    public void UIDebug_UsedBoostItem(bool isActive)
    {
        Text_DebugIsUsingBoostItem.text = $"Boost {(isActive ? "X" : "O")}";
    }
}
