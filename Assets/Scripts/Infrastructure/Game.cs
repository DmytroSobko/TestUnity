public class Game
{
    public GameStateMachine StateMachine;

    public Game(ICoroutineRunner coroutineRunner, GameWorld gameWorld, GameUI gameUI, GameSetupScriptableObject gameSetup, PoolingDataScriptableObject poolingData)
    {
        StateMachine = new GameStateMachine(AllServices.Container, coroutineRunner, gameWorld, gameUI, gameSetup, poolingData);
    }
}