using Unity.VisualScripting;

public class SController
{
    public IState currentState { get; private set; }

    public void Initialize(IState startState)
    {
        currentState = startState;
        startState.Enter();
    }

    public void ChangeState(IState nextState)
    {
        currentState.Exit();
        currentState = nextState;
        nextState?.Enter();
    }

    public void TurnOffState()
    {
        currentState.Exit();
        currentState = null;
    }

    public void Update()
    {
        currentState?.Update();
    }
}
