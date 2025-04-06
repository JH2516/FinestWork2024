using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using TMPro;

[DefaultExecutionOrder(-80)]
public class StageManager : MonoBehaviour, IEventListener
{
    public  static  StageManager    stageManager;

    public  SO_Stage    sO_Stage;

    public  UIManager   uiManager;

    [Header("Stage List")]
    public  GameObject      stage;
    public  GameObject[]    stageList;

    [Header("Manager")]
    public  AudioManager    audio;
    public  BlurManager     blur;

    [Header("UI : Panel")]
    public  GameObject      Panel_Pause;
    public  GameObject      Panel_GameOver;
    public  GameObject      Panel_GameClear;
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

    //[Header("UI : Background")]
    //public  Image           ui_Survivor;
    //public  Image           ui_Fires;

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

    [Header("Light")]
    public  Light2D         light_Global;

    [Header("Player")]
    public  Player          player;

    [Header("Game Option")]
    [Range(1, 10), SerializeField]
    private float           decreaseHP = 3f;

    [Header("All Survivors")]
    [SerializeField]
    private Transform           all_Survivors;

    [Header("All CollapseRooms")]
    [SerializeField]
    private Transform           all_CollapseRooms;

    [Header("Items")]
    [SerializeField]
    private Item_CollapseAlarm  item_CollapseAlarm;
    [SerializeField]
    private Item_PistolNozzle   item_PistolNozzle;
    [SerializeField]
    private Item_PortableLift   item_PortableLift;

    public  bool            used_CollapseAlarm;


    [Header("List")]
    [SerializeField]
    private List<GameObject> list_Fires;
    [SerializeField]
    private List<GameObject> list_Survivors;

    [Header("Player HP Info")]
    public  float           player_HP;
    private float           player_HPMax;


    [Header("Stage Status")]
    [SerializeField]
    private bool            isGamePlay;
    [SerializeField]
    private bool            isGamePause;
    [SerializeField]
    private bool            isGameOver;
    [SerializeField]
    private bool            isRecoveryHP;
    [SerializeField]
    //private float           time_InGame;
    public  bool            isPlayerWarning;

    private float           startedTime;

    [Header("Stage Unit")]
    public  int             fires;
    public  int             survivors;
    public  int             interacts;

    public  float           Player_HP   =>  player_HP;
    public  float           Player_HPMax => player_HPMax;
    public  bool            IsGamePlay  =>  isGamePlay;
    public  bool            IsGameOver  =>  isGameOver;

    [Header("Stage Clear")]
    [SerializeField]
    private bool            clear_SavedAllSurvivors;
    [SerializeField]
    private bool            clear_ExtinguishedAllFires;
    [SerializeField]
    private byte            clearStars;

    [Header("Stage Clear Require")]
    public  float[]         clearTime_AllStages;
    private bool            unUsedBoostItems;
    private bool            clearTimeInCondition;

    

    [Header("Debug")]
    public  bool            isDebug;
    public  bool            debug_isHPFreeze;

    private PlayerEventType[] listenerTypes =
    {
        PlayerEventType.f_Summon, PlayerEventType.f_Dead,
        PlayerEventType.s_Summon, PlayerEventType.s_Saved,
        PlayerEventType.d_Interact,
        PlayerEventType.g_GameOver,
        PlayerEventType.Debug,
        PlayerEventType.i_UseItem1, PlayerEventType.i_UseItem2, PlayerEventType.i_UseItem3,
        PlayerEventType.UI_UseItem1, PlayerEventType.UI_UseItem2, PlayerEventType.UI_UseItem3,
    };


    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (stageManager == null) stageManager = this;

        Init_Argument();
        Init_Items();
        Init_Player();
        //Init_Survivors();
        //Init_UI();

        //Active_StageBoost();

        Debug.Log("Stage" + StageLoader.Stage);
        Debug.Log("Item" + StageLoader.Item);

        stage = stageList[StageLoader.Stage];
        stage.SetActive(true);
    }

    /// <summary> 초기화 : 변수 </summary>
    private void Init_Argument()
    {

        isGamePlay = true;
        isGamePause = false;
        isGameOver = false;
        isRecoveryHP = false;
        isPlayerWarning = false;

        startedTime = Time.time;

        unUsedBoostItems = (StageLoader.Item == 3) ? true : false;

        clear_ExtinguishedAllFires = false;
        clear_SavedAllSurvivors = false;

        isDebug = false;
        debug_isHPFreeze = false;

        Time.timeScale = 1;
    }

    private void Init_Items()
    {
        item_CollapseAlarm.Init_Item();
        item_PistolNozzle.Init_Item();
        item_PortableLift.Init_Item();
    }

    /// <summary> 초기화 : 플레이어 속성 </summary>
    private void Init_Player()
    {
        player_HP = 100;
        player_HPMax = 100;
    }

    /// <summary> 초기화 : UI </summary>
    private void Init_UI()
    {
        //Panel_Pause.SetActive(false);
        //Panel_GameOver.SetActive(false);
        //Panel_GameClear.SetActive(false);
        backGround_HPBar.enabled = true;
        backGround_ExtendHPBar.enabled = false;
        ui_PlayerExtendHPBar.SetActive(false);
        ui_RemainCollapseRoom.SetActive(false);

        foreach (var img in img_ClearStars) img.SetActive(false);

        text_FireRemain.text = fires.ToString();
        mask_HPBar.sizeDelta = new Vector2(800, 150);
    }

    private void Active_StageBoost()
    {
        switch (StageLoader.Item)
        {
            case 0: Boost_SaveAllSurvivor();                break;
            case 1: Boost_IncreaseHPAmount();               break;
            case 2: Boost_IncreasePlayerLightFOVAngle();    break;
        }
    }

    private void OnEnable()
    {
        EventManager.instance.AddListener(this, listenerTypes);
    }

    private void Start()
    {
        uiManager.UI_PlayerHP(100);
        Active_StageBoost();
    }

    private void Update()
    {
        if (isGameOver) return;
        if (isGamePause) return;

        Timing();

        Set_decreaseHP();
        Check_PlayerHP();
    }

    private void Timing()
        => uiManager.UI_InGameTime(Time.time - startedTime);

    /// <summary> 플레이어 산소 감소 </summary>
    private void Set_decreaseHP()
    {
        if (isRecoveryHP) return;
        if (debug_isHPFreeze) return;

        uiManager.UI_PlayerHP(player_HP -= Time.deltaTime * decreaseHP);
    }

    /// <summary> 플레이어 산소량 검사 </summary>
    private void Check_PlayerHP()
    {
        if (player_HP <= 0)
        {
            audio.GameoverByLowerOxygen(true);
            GameOver(GameOverType.LowerOxygen);
            uiManager.UI_PlayerHP(0);
        }

        FadeAlphaOfWarinigEffect();
    }

    /// <summary> 플레이어 산소량 갱신 </summary>
    public void Player_RemoteHP(float fillHP)
    {
        player_HPBar.fillAmount = fillHP * (player_HPMax / 100);
        player_ExtendHPBar.fillAmount = fillHP;
        player_HP = fillHP * player_HPMax;
    }

    /// <summary> 플레이어 산소 회복 설정 </summary>
    public void Set_RecoveryHP(bool active) => isRecoveryHP = active;






    /// <summary> 생존자 구출 작업 수행 </summary>
    public void Save_Survivor()
    {
        survivors--;
    }

    /// <summary> 생존자 현장 구출 완료 </summary>
    public void Complete_EscapeSurvivor()
    {
        uiManager.UI_SurvovirRemains(survivors);

        if (survivors == 0) clear_SavedAllSurvivors = true;
        if (Check_isGameClear()) GameClear();
    }

    private void CountFireRemains(bool isSummond)
    {
        uiManager.UI_FireRemains(isSummond ? ++fires : --fires);

        if (!isSummond && fires == 0) clear_SavedAllSurvivors = true;
        if (Check_isGameClear()) GameClear();
    }

    private void CountSurvivorRemains()
    {
        uiManager.UI_SurvovirRemains(++survivors);
    }

    private void CountInteract(bool isPlayerAround)
    {
        _ = isPlayerAround ? ++interacts : --interacts;
        if (interacts == 0) Button_ChangeToAttack();
        else                Button_ChangeToInteract();
    }

    /// <summary> 부스트 : 모든 생존자 구출 </summary>
    public void Boost_SaveAllSurvivor()
    {
        EventManager.instance.TriggerEvent(PlayerEventType.b_Save, this);
        survivors = 0;
        uiManager.UI_SurvovirRemains(0);

        Complete_EscapeSurvivor();
    }

    /// <summary> 부스트 : 플레이어 산소 감소 (50%) </summary>
    public void Boost_IncreaseHPAmount()
    {
        player_HP = 150;
        player_HPMax = 150;
        
        uiManager.UI_EnablePlayerExtendHP();
        uiManager.UI_PlayerHP(150);
    }

    public void Boost_IncreasePlayerLightFOVAngle()
    {
        EventManager.instance.TriggerEventForOneListener(PlayerEventType.b_Light, this);
        //player.Set_LightFOVAngle(60f, 90f);
        //player.Set_LightFOVRadius(8f);
        //player.Set_LightAroundRadius(2f, 8f);
        //player.change_FOVAngleRange = 1.5f;
    }



    //public bool UseItem_CollapseAlarm()
    //{
        
    //    bool isUse = item_CollapseAlarm.Use_Item();
        
    //    if (isUse)
    //    {
    //        used_CollapseAlarm = true;
    //        UIButton_IsActiveItemCollapseAlarm(false, true);
    //    }
    //    return isUse;
    //}

    //public bool UseItem_PortableLift()
    //{
        
    //    return item_PortableLift.Use_Item();
    //}

    //public bool UseItem_PistolNozzle()
    //{
        
    //    return item_PistolNozzle.Use_Item();
    //}

    public void UsedItem_PistolNozzle()
    {
        item_PistolNozzle.Used_Item();
    }

    public void SetActive_UIRemainCollapseRoom(bool isActive)
    => ui_RemainCollapseRoom.SetActive(isActive);


    /// <summary> 상태 : 안개가 가득찬 방 입장 </summary>
    public void State_InDarkedRoom(Collider2D room)
    {
        decreaseHP *= 2.5f;

        room.GetComponent<SpriteMask>().enabled = true;
        room.GetComponent<SpriteRenderer>().enabled = false;
    }

    /// <summary> 상태 : 안개가 가득찬 방 퇴장 </summary>
    public void State_OutDarkedRoom(Collider2D room)
    {
        decreaseHP /= 2.5f;

        room.GetComponent<SpriteMask>().enabled = false;
        room.GetComponent<SpriteRenderer>().enabled = true;
    }



    /// <summary> 버튼 : 이동 및 획전 갱신 </summary>
    public void Button_Move(int type)
    {
        player.Set_MoveVec(type);
        player.Set_Rotation(type);
    }

    public void Button_isDown() =>          player.Set_isMove(true);
    public void Button_isUp() =>            player.Set_isMove(false);

    public void Button_Pause() =>       Game_Pause();
    public void Button_Resume() =>      Game_Resume();
    public void Button_Restart() =>     Game_Restart();
    public void Button_Exit() =>        Game_Exit();

    public void Button_ChangeToAttack()
    {
        uiManager.UIButton_EnableAttack();
    }

    public void Button_ChangeToInteract()
    {
        uiManager.UIButton_EnableInteract();
    }

    /// <summary> 게임 일시정지 </summary>
    public void Game_Pause()
    {
        isGamePlay = false;
        isGamePause = true;

        blur.ButtonCaptureID(0);
        audio.PauseSound_WithoutButtonSound();
        audio.ButtonClick(true);
    }

    public void Game_Pause_PanelOn()
    {
        Time.timeScale = 0;
        Panel_Pause.SetActive(true);
    }

    /// <summary> 게임 재개 </summary>
    public void Game_Resume()
    {
        isGamePlay = true;
        isGamePause = false;
        Time.timeScale = 1;

        Panel_Pause.SetActive(false);
        audio.UnPauseSound_WithoutButtonSound();
        audio.ButtonClick(true);
    }

    /// <summary> 게임 재시작 </summary>
    public void Game_Restart()
    {
        EventManager.instance.RemoveListener(this, listenerTypes);
        EventManager.instance.ResetList();
        SceneManager.LoadScene("Stage_Test");
    }

    /// <summary> 게임 나가기 </summary>
    public void Game_Exit()
    {
        EventManager.instance.RemoveListener(this, listenerTypes);
        EventManager.instance.ResetList();
        Time.timeScale = 1;
        SceneManager.LoadScene("Stage");
    }

    /// <summary> 게임 오버 </summary>
    public void GameOver(GameOverType type)
    {
        StopAllCoroutines();

        isGamePlay = false;
        isGameOver = true;
        player.isGameOver = true;
        player_HP = 0;
        

        player.Player_KnockOut();

        blur.ButtonCaptureID(1); // 게임오버 화면 표기 지연
        FadeOutAudio_BurningAround();

        uiManager.UI_GameOver(type);
    }

    public void GameOver_PanelOn()
    {
        Time.timeScale = 0;
        
    }



    public bool Check_isGameClear()
    {
        if (!clear_SavedAllSurvivors)       return false;
        if (!clear_ExtinguishedAllFires)    return false;

        return true;
    }

    /// <summary> 게임 결과 출력 </summary>
    public void GameClear()
    {
        float resultTime = Time.time - startedTime;

        isGamePlay = false;
        int clearRank = 0;

        clearTimeInCondition =
        (resultTime < sO_Stage.stageClearTime[StageLoader.Stage]) ? true : false;

        if (unUsedBoostItems)       clearRank++;
        if (clearTimeInCondition)   clearRank++;


        blur.ButtonCaptureID(2); // 게임 클리어 화면 표기 지연
        //StopCoroutine("FadeInWarningEffect");
        //StartCoroutine("FadeOutWarningEffect");
        //FadeOutAudio_BurningAround();
        audio.StopSound_AllSounds();

        uiManager.UI_GameClear(unUsedBoostItems, clearTimeInCondition, clearRank);
    }

    public void GameClear_PanelOn()
    {
        Time.timeScale = 0;
        Panel_GameClear.SetActive(true);
    }

    public void FadeInAudio_BurningAround() =>
    StartCoroutine(FadeInAudio(audio.audio_BurningAround));

    public void FadeOutAudio_BurningAround() =>
    StartCoroutine(FadeOutAudio(audio.audio_BurningAround));

    IEnumerator FadeInAudio(AudioSource audio)
    {
        AudioSource audio_Burning = audio;
        float currentTime = audio_Burning.volume;

        if (!audio_Burning.isPlaying) audio_Burning.Play();

        while (true)
        {
            if (!player.isDetectedFire) yield break;
            if (stageManager.IsGameOver) yield break;
            if (currentTime > 1)
            {
                audio_Burning.volume = 1;
                yield break;
            }

            currentTime += Time.unscaledDeltaTime;
            audio_Burning.volume = currentTime;
            yield return null;
        }
    }

    IEnumerator FadeOutAudio(AudioSource audio)
    {
        AudioSource audio_Burning = audio;
        float currentTime = audio_Burning.volume;

        while (true)
        {
            if (player.isDetectedFire && !isGameOver) yield break;
            if (currentTime < 0)
            {
                audio_Burning.volume = 0;
                audio_Burning.Stop();
                yield break;
            }

            currentTime -= Time.unscaledDeltaTime;
            audio_Burning.volume = currentTime;
            yield return null;
        }
    }

    private void FadeAlphaOfWarinigEffect()
    {
        if (player_HP >= player_HPMax / 10)
        {
            effect_Warning.color = Color.white * 0;
            return;
        }

        float alpha = 1 - player_HP / (player_HPMax / 10);
        effect_Warning.color = new Color(1, 1, 1, alpha);
        //effect_Warning.color = Color.white * alpha;
    }



    
    public bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        switch (e_Type)
        {
            case PlayerEventType.f_Dead:        CountFireRemains(false);        return true;
            case PlayerEventType.f_Summon:      CountFireRemains(true);         return true;
            case PlayerEventType.s_Saved:       Save_Survivor();                return true;
            case PlayerEventType.s_Summon:      CountSurvivorRemains();         return true;

            case PlayerEventType.d_Interact:    CountInteract((bool)args);      return true;


            case PlayerEventType.g_GameOver:    GameOver((GameOverType)args);   return true;

            case PlayerEventType.Debug:         OnDebug((KeyCode)args);         return true;

            case PlayerEventType.i_UseItem1:
                used_CollapseAlarm = true;
                return true;

            case PlayerEventType.i_UseItem2:
                
                return true;

            case PlayerEventType.i_UseItem3:
                
                return true;

            case PlayerEventType.UI_UseItem1:
                uiManager.UIButton_ItemIsActive(UIItemImage.CollapseAlarm, false, true);
                SetActive_UIRemainCollapseRoom(true);
                return true;

            case PlayerEventType.UI_UseItem2:
                uiManager.UIButton_ItemIsActive(UIItemImage.PortableLift, (bool)args);
                return true;

            case PlayerEventType.UI_UseItem3:
                uiManager.UIButton_ItemIsActive(UIItemImage.PistolNozzle, (bool)args);
                return true;
        }

        return false;
    }




    /// <summary>
    /// 디버그 실행 목록
    /// </summary>
    /// <param name="key"> 디버그 할당 키 </param>
    private void OnDebug(KeyCode key)
    {
        if (!isDebug)
        {
            isDebug = true;
            uiManager.UIDebug();
        }

        switch (key)
        {
            case KeyCode.Alpha9:
                Debug_PlayerHPMax();
                Debug.Log("디버그 : 체력 모두 회복"); break;

            case KeyCode.Alpha0:
                Debug_PlayerHPFreeze();
                Debug.Log($"디버그 : 체력 유지 {(debug_isHPFreeze ? "활성화" : "비활성화")}"); break;

            case KeyCode.Minus:
                Debug_AllFiresExtinguished();
                Debug.Log($"디버그 : 화재 진압 완료"); break;

            case KeyCode.Equals:
                Debug_SaveAllSurvivor();
                Debug.Log($"디버그 : 모든 생존자 구출"); break;

            case KeyCode.Backspace:
                Debug_UsedBoostItem();
                Debug.Log($"디버그 : 부스트 아이템 {(unUsedBoostItems ? "미사용" : "사용")}"); break;

            case KeyCode.O:
                GameOver(GameOverType.Debug);
                Debug.Log("디버그 : 게임 오버"); break;

            case KeyCode.P:
                GameClear();
                Debug.Log("디버그 : 게임 클리어"); break;

            case KeyCode.LeftBracket:
                Debug_DecreaseTime();
                Debug.Log($"디버그 : 스테이지 시간 10초 감소"); break;

            case KeyCode.RightBracket:
                Debug_IncreaseTime();
                Debug.Log($"디버그 : 스테이지 시간 10초 증가"); break;

            case KeyCode.Backslash:
                Debug_ResetTime();
                Debug.Log($"디버그 : 스테이지 시간 초기화"); break;
        }
    }

    /// <summary>
    /// 디버그 : 플레이어 체력 최대 회복
    /// </summary>
    private void Debug_PlayerHPMax()
        => player_HP = player_HPMax;

    /// <summary>
    /// 디버그 : 플레이어 체력 동결
    /// </summary>
    private void Debug_PlayerHPFreeze()
        => uiManager.UIDebug_HPFreeze(debug_isHPFreeze = !debug_isHPFreeze);

    /// <summary>
    /// 디버그 : 플레이어 부스트 아이템 임시 사용
    /// </summary>
    private void Debug_UsedBoostItem()
        => uiManager.UIDebug_UsedBoostItem(unUsedBoostItems = !unUsedBoostItems);

    /// <summary>
    /// 디버그 : 모든 생존자 구출
    /// </summary>
    public void Debug_SaveAllSurvivor()
    {
        if (clear_SavedAllSurvivors) return;

        //EventManager.instance.TriggerEvent(PlayerEventType.Debug_Survivor, this, true);

        survivors = 0;
        Complete_EscapeSurvivor();
    }

    /// <summary>
    /// 디버그 : 화재 완전 진화
    /// </summary>
    public void Debug_AllFiresExtinguished()
    {
        if (clear_ExtinguishedAllFires) return;

        EventManager.instance.TriggerEvent(PlayerEventType.Debug_Fire, this, true);
        fires = 0;
        uiManager.UI_FireRemains(0);
    }

    /// <summary>
    /// 디버그 : 게임 시간 증가
    /// </summary>
    private void Debug_IncreaseTime()
    {
        startedTime -= 10f;
    }

    /// <summary>
    /// 디버그 : 게임 시간 감소
    /// </summary>
    private void Debug_DecreaseTime()
    {
        startedTime += 10f;
        if (startedTime > Time.time) startedTime = Time.time;
    }

    /// <summary>
    /// 디버그 : 게임 시간 초기화
    /// </summary>
    private void Debug_ResetTime()
    {
        startedTime = Time.time;
    }
}
