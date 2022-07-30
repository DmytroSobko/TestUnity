public class LoadGameState : IState
{
    private readonly GameStateMachine stateMachine;
    private readonly PoolingSystem poolingSystem;
    private readonly ScreenUI screenUI;

    public LoadGameState(GameStateMachine stateMachine, AllServices services, ScreenUI screenUI)
    {
        this.stateMachine = stateMachine;
        this.screenUI = screenUI;

        poolingSystem = services.Single<PoolingSystem>();
    }

    public void Enter()
    {
        screenUI.PoolingSystemDropdown.Init(poolingSystem.ObjectPools);

        stateMachine.Enter<GameLoopState>();
    }

    public void Exit()
    {

    }
}