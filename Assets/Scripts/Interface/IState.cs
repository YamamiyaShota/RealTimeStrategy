public interface IState
{
    public StateType GetCurrentState { get; }
    public StateType GetNextState { get; }
    /// <summary>
    /// ステートを設定する
    /// </summary>
    /// <param name="currentState">今の状態</param>
    /// <param name="nextState">次の状態</param>
    public void SetState(StateType currentState, StateType nextState = StateType.Idle);
    public void OnStateBegine();
    public void OnStateEnd();

    public void Update();
}

public enum StateType
{
    Idle,
    Move,
    Attack,
    Death,
}
