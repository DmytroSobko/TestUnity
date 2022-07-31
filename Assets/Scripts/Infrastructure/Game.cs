public class Game
{
    public GameStateMachine StateMachine;

    public Game(ICoroutineRunner coroutineRunner, GameWorld gameWorld, GameUI gameUI, GameSetupScriptableObject gameSetupData, PoolingDataScriptableObject poolingData)
    {
        StateMachine = new GameStateMachine(AllServices.Container, coroutineRunner, gameWorld, gameUI, gameSetupData, poolingData);
    }
}