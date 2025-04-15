using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item_PistolNozzle : Item
{
    public  TextMeshProUGUI text_CoolTime;
    public  Image           image_CoolTime;

    [SerializeField]
    private float   coolTime;
    [SerializeField]
    private float   remainTime;

    private bool    isCanUseItem;
    private bool    isUsingItem;





    //-----------------< Initialize. 초기화 모음 >-----------------//

    public override void Init_Item()
    {
        base.Init_Item();

        listenerTypes = new PlayerEventType[]
        { PlayerEventType.a_UseItem3, PlayerEventType.p_UseItem3 };

        coolTime = 30f;
        remainTime = 0;

        isCanUseItem = true;
        text_CoolTime.gameObject.SetActive(false);
    }





    //-----------------< Activity. 활동 모음 >-----------------//

    /// <summary>
    /// 아이템 대기 시간 갱신
    /// </summary>
    private IEnumerator CoolTiming()
    {
        isCanUseItem = false;
        remainTime = coolTime;
        text_CoolTime.gameObject.SetActive(true);

        while (true)
        {
            remainTime -= Time.deltaTime;
            text_CoolTime.text = $"{(int)remainTime}";
            image_CoolTime.fillAmount = (coolTime - remainTime) / coolTime;

            if (remainTime <= 0)
            {
                text_CoolTime.gameObject.SetActive(false);
                isCanUseItem = true;
                yield break;
            }

            yield return null;
        }
    }

    public override bool Check_isPossableUseItem()
    {
        if (!isCanUseItem)  return false;
        if (isUsingItem)    return false;

        return true;
    }





    //-----------------< Event. 이벤트 모음 >-----------------//

    public override bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        switch (e_Type)
        {
            case PlayerEventType.a_UseItem3 when Check_isPossableUseItem():
                isUsingItem = false;
                return true;

            case PlayerEventType.p_UseItem3 when Check_isPossableUseItem():
                TriggerEvent(PlayerEventType.Try_UseItem3, this, true);
                StartCoroutine(CoolTiming());
                isUsingItem = true;

                Debug.Log("사용 : PistolNozzle");
                return true;
        }
        return false;
    }
}
