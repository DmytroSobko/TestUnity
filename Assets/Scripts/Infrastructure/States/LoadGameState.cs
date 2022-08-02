public class LoadGameState : IState
{
    private readonly GameStateMachine stateMachine;
    private readonly GameUI gameUI;
    private readonly GameSetupScriptableObject gameSetup;
    private readonly PoolingDataScriptableObject poolingData;

    public LoadGameState(GameStateMachine stateMachine, GameUI gameUI, GameSetupScriptableObject gameSetup, PoolingDataScriptableObject poolingData)
    {
        this.stateMachine = stateMachine;
        this.gameUI = gameUI;
        this.gameSetup = gameSetup;
        this.poolingData = poolingData;
    }

    public void Enter()
    {
        gameUI.Init(gameSetup, poolingData);
        stateMachine.Enter<GameLoopState>();
    }

    public void Exit()
    {

    }
}