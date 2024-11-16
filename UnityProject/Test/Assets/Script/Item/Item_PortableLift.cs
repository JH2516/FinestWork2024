using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_PortableLift: Item
{
    public static Item_PortableLift item;


    private void Awake()
    {
        item = this;
    }

    public override bool Use_Item()
    {
        return false;
    }
}
