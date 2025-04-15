using UnityEngine;

public class Item_CollapseAlarm: Item
{
    [SerializeField]
    private bool    isUsed;





    //-----------------< Initialize. 초기화 모음 >-----------------//

    public override void Init_Item()
    {
        base.Init_Item();

        listenerTypes = new PlayerEventType[]
        { PlayerEventType.a_UseItem1, PlayerEventType.p_UseItem1 };
    }





    //-----------------< Activity. 활동 모음 >-----------------//

    public override bool Check_isPossableUseItem()
    {
        if (isUsed)         return  false;

        return true;
    }





    //-----------------< Event. 이벤트 모음 >-----------------//

    public override bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        switch (e_Type)
        {
            case PlayerEventType.a_UseItem1:
                isUsed = false;
                return true;

            case PlayerEventType.p_UseItem1 when Check_isPossableUseItem():
                TriggerEvent(PlayerEventType.i_UseItem1, this, true);
                isUsed = true;

                Debug.Log("사용 : CollapseAlarm");
                return true;
        }
        return false;
    }
}
