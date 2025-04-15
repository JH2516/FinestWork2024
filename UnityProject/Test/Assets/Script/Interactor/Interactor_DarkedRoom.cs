using UnityEngine;

public class Interactor_DarkedRoom : Interactor
{
    public SpriteMask       mask;
    public SpriteRenderer   sr;





    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    protected override void Awake()
    {
        AddEvent(this, PlayerEventType.d_Interact);
    }

    protected override void OnDestroy()
    {
        RemoveEvent(this, PlayerEventType.d_Interact);
    }





    //-----------------< Interact. 상호작용 모음 >-----------------//

    protected override void Show_Interact()
    {
        mask.enabled = true;
        sr.enabled = false;
        TriggerEvent(PlayerEventType.d_DarkedRoom, this, true);
    }

    protected override void Hide_Interact()
    {
        mask.enabled = false;
        sr.enabled = true;
        TriggerEvent(PlayerEventType.d_DarkedRoom, this, false);
    }





    //-----------------< Event. 이벤트 모음 >-----------------//

    public override bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        switch (e_Type)
        {
            default:
                if ((sender as Interactor_DarkedRoom) == this)
                    return base.OnEvent(e_Type, sender, args);

                return false;
        }
    }
}
