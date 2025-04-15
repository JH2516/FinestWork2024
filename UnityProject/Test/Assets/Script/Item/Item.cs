using UnityEngine;

public class Item : MonoBehaviour, IEventListener, IEventTrigger
{
    [SerializeField]
    protected AudioManager audio;

    protected PlayerEventType[] listenerTypes;





    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    public void Start()
    {
        AddEvent(this, listenerTypes);
    }

    private void OnDestroy()
    {
        RemoveEvent(this, listenerTypes);
    }





    //-----------------< Initialize. 초기화 모음 >-----------------//

    /// <summary>
    /// Item 초기화
    /// </summary>
    public virtual void Init_Item()
    {
        audio = StageManager.stageManager.audio;
    }





    //-----------------< Activity. 활동 모음 >-----------------//

    /// <summary>
    /// 아이템 사용 가능 여부 검사
    /// </summary>
    /// <returns> True : 사용 가능, False : 사용 불가능 </returns>
    public virtual bool Check_isPossableUseItem()
    {
        return false;
    }





    //-----------------< Event. 이벤트 모음 >-----------------//

    public virtual bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        return false;
    }

    public void AddEvent(IEventListener listener, params PlayerEventType[] types)
    => EventManager.instance.AddListener(listener, types);

    public void RemoveEvent(IEventListener listener, params PlayerEventType[] types)
    => EventManager.instance.RemoveListener(listener, types);

    public void TriggerEvent(PlayerEventType e_Type, Component sender, object args = null)
    => EventManager.instance.TriggerEvent(e_Type, sender, args);

    public void TriggerEventOneListener(PlayerEventType e_Type, Component sender, object args = null)
    => EventManager.instance.TriggerEventForOneListener(e_Type, sender, args);
}
