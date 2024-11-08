
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class StageManager : MonoBehaviour
{
    [Header("UI")]
    public  GameObject      Panel_Pause;
    public  GameObject      Panel_GameOver;
    public  Image           player_HPBar;
    public  SurviverUpdate  surviver_Text;

    [Header("Light")]
    public  Light2D         light_Global;

    [Header("Player")]
    public  Player          player;

    [Header("Game Option")]
    [Range(1, 10), SerializeField]
    private float           decreaseHP = 2f;

    [Header("All Survivors")]
    [SerializeField]
    private Transform           all_Survivors;

    [Header("All CollapseRooms")]
    [SerializeField]
    private Transform           all_CollapseRooms;

    [SerializeField]
    private List<GameObject>    List_Survivors;

    private bool            isGamePlay;
    private bool            isGameOver;
    private bool            isRecoveryHP;
    private float           player_HP;
    [SerializeField]
    private int             survivors;

    public  float           Player_HP   =>  player_HP;
    public  bool            IsGamePlay  =>  isGamePlay;

    private void Awake()
    {
        Init_Argument();
        Init_Player();
        Init_Survivors();
        Init_UI();
        Debug.Log("Stage" + StageLoader.Stage);
        Debug.Log("Item" + StageLoader.Item);



    }

    /// <summary> 초기화 : 변수 </summary>
    private void Init_Argument()
    {
        isGamePlay = true;
        isGameOver = false;
        isRecoveryHP = false;
        Time.timeScale = 1;
    }

    /// <summary> 초기화 : 플레이어 속성 </summary>
    private void Init_Player()
    {
        player_HP = 100;
        player_HPBar.fillAmount = 1;
    }

    /// <summary> 초기화 : 생존자들 </summary>
    private void Init_Survivors()
    {
        List_Survivors = new List<GameObject>();

        foreach (Transform survivor in all_Survivors)
        List_Survivors.Add(survivor.gameObject);

        survivors = List_Survivors.Count;
        surviver_Text.SetSurviverNumber(survivors);
    }

    /// <summary> 초기화 : UI </summary>
    private void Init_UI()
    {
        Panel_Pause.SetActive(false);
        Panel_GameOver.SetActive(false);
    }

    private void Update()
    {
        if (isGameOver) return;

        Set_decreaseHP();
        Check_PlayerHP();
    }

    /// <summary> 플레이어 산소 감소 </summary>
    private void Set_decreaseHP()
    {
        if (isRecoveryHP) return;
        player_HP -= Time.deltaTime * decreaseHP;
        player_HPBar.fillAmount = player_HP / 100f;
    }

    /// <summary> 플레이어 산소량 검사 </summary>
    private void Check_PlayerHP()
    {
        if (player_HP <= 0) GameOver();
    }

    /// <summary> 플레이어 산소량 갱신 </summary>
    public void Player_RemoteHP(float fillHP)
    {
        player_HPBar.fillAmount = fillHP;
        player_HP = fillHP * 100;
    }

    /// <summary> 플레이어 산소 회복 설정 </summary>
    public void Set_RecoveryHP(bool active) => isRecoveryHP = active;






    /// <summary> 생존자 구출 </summary>
    public void Save_Survivor(GameObject survivor)
    {
        List_Survivors.Remove(survivor);
        survivors--;
        surviver_Text.SetSurviverNumber(survivors);
    }



    /// <summary> 부스트 : 모든 생존자 구출 </summary>
    public void Boost_SaveAllSurvivor()
    {
        List_Survivors.Clear();
        survivors = 0;
    }

    /// <summary> 부스트 : 플레이어 산소 감소 (50%) </summary>
    public void Boost_decreaseHPHalfOff()
    {
        decreaseHP /= 2;
    }



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
}
