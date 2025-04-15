using UnityEngine;

public class Item_PortableLift: Item
{
    private bool isUsingItem;





    //-----------------< Initialize. 초기화 모음 >-----------------//

    public override void Init_Item()
    {
        base.Init_Item();

        listenerTypes = new PlayerEventType[]
        { PlayerEventType.a_UseItem2, PlayerEventType.p_UseItem2 };
    }





    //-----------------< Activity. 활동 모음 >-----------------//

    public override bool Check_isPossableUseItem()
    {
        if (isUsingItem)    return false;

        return true;
    }





    //-----------------< Event. 이벤트 모음 >-----------------//

    public override bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        switch (e_Type)
        {
            case PlayerEventType.a_UseItem2:
                isUsingItem = false;
                return true;

            case PlayerEventType.p_UseItem2 when Check_isPossableUseItem():
                TriggerEvent(PlayerEventType.Try_UseItem2, this, true);
                TriggerEvent(PlayerEventType.i_UseItem2, this, args);
                audio.UsePortableLift(true);
                isUsingItem = true;

                Debug.Log("사용 : PortableLift");
                return true;
        }
        return false;
    }
}
