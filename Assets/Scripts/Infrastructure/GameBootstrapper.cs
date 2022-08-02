using UnityEngine;

public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
{
    [SerializeField]
    private GameWorld gameWorld;

    [SerializeField]
    private GameUI gameUI;

    [SerializeField]
    private GameSetupScriptableObject gameSetup;

    [SerializeField]
    private PoolingDataScriptableObject poolingData;

    private Game game;

    private void Start()
    {
        game = new Game(this, gameWorld, gameUI, gameSetup, poolingData);
        game.StateMachine.Enter<BootstrapState>();

        DontDestroyOnLoad(this);
    }
}