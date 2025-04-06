using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_CollapseAlarm: Item
{
    public static Item_CollapseAlarm item;

    [SerializeField]
    private bool    isUsed;

    public  bool    IsUsed => isUsed;

    protected override void Init()
    {
        base.Init();

        item = this;
        isUsed = false;
    }

    private void Start()
    {
        EventManager.instance.AddListener(this, PlayerEventType.p_UseItem1);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveListener(this, PlayerEventType.p_UseItem1);
    }

    public override bool Use_Item()
    {
        if (!Check_isPossableUseItem()) return false;

        //stageManager.player.Active_NavigateToCollapseRoom();
        //stageManager.SetActive_UIRemainCollapseRoom(true);
        //isUsed = true;

        return true;
    }

    private bool Check_isPossableUseItem()
    {
        if (isUsed)                                 return  false;
        if (player.using_CollapseAlarm)             return  false;
        if (player.transform_CollapseRoom == null)  return  false;

        return true;
    }

    public override bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        if (e_Type == PlayerEventType.p_UseItem1 && Check_isPossableUseItem())
        {
            // StageManager, Player 수신
            EventManager.instance.TriggerEvent(PlayerEventType.i_UseItem1, this, true);
            
            isUsed = true;

            Debug.Log("사용 : CollapseAlarm");
            return true;
        }

        return false;
    }
}
