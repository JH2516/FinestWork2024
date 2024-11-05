
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

    [Header("Light")]
    public Light2D          light_Global;

    [Header("Player")]
    public Player           player;

    [Header("Game Option")]
    [Range(1, 10), SerializeField]
    private float decreaseHP = 2f;

    private bool            isGamePlay;
    private bool            isGameOver;
    private bool            isRecoveryHP;
    private float           player_HP;

    public  float           Player_HP   =>  player_HP;
    public  bool            IsGamePlay  =>  isGamePlay;

    private void Awake()
    {
        Init_Argument();
        Init_Player();
        Init_UI();
        Debug.Log(StageLoader.Stage);
    }

    private void Init_Argument()
    {
        isGamePlay = true;
        isGameOver = false;
        isRecoveryHP = false;
        Time.timeScale = 1;
    }

    private void Init_Player()
    {
        player_HP = 100;
        player_HPBar.fillAmount = 1;
    }

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

    private void Set_decreaseHP()
    {
        if (isRecoveryHP) return;
        player_HP -= Time.deltaTime * decreaseHP;
        player_HPBar.fillAmount = player_HP / 100f;
    }

    private void Check_PlayerHP()
    {
        if (player_HP <= 0) GameOver();
    }


    public void Player_RemoteHP(float fillHP)
    {
        player_HPBar.fillAmount = fillHP;
        player_HP = fillHP * 100;
    }

    public void Set_RecoveryHP(bool active) => isRecoveryHP = active;


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


    public void State_InDarkedRoom(Collider2D room)
    {
        decreaseHP = 5;

        room.GetComponent<SpriteMask>().enabled = true;
        room.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void State_OutDarkedRoom(Collider2D room)
    {
        decreaseHP = 2;

        room.GetComponent<SpriteMask>().enabled = false;
        room.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Game_Pause()
    {
        isGamePlay = false;
        Panel_Pause.SetActive(true);
    }

    public void Game_Resume()
    {
        isGamePlay = true;
        Panel_Pause.SetActive(false);
    }

    public void Game_Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Game_Exit()
    {
        SceneManager.LoadScene("Stage");
    }

    public void GameOver()
    {
        isGamePlay = false;
        isGameOver = true;
        player_HP = 0;
        player_HPBar.fillAmount = 0;

        Panel_GameOver.SetActive(true);
    }
}
