
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class StageManager : MonoBehaviour
{
    public Player           player;

    public GameObject       Panel_Pause;
    public GameObject       Panel_GameOver;
    public Image            player_HPBar;


    bool                    isGamePlay;
    private float                   player_HP;

    [SerializeField]
    bool                    isGameOver;

    public float Player_HP => player_HP;

    [Range(1, 10) ,SerializeField]
    private float decreaseHP = 2f;

    private void Start()
    {
        isGamePlay = true;
        isGameOver = false;
        player_HP = 100;
        player_HPBar.fillAmount = 1;
        Debug.Log(StageLoader.Stage);

        Panel_Pause.SetActive(false);
        Panel_GameOver.SetActive(false);
    }

    private void Update()
    {
        if (!isGamePlay) return;
        player_HP -= Time.deltaTime * decreaseHP;
        player_HPBar.fillAmount = player_HP / 100f;

        if (player_HP <= 0)
        {
            isGameOver = true;
            isGamePlay = false;
        }
    }


    public void Player_RecoveryHP(float fillHP)
    {
        player_HPBar.fillAmount = fillHP;
        player_HP = fillHP * 100;
    }



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

        room.gameObject.GetComponent<SpriteMask>().enabled = true;
        room.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void State_OutDarkedRoom(Collider2D room)
    {
        decreaseHP = 2;

        room.gameObject.GetComponent<SpriteMask>().enabled = false;
        room.gameObject.GetComponent<SpriteRenderer>().enabled = true;
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

    public void GameOver_BackDraft()
    {
        isGamePlay = false;
        isGameOver = true;
        player_HP = 0;
        player_HPBar.fillAmount = 0;

        Panel_GameOver.SetActive(true);
    }
}
