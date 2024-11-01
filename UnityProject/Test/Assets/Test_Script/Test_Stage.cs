
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class Test_Stage : MonoBehaviour
{
    public Test_Player      player;

    public GameObject       Panel_Pause;
    public GameObject       Panel_GameOver;
    public Image            player_HPBar;


    bool                    isGamePlay;
    float                   player_HP;

    [SerializeField]
    bool                    isGameOver;

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
        player_HP -= Time.deltaTime * 2;
        player_HPBar.fillAmount = player_HP / 100f;

        if (player_HP <= 0)
        {
            isGameOver = true;
            isGamePlay = false;
        }
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
