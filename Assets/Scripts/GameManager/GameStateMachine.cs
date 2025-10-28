public class GameStateMachine
{
    public IState CurrentState;

    public void ChangeState(IState nextState)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }
        CurrentState = nextState;
        CurrentState.Enter();
    }
}
