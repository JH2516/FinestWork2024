using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    protected   StageManager    stageManager;
    [SerializeField]
    protected   Player          player;

    public void Init_Item() => Init();

    protected virtual void Init()
    {
        stageManager    = StageManager.stageManager;
        player          = stageManager.player;
    }

    public virtual bool Use_Item()
    {
        return false;
    }
}
