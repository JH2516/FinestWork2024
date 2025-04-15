using UnityEngine;

public class Interactor_Recovery : Interactor
{
    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    protected override void Awake()
    {
        Init_UIInteract(InteractorType.Recovery);
        base.Awake();
    }





    //-----------------< Interact. 상호작용 모음 >-----------------//

    protected override void Show_Interact()
    {
        base.Show_Interact();
        Start_Interact();
    }

    protected override void Hide_Interact()
    {
        base.Hide_Interact();
        TriggerEvent(PlayerEventType.p_Recovery, this, false);
        show_Interaction.Request_Stop();
    }

    public override void Start_Interact()
    {
        if (isInteraction) return;

        TriggerEvent(PlayerEventType.p_Recovery, this, true);
        show_Interaction.Request_Start(stageManager.Player_HP / stageManager.Player_HPMax * 100);
        isInteraction = true;
    }

    public override void Done_Interact()
    {
        show_Interaction.Init();
        isInteraction = false;

        TriggerEvent(PlayerEventType.p_Recovery, this, false);
    }





    //-----------------< Event. 이벤트 모음 >-----------------//

    public override bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        if ((sender as Interactor_Recovery) == this)
            return base.OnEvent(e_Type, sender, args);

        return false;
    }
}
