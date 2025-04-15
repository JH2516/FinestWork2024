using UnityEngine;

public class Interactor_Survivor : Interactor
{
    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    protected override void Awake()
    {
        Init_UIInteract(InteractorType.Survivor);
        base.Awake();
        AddEvent(this, PlayerEventType.b_Save);
    }

    private void Start()
    {
        TriggerEvent(PlayerEventType.s_Summon, this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        RemoveEvent(this, PlayerEventType.b_Save, PlayerEventType.Debug_Survivor);
    }





    //-----------------< Interact. 상호작용 모음 >-----------------//

    public override void Start_Interact()
    {
        base.Start_Interact();
        TriggerEvent(PlayerEventType.s_Saved, this, false);
    }

    public override void Done_Interact()
    {
        TriggerEvent(PlayerEventType.s_Saved, this, true);
        base.Done_Interact();
    }





    //-----------------< Event. 이벤트 모음 >-----------------//

    public override bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        switch (e_Type)
        {
            case PlayerEventType.b_Save:
            case PlayerEventType.Debug_Survivor:
                gameObject.SetActive(false);
                return true;

            default:
                if ((sender as Interactor_Survivor) == this)
                    return base.OnEvent(e_Type, sender, args);

                return false;
        }
    }
}
