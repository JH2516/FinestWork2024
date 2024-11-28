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

    public override bool Use_Item()
    {
        if (!Check_isPossableUseItem()) return false;

        stageManager.player.Active_NavigateToCollapseRoom();
        stageManager.SetActive_UIRemainCollapseRoom(true);
        isUsed = true;

        return true;
    }

    private bool Check_isPossableUseItem()
    {
        if (isUsed)                                 return  false;
        if (player.using_CollapseAlarm)             return  false;
        if (player.transform_CollapseRoom == null)  return  false;

        return true;
    }
}
