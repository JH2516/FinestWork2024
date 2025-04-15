using UnityEngine;

public interface IEventTrigger
{
    /// <summary>
    /// 대상에게 이벤트 전송
    /// </summary>
    /// <param name="e_Type"> 대상 이벤트 타입 </param>
    /// <param name="sender"> 송신자 Component </param>
    /// <param name="args"> 추가 정보 </param>
    void TriggerEvent(PlayerEventType e_Type, Component sender, object args = null);

    /// <summary>
    /// 단일 대상에게 이벤트 전송
    /// </summary>
    /// <param name="e_Type"> 대상 이벤트 타입 </param>
    /// <param name="sender"> 송신자 Component </param>
    /// <param name="args"> 추가 정보 </param>
    void TriggerEventOneListener(PlayerEventType e_Type, Component sender, object args = null);
}