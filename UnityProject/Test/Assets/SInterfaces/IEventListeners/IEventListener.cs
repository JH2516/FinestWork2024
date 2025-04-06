using UnityEngine;


public interface IEventListener
{
    /// <summary>
    /// 이벤트 실행
    /// </summary>
    /// <param name="e_Type"> 이벤트 타입 </param>
    /// <param name="sender"> 이벤트 호출자 Component </param>
    /// <param name="args"> 이벤트 매개변수 </param>
    /// <returns></returns>
    bool OnEvent(PlayerEventType e_Type, Component sender, object args = null);
}

/*  PlayerEventType Tags
 * 
 *  <Unit>
 *  p : Player
 *  f : Fire
 *  s : Survivor
 *  i : Item
 *  
 *  <Activity>
 *  b : Boost
 *  d : Detected
 *  e : Event Activate
 *  t : Target
 *  Try
 *  UI
 *  
 *  <Stage>
 *  g_GameOver
 *  
 *  <Debug>
 *  Debug
 */

public enum PlayerEventType
{
    p_StartAttack, p_EndAttack,
    p_Interact,
    p_UseItem1, p_UseItem2, p_UseItem3,

    f_Summon, f_Dead,
    s_Summon, s_Saved,

    i_UseItem1, i_UseItem2, i_UseItem3,
    Try_UseItem1, Try_UseItem2, Try_UseItem3,
    UI_UseItem1, UI_UseItem2, UI_UseItem3,

    b_Light, b_Save,
    d_Collapse, d_Interact, d_DarkedRoom,
    e_CollapseSoon, e_CollapseDone,

    g_GameOver,

    Debug, Debug_Fire
}
