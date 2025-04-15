public class SController
{
    public IState currentState { get; private set; }

    /// <summary>
    /// 초기 상태 설정
    /// </summary>
    /// <param name="startState"> 시작할 IState </param>
    public void Initialize(IState startState)
    {
        currentState = startState;
        startState.Enter();
    }

    /// <summary>
    /// 기존 상태를 끝내고 다음 상태로 변경
    /// </summary>
    /// <param name="nextState"> 변경할 IState </param>
    public void ChangeState(IState nextState)
    {
        currentState.Exit();
        currentState = nextState;
        nextState?.Enter();
    }

    /// <summary>
    /// 상태 종료
    /// </summary>
    public void TurnOffState()
    {
        currentState.Exit();
        currentState = null;
    }

    /// <summary>
    /// 매 프레임 상태 별 Update 실행
    /// </summary>
    public void Update()
    {
        currentState?.Update();
    }
}
