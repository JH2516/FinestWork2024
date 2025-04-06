using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IEventListener
{
    [SerializeField]
    protected   StageManager    stageManager;
    [SerializeField]
    protected   AudioManager    audio;
    [SerializeField]
    protected   Player          player;

    public void Init_Item() => Init();

    protected virtual void Init()
    {
        stageManager    = StageManager.stageManager;
        audio           = stageManager.audio;
        player          = stageManager.player;
    }

    public virtual bool Use_Item()
    {
        return false;
    }

    public virtual bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        return false;
    }
}
