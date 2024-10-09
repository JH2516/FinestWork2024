
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Stage : MonoBehaviour
{
    public Test_Player       player;






    public void Button_Move(int type) =>    player.Set_MoveVec(type);

    public void Button_isDown() =>          player.Set_isMove(true);
    public void Button_isUp() =>            player.Set_isMove(false);
}
