using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class EventManager : MonoBehaviour
{
    public static EventManager instance
    { get; private set;}

    private Dictionary<PlayerEventType, List<IEventListener>> _eventListeners =
        new Dictionary<PlayerEventType, List<IEventListener>>();

    private Stack<int> removeIndex = new Stack<int>();





    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        DestroyImmediate(gameObject);
    }





    //-----------------< Event management. 이벤트 작업 >-----------------//

    /// <summary>
    /// 이벤트 타입 별 Listener 연결
    /// </summary>
    /// <param name="listener"> 이벤트를 수신할 PlayerEventType 객체 </param>
    /// <param name="types"> 이벤트 타입 </param>
    public void AddListener(IEventListener listener, params PlayerEventType[] types)
    {
        List<IEventListener> getList;

        for (int i = 0; i < types.Length; i++)
        {
            if (_eventListeners.TryGetValue(types[i], out getList))
            {
                getList.Add(listener); continue;
            }

            _eventListeners.Add(types[i], new List<IEventListener> { listener });
        }
            
    }

    /// <summary>
    /// 이벤트 타입 별 Listener 연결 해제
    /// </summary>
    /// <param name="listener"> 이벤트를 해제할 PlayerEventType 객체 </param>
    /// <param name="types"> 이벤트 타입 </param>
    public void RemoveListener(IEventListener listener, params PlayerEventType[] types)
    {
        List<IEventListener> getList;

        for (int i = 0; i < types.Length; i++)
        {
            if (!_eventListeners.TryGetValue(types[i], out getList)) continue;

            int idx = getList.IndexOf(listener);

            if (idx != -1)
                getList[idx] = null;
        }
    }

    /// <summary>
    /// 이벤트 타입 대상 Listener 모두 해제
    /// </summary>
    /// <param name="e_Type"> 이벤트 타입 </param>
    public void ClearListeners(PlayerEventType e_Type)
    {
        _eventListeners.Remove(e_Type);
    }

    /// <summary>
    /// 모든 이벤트 연결 내역 초기화
    /// </summary>
    public void ResetList()
    {
        _eventListeners.Clear();
    }





    //-----------------< Event Trigger. 이벤트 실행 >-----------------//

    /// <summary>
    /// 이벤트 타입 별 대상 Listener에게 이벤트 송신
    /// </summary>
    /// <param name="e_Type"> 이벤트 타입 </param>
    /// <param name="sender"> 발신자의 Component </param>
    /// <param name="args"> 추가 매개변수 </param>
    public void TriggerEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        List<IEventListener> listeners = null;
        if (!_eventListeners.TryGetValue(e_Type, out listeners)) return;

        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i]?.OnEvent(e_Type, sender, args);
            if (listeners[i] == null) removeIndex.Push(i);
        }

        while(removeIndex.Count > 0)
            listeners.RemoveAt(removeIndex.Pop());
    }

    /// <summary>
    /// 이벤트 타입 별 단일 대상 Listener에게 이벤트 송신
    /// </summary>
    /// <param name="e_Type"> 이벤트 타입 </param>
    /// <param name="sender"> 발신자의 Component </param>
    /// <param name="args"> 추가 매개변수 </param>
    public void TriggerEventForOneListener(PlayerEventType e_Type, Component sender, object args = null)
    {
        List<IEventListener> listeners = null;
        if (!_eventListeners.TryGetValue(e_Type, out listeners)) return;

        for (int i = 0; i < listeners.Count; i++)
            if (listeners[i]?.OnEvent(e_Type, sender, args) ?? false) break;
    }
}
