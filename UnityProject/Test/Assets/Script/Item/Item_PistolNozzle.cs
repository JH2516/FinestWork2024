using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_PistolNozzle : Item
{
    public static Item_PistolNozzle item;

    [SerializeField]
    private float   coolTime;
    [SerializeField]
    private float   remainTime_UsePistolNozzle;

    protected override void Init()
    {
        base.Init();

        coolTime = 30f;
        remainTime_UsePistolNozzle = 30f;
        item = this;
    }

    private void Update()
    {
        Timing();
    }

    private void Timing()
    {
        remainTime_UsePistolNozzle += Time.deltaTime;
    }

    public override bool Use_Item()
    {
        if (!Check_isPossableUseItem()) return false;

        player.SetActive_UsingPistolNozzle(true);
        remainTime_UsePistolNozzle = 0f;

        return true;
    }

    private bool Check_isPossableUseItem()
    {
        if (!player.isInFrontOfDoor)                return false;
        if (player.target_BackDraft == null)        return false;
        if (remainTime_UsePistolNozzle < coolTime)  return false;

        return true;
    }
}
