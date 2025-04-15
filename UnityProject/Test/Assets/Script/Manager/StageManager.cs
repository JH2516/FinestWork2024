using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(3)]
public class StageManager : MonoBehaviour, IEventListener, IEventTrigger
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
        PlayerEventType.p_Recovery, PlayerEventType.e_CollapseDone,
        PlayerEventType.f_Summon, PlayerEventType.f_Dead,
        PlayerEventType.s_Summon, PlayerEventType.s_Saved,
        PlayerEventType.c_isGameClear,
        PlayerEventType.d_Interact, PlayerEventType.d_DarkedRoom,
        PlayerEventType.g_GameOver,
        PlayerEventType.Debug,
        PlayerEventType.Try_UseItem1,
        PlayerEventType.i_UseItem1, PlayerEventType.i_UseItem2, PlayerEventType.i_UseItem3,
        PlayerEventType.UI_UseItem1, PlayerEventType.UI_UseItem2, PlayerEventType.UI_UseItem3,
    };





    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (stageManager == null) stageManager = this;

        Init_Argument();
        Init_Items();
        Init_Player();

        AddEvent(this, listenerTypes);

        uiManager.UIButton_ItemIsActive(UIItemImage.CollapseAlarm, false);
        uiManager.UIButton_ItemIsActive(UIItemImage.PistolNozzle, false);
        uiManager.UIButton_ItemIsActive(UIItemImage.PortableLift, false);

        // Stage 활성화 작업은 후반부
        stage = stageList[StageLoader.Stage];
        stage.SetActive(true);

        //Debug.Log("Stage" + StageLoader.Stage);
        //Debug.Log("Item" + StageLoader.Item);
    }

    private void Start()
    {
        GetComponent<PlayerInput>()?.SwitchCurrentControlScheme(Keyboard.current);

        uiManager.UI_PlayerHPAmount(100);
        SetActive_StageBoost();
    }

    private void Update()
    {
        if (isGameOver) return;
        if (isGamePause) return;

        Set_Timing();

        Set_decreaseHP();
        Check_PlayerHP();
    }





    //-----------------< Initialize. 초기화 모음 >-----------------//

    /// <summary>
    /// 초기화 : 필드
    /// </summary>
    private void Init_Argument()
    {
        isGamePlay = true;
        isGamePause = false;
        isGameOver = false;
        isRecoveryHP = false;
        isPlayerWarning = false;

        startedTime = Time.time;

        unUsedBoostItems = StageLoader.Item == 3;

        clear_ExtinguishedAllFires = false;
        clear_SavedAllSurvivors = false;

        isDebug = false;
        debug_isHPFreeze = false;

        Time.timeScale = 1;
    }

    /// <summary>
    /// 초기화 : 아이템
    /// </summary>
    private void Init_Items()
    {
        item_CollapseAlarm.Init_Item();
        item_PistolNozzle.Init_Item();
        item_PortableLift.Init_Item();
    }

    /// <summary>
    /// 초기화 : 플레이어 속성
    /// </summary>
    private void Init_Player()
    {
        player_HP = 100;
        player_HPMax = 100;
    }





    //-----------------< Input. 플레이어 입력 기능 모음 >-----------------//

    /// <summary>
    /// Button - 플레이어 이동 및 회전 갱신
    /// </summary>
    public void Button_Move(int type)
    {
        player.Set_MoveVec(type);
        player.Set_Rotation(type);
    }

    /// <summary>
    /// Button - 플레이어 정지
    /// </summary>
    public void Button_Stop()
    {
        player.Set_MoveVec(4);
    }

    /// <summary>
    /// Button - 입력 시 플레이어 이동 여부 활성화
    /// </summary>
    public void Button_isDown() => player.Set_isMove(true);

    /// <summary>
    /// Button - 입력 해제 시 플레이어 이동 여부 비활성화
    /// </summary>
    public void Button_isUp() => player.Set_isMove(false);

    /// <summary>
    /// Button - 일시정지
    /// </summary>
    public void Button_Pause() => Game_Pause();

    /// <summary>
    /// Button - 게임 재개
    /// </summary>
    public void Button_Resume() => Game_Resume();

    /// <summary>
    /// Button - 게임 재시작
    /// </summary>
    public void Button_Restart() => Game_Restart();

    /// <summary>
    /// Button - 게임 나가기
    /// </summary>
    public void Button_Exit() => Game_Exit();

    /// <summary>
    /// Button - 플레이어 메인 버튼 : 공격
    /// </summary>
    public void Button_ChangeToAttack()
    => uiManager.UIButton_EnableAttack();

    /// <summary>
    /// Button - 플레이어 메인 버튼 : 상호작용
    /// </summary>
    public void Button_ChangeToInteract()
    => uiManager.UIButton_EnableInteract();





    //-----------------< Activity. 활동 모음 >-----------------//

    /// <summary>
    /// 생존자 구출 작업 수행
    /// </summary>
    public void Save_Survivor()
    {
        survivors--;
    }

    /// <summary>
    /// 생존자 현장 구출 완료
    /// </summary>
    public void Complete_EscapeSurvivor()
    {
        uiManager.UI_SurvovirRemains(survivors);

        if (survivors == 0) clear_SavedAllSurvivors = true;
        if (Check_isGameClear()) GameClear();
    }

    /// <summary>
    /// 잔여 불 개수 갱신
    /// </summary>
    /// <param name="isSummond"> 불 생성 여부 </param>
    private void CountFireRemains(bool isSummond)
    {
        uiManager.UI_FireRemains(isSummond ? ++fires : --fires);

        if (!isSummond && fires == 0) clear_ExtinguishedAllFires = true;
    }

    /// <summary>
    /// 현장 내 구조 요청자 인원 갱신 (1인 증가)
    /// </summary>
    private void CountSurvivorRemains()
    {
        uiManager.UI_SurvovirRemains(++survivors);
    }

    /// <summary>
    /// 상호작용 감지 대상 개수 갱신
    /// </summary>
    /// <param name="isPlayerAround"> 플레이어의 접근 여부 </param>
    private void CountInteract(bool isPlayerAround)
    {
        _ = isPlayerAround ? ++interacts : --interacts;
        if (interacts == 0) Button_ChangeToAttack();
        else Button_ChangeToInteract();
    }





    //-----------------< Setting. 속성 설정 >-----------------//

    /// <summary>
    /// 플레이 시간 설정
    /// </summary>
    private void Set_Timing()
        => uiManager.UI_InGameTime(Time.time - startedTime);

    /// <summary>
    /// 플레이어 산소 감소
    /// </summary>
    private void Set_decreaseHP()
    {
        if (isRecoveryHP) return;
        if (debug_isHPFreeze) return;

        uiManager.UI_PlayerHPAmount(player_HP -= Time.deltaTime * decreaseHP);
    }

    /// <summary>
    /// 플레이어 산소 회복 설정
    /// </summary>
    public void Set_RecoveryHP(bool active)
        => isRecoveryHP = active;

    /// <summary>
    /// 플레이어 산소 감소량 증폭 설정
    /// </summary>
    /// <param name="isActive"> 활성화 여부 </param>
    private void Set_MoreHPDecrease(bool isActive)
    {
        decreaseHP = isActive ? decreaseHP *= 2.5f : decreaseHP /= 2.5f;
    }

    /// <summary>
    /// 플레이어 부스트 아이템 활성화 (미사용 시 비활성화)
    /// </summary>
    private void SetActive_StageBoost()
    {
        switch (StageLoader.Item)
        {
            case 0: SetBoost_SaveAllSurvivor(); break;
            case 1: SetBoost_IncreaseHPAmount(); break;
            case 2: SetBoost_IncreasePlayerLightFOVAngle(); break;
        }
    }

    /// <summary>
    /// 부스트 : 모든 생존자 구출
    /// </summary>
    public void SetBoost_SaveAllSurvivor()
    {
        TriggerEvent(PlayerEventType.b_Save, this);
        survivors = 0;
        uiManager.UI_SurvovirRemains(0);

        Complete_EscapeSurvivor();
    }

    /// <summary>
    /// 부스트 : 플레이어 산소량 증가
    /// </summary>
    public void SetBoost_IncreaseHPAmount()
    {
        player_HP = 150;
        player_HPMax = 150;
        
        uiManager.UI_EnablePlayerExtendHP();
        uiManager.UI_PlayerHPAmount(150);
    }

    /// <summary>
    /// 부스트 : 플레이어 전등 효과 강화
    /// </summary>
    public void SetBoost_IncreasePlayerLightFOVAngle()
    {
        TriggerEventOneListener(PlayerEventType.b_Light, this);
    }

    /// <summary>
    /// 플레이어 산소량 갱신
    /// </summary>
    public void SetPlayer_RemoteHP(float fillHP)
    {
        uiManager.UI_PlayerHPGauge(fillHP, player_HPMax);
        player_HP = fillHP * player_HPMax;
    }





    //-----------------< Check State. 상태 검사 >-----------------//

    /// <summary>
    /// 플레이어 산소량 검사
    /// </summary>
    private void Check_PlayerHP()
    {
        if (!isPlayerWarning && player_HP < player_HPMax / 10)
        {
            isPlayerWarning = true;
            StartCoroutine(WarinigEffect());
        }

        if (player_HP <= 0)
        {
            audio.GameoverByLowerOxygen(true);
            GameOver(GameOverType.LowerOxygen);
            uiManager.UI_PlayerHPAmount(0);
        }
    }

    /// <summary>
    /// 게임 완료 여부 검사
    /// </summary>
    /// <returns> True : 게임 완료, False 게임 미완료 </returns>
    public bool Check_isGameClear()
    {
        if (!clear_SavedAllSurvivors) return false;
        if (!clear_ExtinguishedAllFires) return false;

        return true;
    }





    //-----------------< Game State. 게임 상태 >-----------------//

    /// <summary>
    /// 게임 일시정지
    /// </summary>
    public void Game_Pause()
    {
        isGamePlay = false;
        isGamePause = true;

        blur.ButtonCaptureID(0);
        audio.PauseSound_WithoutButtonSound();
        audio.ButtonClick(true);
    }

    /// <summary>
    /// 게임 재개
    /// </summary>
    public void Game_Resume()
    {
        isGamePlay = true;
        isGamePause = false;
        Time.timeScale = 1;

        uiManager.UI_Panel(PanelType.GamePause, false);
        audio.UnPauseSound_WithoutButtonSound();
        audio.ButtonClick(true);
    }

    /// <summary>
    /// 게임 재시작
    /// </summary>
    public void Game_Restart()
    {
        RemoveEvent(this, listenerTypes);
        EventManager.instance.ResetList();
        SceneManager.LoadScene("Stage_Test");
    }

    /// <summary>
    /// 게임 나가기
    /// </summary>
    public void Game_Exit()
    {
        RemoveEvent(this, listenerTypes);
        EventManager.instance.ResetList();
        Time.timeScale = 1;
        SceneManager.LoadScene("Stage");
    }





    //-----------------< Game Result. 게임 결과 >-----------------//

    /// <summary>
    /// 게임 완료
    /// </summary>
    public void GameClear()
    {
        float resultTime = Time.time - startedTime;

        StopAllCoroutines();

        isGamePlay = false;
        int clearRank = 0;

        clearTimeInCondition =
        (resultTime < sO_Stage.stageClearTime[StageLoader.Stage]) ? true : false;

        if (unUsedBoostItems) clearRank++;
        if (clearTimeInCondition) clearRank++;


        blur.ButtonCaptureID(2); // 게임 클리어 화면 표기 지연
        audio.StopSound_AllSounds();

        uiManager.UI_GameClear(unUsedBoostItems, clearTimeInCondition, clearRank);
    }

    /// <summary>
    /// 게임 오버
    /// </summary>
    /// <param name="type"> 게임 오버 사유 타입 </param>
    public void GameOver(GameOverType type)
    {
        StopAllCoroutines();

        isGamePlay = false;
        isGameOver = true;
        player_HP = 0;

        player.Player_KnockOut();

        blur.ButtonCaptureID(1); // 게임오버 화면 표기 지연
        FadeOutAudio_BurningAround();

        uiManager.UI_GameOver(type);
    }

    public void GameOver_PanelOn()
    {
        Time.timeScale = 0;
        uiManager.UI_Panel(PanelType.GameOver, true);
    }

    public void GamePause_PanelOn()
    {
        Time.timeScale = 0;
        uiManager.UI_Panel(PanelType.GamePause, true);
    }

    public void GameClear_PanelOn()
    {
        Time.timeScale = 0;
        uiManager.UI_Panel(PanelType.GameClear, true);
    }






    //-----------------< Audio. 오디오 작업 모음 >-----------------//

    /// <summary>
    /// 오디오 FadeIn - 화재 인근 플레이어가 접근 시
    /// </summary>
    public void FadeInAudio_BurningAround() =>
    StartCoroutine(FadeInAudio(audio.audio_BurningAround));

    /// <summary>
    /// 오디오 FadeOut - 화재 인근 플레이어가 퇴출 시
    /// </summary>
    public void FadeOutAudio_BurningAround() =>
    StartCoroutine(FadeOutAudio(audio.audio_BurningAround));

    /// <summary>
    /// 오디오 FadeIn
    /// </summary>
    /// <param name="audio"> 적용 대상 오디오 AudioSource </param>
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

    /// <summary>
    /// 오디오 FadeOut
    /// </summary>
    /// <param name="audio"> 적용 대상 오디오 AudioSource </param>
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





    //-----------------< Effect. 효과 및 연출 모음 >-----------------//

    /// <summary>
    /// 플레이어 산소 고갈 위험 효과 연출
    /// </summary>
    private IEnumerator WarinigEffect()
    {
        WaitForFixedUpdate wf = new WaitForFixedUpdate();

        while (player_HP <= player_HPMax / 10)
        {
            uiManager.UI_WarinigEffect(player_HP, player_HPMax);
            yield return wf;
        }

        isPlayerWarning = false;
        uiManager.UI_WarinigEffect(100, 100);
        yield break;
    }





    //-----------------< Event. 이벤트 모음 >-----------------//

    public bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        switch (e_Type)
        {
            case PlayerEventType.f_Dead:        CountFireRemains(false);        return true;
            case PlayerEventType.f_Summon:      CountFireRemains(true);         return true;

            case PlayerEventType.s_Summon:      CountSurvivorRemains();         return true;
            case PlayerEventType.s_Saved when (bool)args:
                Save_Survivor();
                return true;

            case PlayerEventType.c_isGameClear:
                if (Check_isGameClear())
                    GameClear();
                return true;


            case PlayerEventType.d_Interact:    CountInteract((bool)args);      return true;
            case PlayerEventType.d_DarkedRoom:  Set_MoreHPDecrease((bool)args); return true;
            case PlayerEventType.p_Recovery:
                if (!(bool)args) Complete_EscapeSurvivor();
                Set_RecoveryHP((bool)args);
                return true;

            case PlayerEventType.e_CollapseDone:
                uiManager.UI_CollapseRoomRemain(false);
                return true;


            case PlayerEventType.g_GameOver:    GameOver((GameOverType)args);   return true;

            case PlayerEventType.i_UseItem1:
                used_CollapseAlarm = true;
                return true;

            case PlayerEventType.i_UseItem2:
                
                return true;

            case PlayerEventType.i_UseItem3:
                
                return true;

            case PlayerEventType.Try_UseItem1:
                uiManager.UIButton_ItemIsActive(UIItemImage.CollapseAlarm, false, true);
                uiManager.UI_CollapseRoomRemain(true);
                return true;

            case PlayerEventType.UI_UseItem1:
                uiManager.UIButton_ItemIsActive(UIItemImage.CollapseAlarm, (bool)args);
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

    public void AddEvent(IEventListener listener, params PlayerEventType[] types)
    => EventManager.instance.AddListener(listener, types);

    public void RemoveEvent(IEventListener listener, params PlayerEventType[] types)
    => EventManager.instance.RemoveListener(listener, types);

    public void TriggerEvent(PlayerEventType e_Type, Component sender, object args = null)
    => EventManager.instance.TriggerEvent(e_Type, sender, args);

    public void TriggerEventOneListener(PlayerEventType e_Type, Component sender, object args = null)
    => EventManager.instance.TriggerEventForOneListener(e_Type, sender, args);





    //-----------------< Debug. 디버그 모음 >-----------------//

    /// <summary>
    /// 디버그 실행
    /// </summary>
    /// <param name="context"></param>
    public void OnDebug(InputAction.CallbackContext context)
    {
        if (context.started)
            TriggerDebug(sO_Stage.debugKeyList[context.control.path.Substring(10)]);
    }

    /// <summary>
    /// 디버그 실행 목록
    /// </summary>
    /// <param name="key"> 디버그 할당 키 </param>
    private void TriggerDebug(KeyCode key)
    {
        if (!isGamePlay) return;

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

            default:
                Debug.LogError($"디버그 오류 : 할당되어있지 않거나 잘못된 형식의 키");
                break;
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

        TriggerEvent(PlayerEventType.Debug_Survivor, this, true);

        survivors = 0;
        Complete_EscapeSurvivor();
    }

    /// <summary>
    /// 디버그 : 화재 완전 진화
    /// </summary>
    public void Debug_AllFiresExtinguished()
    {
        if (clear_ExtinguishedAllFires) return;

        TriggerEvent(PlayerEventType.Debug_Fire, this, true);
        fires = 0;
        clear_ExtinguishedAllFires = true;
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
