
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class StageManager : MonoBehaviour
{
    public  static  StageManager    stageManager;

    public  GameObject      stage_Test;

    [Header("Manager")]
    public  AudioManager    audio;

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

    //[Header("UI : Background")]
    //public  Image           ui_Survivor;
    //public  Image           ui_Fires;

    [Header("UI : Text")]
    public  TextMeshProUGUI text_FireRemain;
    public  TextMeshProUGUI text_SurvivorRemain;
    public  TextMeshProUGUI text_CollapseRoomRemain;

    [Header("UI : Clear Text")]
    public  TextMeshProUGUI text_GameClear;
    public  TextMeshProUGUI text_UnusedBoostItem;
    public  TextMeshProUGUI text_InTime;
    public  TextMeshProUGUI text_ClearStars;
    
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

    [Header("Boost Item Use")]
    public  bool            used_AirMat;
    public  bool            used_OxygenTank;
    public  bool            used_Lantern;

    //[Header("Survivors")]
    //[SerializeField]
    //private List<GameObject>    List_Survivors;

    [Header("Stage Status")]
    [SerializeField]
    private bool            isGamePlay;
    [SerializeField]
    private bool            isGameOver;
    [SerializeField]
    private bool            isRecoveryHP;
    [SerializeField]
    private float           time_InGame;

    [Header("Player HP Info")]
    public  float           player_HP;
    private float           player_HPMax;

    [Header("Stage Unit")]
    [SerializeField]
    private int             survivors;
    [SerializeField]
    private int             fires;
    [SerializeField]
    private int             max_Countfires;

    public  float           Player_HP   =>  player_HP;
    public  float           Player_HPMax => player_HPMax;
    public  bool            IsGamePlay  =>  isGamePlay;
    public  bool            IsGameOver  =>  isGameOver;

    [Header("Stage Clear")]
    private bool            clear_SavedAllSurvivors;
    private bool            clear_ExtinguishedAllFires;
    private byte            clearStars;

    [Header("Stage Clear Require")]
    private float           clearTime = 100;
    private bool            unUsedBoostItems;

    public void Count_Fires(int count)
    {
        max_Countfires = fires += count;
        text_FireRemain.text = fires.ToString();
    }

    public void Count_Survivors()
    {
        survivors++;
        text_SurvivorRemain.text = survivors.ToString();
    }

    private void Awake()
    {
        if (stageManager == null) stageManager = this;

        Init_Argument();
        Init_Items();
        Init_Player();
        //Init_Survivors();
        Init_UI();

        Active_StageBoost();

        Debug.Log("Stage" + StageLoader.Stage);
        Debug.Log("Item" + StageLoader.Item);

        stage_Test.SetActive(true);
    }

    /// <summary> 초기화 : 변수 </summary>
    private void Init_Argument()
    {
        isGamePlay = true;
        isGameOver = false;
        isRecoveryHP = false;

        used_AirMat = false;
        used_OxygenTank = false;
        used_Lantern = false;

        time_InGame = 0;

        unUsedBoostItems = (StageLoader.Item == 3) ? true : false;

        clear_ExtinguishedAllFires = false;
        clear_SavedAllSurvivors = false;

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
        player_HPBar.fillAmount = 1;
        player_ExtendHPBar.fillAmount = 1;
    }

    ///// <summary> 초기화 : 생존자들 </summary>
    //private void Init_Survivors()
    //{
    //    List_Survivors = new List<GameObject>();

    //    foreach (Transform survivor in all_Survivors)
    //    List_Survivors.Add(survivor.gameObject);

    //    survivors = List_Survivors.Count;
    //    text_SurvivorRemain.text = survivors.ToString();
    //}

    /// <summary> 초기화 : UI </summary>
    private void Init_UI()
    {
        Panel_Pause.SetActive(false);
        Panel_GameOver.SetActive(false);
        Panel_GameClear.SetActive(false);
        backGround_HPBar.enabled = true;
        backGround_ExtendHPBar.enabled = false;
        ui_PlayerExtendHPBar.SetActive(false);
        ui_RemainCollapseRoom.SetActive(false);

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

    private void Update()
    {
        if (isGameOver) return;

        Timing();

        Set_decreaseHP();
        Check_PlayerHP();
    }

    private void Timing()
    {
        time_InGame += Time.deltaTime;
    }

    /// <summary> 플레이어 산소 감소 </summary>
    private void Set_decreaseHP()
    {
        if (isRecoveryHP) return;
        player_HP -= Time.deltaTime * decreaseHP;
        player_HPBar.fillAmount = player_HP / 100;
        player_ExtendHPBar.fillAmount = player_HP / 150;
    }

    /// <summary> 플레이어 산소량 검사 </summary>
    private void Check_PlayerHP()
    {
        if (player_HP <= 0)
        {
            audio.GameoverByLowerOxygen(true);
            GameOver();
        }
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
        text_SurvivorRemain.text = survivors.ToString();
        Check_SavedAllSurvivors();
    }

    private void Check_SavedAllSurvivors()
    {
        if (survivors != 0) return;

        text_SurvivorRemain.color = Color.green;
        clear_SavedAllSurvivors = true;
    }



    public void Discount_Fires()
    {
        fires--;
        text_FireRemain.text = fires.ToString();
        Check_isExtinguishedAllFires();
    }

    private void Check_isExtinguishedAllFires()
    {
        if (fires != 0) return;

        text_FireRemain.color = Color.green;
        clear_ExtinguishedAllFires = true;
    }


    /// <summary> 부스트 : 모든 생존자 구출 </summary>
    public void Boost_SaveAllSurvivor()
    {
        used_AirMat = true;
        survivors = 0;
        text_SurvivorRemain.text = survivors.ToString();

        Check_SavedAllSurvivors();
    }

    /// <summary> 부스트 : 플레이어 산소 감소 (50%) </summary>
    public void Boost_IncreaseHPAmount()
    {
        used_OxygenTank = true;
        player_HP = 150;
        player_HPMax = 150;
        backGround_HPBar.enabled = false;
        backGround_ExtendHPBar.enabled = true;
        ui_PlayerExtendHPBar.SetActive(true);
        mask_HPBar.sizeDelta = new Vector2(1185, 150);
    }

    public void Boost_IncreasePlayerLightFOVAngle()
    {
        used_Lantern = true;
        player.Set_LightFOVAngle(60f, 90f);
        player.Set_LightFOVRadius(8f);
        player.Set_LightAroundRadius(2f, 8f);
        player.change_FOVAngleRange = 1.5f;
    }



    public bool UseItem_CollapseAlarm()
    {
        return item_CollapseAlarm.Use_Item();
    }

    public bool UseItem_PistolNozzle()
    {
        return item_PistolNozzle.Use_Item();
    }

    public bool UseItem_PortableLift()
    {
        return item_PortableLift.Use_Item();
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


    
    /// <summary> 게임 일시정지 </summary>
    public void Game_Pause()
    {
        isGamePlay = false;
        Time.timeScale = 0;

        Panel_Pause.SetActive(true);
        audio.ButtonClick(true);
    }

    /// <summary> 게임 재개 </summary>
    public void Game_Resume()
    {
        isGamePlay = true;
        Time.timeScale = 1;

        Panel_Pause.SetActive(false);
        audio.ButtonClick(true);
    }

    /// <summary> 게임 재시작 </summary>
    public void Game_Restart()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary> 게임 나가기 </summary>
    public void Game_Exit()
    {
        SceneManager.LoadScene("Stage");
    }

    /// <summary> 게임 오버 </summary>
    public void GameOver()
    {
        StopAllCoroutines();

        isGamePlay = false;
        isGameOver = true;
        player_HP = 0;
        player_HPBar.fillAmount = 0;
        Time.timeScale = 0;

        Panel_GameOver.SetActive(true);
        FadeOutAudio_BurningAround();
    }

    public bool Check_isGameClear()
    {
        if (!clear_SavedAllSurvivors)       return false;
        if (!clear_ExtinguishedAllFires)    return false;

        GameClear();
        return true;
    }

    /// <summary> 게임 결과 출력 </summary>
    public void GameClear()
    {
        isGamePlay = false;
        Time.timeScale = 0;

        clearStars = 1;
        GameClear_ShowStarAcquire();
        
        text_ClearStars.text = $"별 {clearStars}개";

        Panel_GameClear.SetActive(true);
    }

    private void GameClear_ShowStarAcquire()
    {
        text_GameClear.color        = Color.green;
        text_UnusedBoostItem.color  = new Color(1, 1, 1, 0.5f);
        text_InTime.color           = new Color(1, 1, 1, 0.5f);

        if (unUsedBoostItems)
        {
            text_UnusedBoostItem.color = Color.green;
            clearStars++;
        }

        if (time_InGame < clearTime)
        {
            text_InTime.color = Color.green;
            clearStars++;
        }
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
            yield return null; // 다음 프레임까지 대기
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
            yield return null; // 다음 프레임까지 대기
        }
    }
}
