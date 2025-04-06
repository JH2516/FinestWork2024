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

    private void Start()
    {
        EventManager.instance.AddListener(this, PlayerEventType.p_UseItem2);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveListener(this, PlayerEventType.p_UseItem1);
    }

    public override bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        if (e_Type == PlayerEventType.p_UseItem2)
        {
            bool isCanUse = Check_isPossableUseItem();
            EventManager.instance.TriggerEvent(PlayerEventType.Try_UseItem2, this, isCanUse);

            if (isCanUse)
            {
                EventManager.instance.TriggerEvent(PlayerEventType.i_UseItem2, this, args);
                audio.UsePortableLift(true);

                Debug.Log("사용 : PortableLift");
            }

            return true;
        }

        return false;
    }
}
