using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_PortableLift: Item
{
    public static Item_PortableLift item;

    protected override void Init()
    {
        base.Init();

        item = this;
    }

    public override bool Use_Item()
    {
        if (!Check_isPossableUseItem()) return false;

        player.SetActive_UsingPortableLift(true);
        audio.UsePortableLift(true);

        return true;
    }

    private bool Check_isPossableUseItem()
    {
        if (!player.isRemoveCollapse)       return false;
        if (player.using_PortableLift)      return false;
        if (player.target_Collapse == null) return false;

        return true;
    }
}
