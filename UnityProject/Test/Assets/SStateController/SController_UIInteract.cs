public class SController_UIInteract : SController
{
    public  UIInteractState_Idle    state_Idle;
    public  UIInteractState_Working state_Working;
    public  UIInteractState_Done    state_Done;

    public SController_UIInteract(UIInteract ui)
    {
        this.state_Idle     = new UIInteractState_Idle(ui);
        this.state_Working  = new UIInteractState_Working(ui);
        this.state_Done     = new UIInteractState_Done(ui);
    }
}
