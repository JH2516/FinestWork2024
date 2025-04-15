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

    /// <summary>
    /// 이벤트 추가
    /// </summary>
    /// <param name="types"> 이벤트를 수신할 타입 모음 </param>
    void AddEvent(IEventListener listener, params PlayerEventType[] types);

    /// <summary>
    /// 이벤트 제거
    /// </summary>
    /// <param name="types"> 이벤트를 제거할 타입 모음 </param>
    void RemoveEvent(IEventListener listener, params PlayerEventType[] types);
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
 *  a : Active
 *  b : Boost
 *  c : Check
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
    p_Interact, p_Recovery,
    p_UseItem1, p_UseItem2, p_UseItem3,

    f_Summon, f_Dead,
    s_Summon, s_Saved,

    i_UseItem1, i_UseItem2, i_UseItem3,
    a_UseItem1, a_UseItem2, a_UseItem3, 
    Try_UseItem1, Try_UseItem2, Try_UseItem3,
    UI_UseItem1, UI_UseItem2, UI_UseItem3,

    b_Light, b_Save,
    c_isGameClear,
    d_Collapse, d_Interact, d_DarkedRoom,
    e_CollapseSoon, e_CollapseDone,

    g_GameOver,

    Debug, Debug_Fire, Debug_Survivor
}
