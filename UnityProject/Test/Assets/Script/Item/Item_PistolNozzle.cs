using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_PistolNozzle : Item
{
    public static Item_PistolNozzle item;


    private void Awake()
    {
        item = this;
    }

    public override bool Use_Item()
    {
        return false;
    }
}
