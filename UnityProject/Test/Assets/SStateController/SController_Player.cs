public class SController_Player : SController
{
    public  PlayerState_Idle    state_Idle;
    public  PlayerState_Walk    state_Walk;
    public  PlayerState_Attack  state_Attack;
    public  PlayerState_Dead    state_Dead;

    public SController_Player(Player player)
    {
        this.state_Idle     = new PlayerState_Idle(player);
        this.state_Walk     = new PlayerState_Walk(player);
        this.state_Attack   = new PlayerState_Attack(player);
        this.state_Dead     = new PlayerState_Dead(player);
    }
}
