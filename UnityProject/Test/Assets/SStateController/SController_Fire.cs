public class SController_Fire : SController
{
    public  FireState_Idle          state_Idle;
    public  FireState_Extinguish    state_Extinguish;
    public  FireState_Restore       state_Restore;
    public  FireState_Dead          state_Dead;

    public SController_Fire(Fire fire)
    {
        this.state_Idle         = new FireState_Idle(fire);
        this.state_Extinguish   = new FireState_Extinguish(fire);
        this.state_Restore      = new FireState_Restore(fire);
        this.state_Dead         = new FireState_Dead(fire);

        fire.isExtinguish       = false;
    }
}
