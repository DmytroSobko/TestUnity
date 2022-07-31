public class LoadGameState : IState
{
    private readonly GameStateMachine stateMachine;
    private readonly GameUI gameUI;
    private readonly PoolingDataScriptableObject poolingData;

    public LoadGameState(GameStateMachine stateMachine, GameUI gameUI, PoolingDataScriptableObject poolingData)
    {
        this.stateMachine = stateMachine;
        this.gameUI = gameUI;
        this.poolingData = poolingData;
    }

    public void Enter()
    {
        gameUI.PoolingSystemDropdown.Init(poolingData.PoolsData);
        stateMachine.Enter<GameLoopState>();
    }

    public void Exit()
    {

    }
}