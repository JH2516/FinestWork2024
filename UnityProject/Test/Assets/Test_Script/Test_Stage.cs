
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Test_Stage : MonoBehaviour
{
    public Test_Player      player;

    public GameObject       Panel_Pause;
    public Image            player_HPBar;


    bool                    isGamePlay;
    float                   player_HP;

    private void Start()
    {
        isGamePlay = true;
        player_HP = 100;
        player_HPBar.fillAmount = 1;
    }

    private void Update()
    {
        if (!isGamePlay) return;
        player_HP -= Time.deltaTime * 2;
        player_HPBar.fillAmount = player_HP / 100f;
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
}
