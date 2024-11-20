
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using TMPro;

public class StageManager : MonoBehaviour
{
    public  static  StageManager    stageManager;

    [Header("UI")]
    public  GameObject      Panel_Pause;
    public  GameObject      Panel_GameOver;
    public  GameObject      Panel_GameClear;
    public  GameObject      ui_PlayerExtendHPBar;
    public  GameObject      ui_RemainCollapseRoom;
    public  Image           player_HPBar;
    public  Image           Player_ExtendHPBar;
    public  Image           backGround_HPBar;
    public  Image           backGround_ExtendHPBar;
    public  TextMeshProUGUI text_survivorRemain;
    public  TextMeshProUGUI text_CollapseRoomRemain;

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

    [Header("Survivors")]
    [SerializeField]
    private List<GameObject>    List_Survivors;

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

    [Header("Survivor Status")]
    [SerializeField]
    private int             survivors;

    public  float           Player_HP   =>  player_HP;
    public  float           Player_HPMax => player_HPMax;
    public  bool            IsGamePlay  =>  isGamePlay;

    [Header("Stage Clear")]
    private byte            clearStars;

    [Header("Stage Clear Require")]
    private float           clearTime = 100;
    private bool            unUsedBoostItems;

    private void Awake()
    {
        if (stageManager == null) stageManager = this;

        Init_Argument();
        Init_Items();
        Init_Player();
        Init_Survivors();
        Init_UI();

        Active_StageBoost();

        Debug.Log("Stage" + StageLoader.Stage);
        Debug.Log("Item" + StageLoader.Item);
    }

    /// <summary> 초기화 : 변수 </summary>
    private void Init_Argument()
    {
        isGamePlay = true;
        isGameOver = false;
        isRecoveryHP = false;

        time_InGame = 0;

        unUsedBoostItems = (StageLoader.Item == 99) ? true : false;

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
        Player_ExtendHPBar.fillAmount = 1;
    }

    /// <summary> 초기화 : 생존자들 </summary>
    private void Init_Survivors()
    {
        List_Survivors = new List<GameObject>();

        foreach (Transform survivor in all_Survivors)
        List_Survivors.Add(survivor.gameObject);

        survivors = List_Survivors.Count;
        text_survivorRemain.text = survivors.ToString();
    }

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
        Player_ExtendHPBar.fillAmount = player_HP / 150;
    }

    /// <summary> 플레이어 산소량 검사 </summary>
    private void Check_PlayerHP()
    {
        if (player_HP <= 0) GameOver();
    }

    /// <summary> 플레이어 산소량 갱신 </summary>
    public void Player_RemoteHP(float fillHP)
    {
        player_HPBar.fillAmount = fillHP * (player_HPMax / 100);
        Player_ExtendHPBar.fillAmount = fillHP;
        player_HP = fillHP * player_HPMax;
    }

    /// <summary> 플레이어 산소 회복 설정 </summary>
    public void Set_RecoveryHP(bool active) => isRecoveryHP = active;






    /// <summary> 생존자 구출 작업 수행 </summary>
    public void Save_Survivor(GameObject survivor)
    {
        List_Survivors.Remove(survivor);
        survivors--;
    }

    /// <summary> 생존자 현장 구출 완료 </summary>
    public void Complete_EscapeSurvivor()
    {
        text_survivorRemain.text = survivors.ToString();
    }



    /// <summary> 부스트 : 모든 생존자 구출 </summary>
    public void Boost_SaveAllSurvivor()
    {
        foreach (GameObject survivor in List_Survivors)
            survivor.SetActive(false);

        List_Survivors.Clear();
        survivors = 0;
        text_survivorRemain.text = survivors.ToString();
    }

    /// <summary> 부스트 : 플레이어 산소 감소 (50%) </summary>
    public void Boost_IncreaseHPAmount()
    {
        player_HP = 150;
        player_HPMax = 150;
        backGround_HPBar.enabled = false;
        backGround_ExtendHPBar.enabled = true;
        ui_PlayerExtendHPBar.SetActive(true);
    }

    public void Boost_IncreasePlayerLightFOVAngle()
    {
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
    }

    /// <summary> 게임 재개 </summary>
    public void Game_Resume()
    {
        isGamePlay = true;
        Time.timeScale = 1;

        Panel_Pause.SetActive(false);
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
        isGamePlay = false;
        isGameOver = true;
        player_HP = 0;
        player_HPBar.fillAmount = 0;
        Time.timeScale = 0;

        Panel_GameOver.SetActive(true);
    }

    /// <summary> 게임 결과 출력 </summary>
    public void GameClear()
    {
        isGamePlay = false;
        Time.timeScale = 0;

        clearStars = 1;

        clearTime = 0;

        if (unUsedBoostItems)           clearStars++;
        if (time_InGame < clearTime)    clearStars++;

        Debug.Log(clearStars);

        Panel_GameClear.SetActive(true);
    }
}
