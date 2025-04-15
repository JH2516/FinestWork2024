public interface IState
{
    /// <summary>
    /// 상태 진입 시 1회 실행
    /// </summary>
    public void Enter();

    /// <summary>
    /// 매 프레임 상태에 따른 Update 실행
    /// </summary>
    public void Update();

    /// <summary>
    /// 상태 마무리 시 1회 실행
    /// </summary>
    public void Exit();
}