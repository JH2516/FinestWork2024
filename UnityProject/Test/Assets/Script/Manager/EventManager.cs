using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class EventManager : MonoBehaviour
{
    public static EventManager instance
    { get; private set;}

    private Dictionary<PlayerEventType, List<IEventListener>> _eventListeners =
        new Dictionary<PlayerEventType, List<IEventListener>>();

    private Stack<int> removeIndex = new Stack<int>();

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

    public void ClearListeners(PlayerEventType e_Type)
    {
        _eventListeners.Remove(e_Type);
    }

    public void ResetList()
    {
        _eventListeners.Clear();
    }

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

    public void TriggerEventForOneListener(PlayerEventType e_Type, Component sender, object args = null)
    {
        List<IEventListener> listeners = null;
        if (!_eventListeners.TryGetValue(e_Type, out listeners)) return;

        for (int i = 0; i < listeners.Count; i++)
            if (listeners[i]?.OnEvent(e_Type, sender, args) ?? false) break;
    }
}
