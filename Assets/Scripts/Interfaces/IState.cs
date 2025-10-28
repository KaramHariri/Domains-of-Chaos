public interface IState
{
    public void Awake();
    public void Enter();
    public void Execute();
    public void LateExectue();
    public void Exit();
}
