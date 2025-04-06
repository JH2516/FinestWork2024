using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item_PistolNozzle : Item
{
    public static Item_PistolNozzle item;

    public  TextMeshProUGUI text_CoolTime;
    public  Image           image_CoolTime;

    [SerializeField]
    private float   coolTime;
    [SerializeField]
    private float   remainTime;
    [SerializeField]
    private bool    isCanUseItem;

    protected override void Init()
    {
        base.Init();

        coolTime = 30f;
        remainTime = 0;
        item = this;

        isCanUseItem = true;
        text_CoolTime.gameObject.SetActive(false);
    }

    private void Start()
    {
        EventManager.instance.AddListener(this, PlayerEventType.p_UseItem3);
    }

    private void Update()
    {
        if (isCanUseItem) return;
        Timing();
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveListener(this, PlayerEventType.p_UseItem1);
    }

    private void Timing()
    {
        remainTime -= Time.deltaTime;
        text_CoolTime.text = $"{(int)remainTime}";
        image_CoolTime.fillAmount = (coolTime - remainTime) / coolTime;

        if (remainTime <= 0)
        {
            text_CoolTime.gameObject.SetActive(false);
            isCanUseItem = true;
        }
    }

    public void Used_Item()
    {
        text_CoolTime.gameObject.SetActive(true);
        isCanUseItem = false;
    }

    public override bool Use_Item()
    {
        if (!Check_isPossableUseItem()) return false;

        player.SetActive_UsingPistolNozzle(true);
        remainTime = coolTime;

        return true;
    }

    private bool Check_isPossableUseItem()
    {
        if (!player.isInFrontOfDoor)                return false;
        if (player.target_BackDraft == null)        return false;
        if (!isCanUseItem)  return false;

        return true;
    }

    public override bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        if (e_Type == PlayerEventType.p_UseItem3 && Check_isPossableUseItem())
        {
            player.SetActive_UsingPistolNozzle(true);
            remainTime = coolTime;

            Debug.Log("사용 : PistolNozzle");
            return true;
        }

        return false;
    }
}
