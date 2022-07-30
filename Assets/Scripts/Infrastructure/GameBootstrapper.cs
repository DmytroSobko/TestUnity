using UnityEngine;

public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
{
    [SerializeField]
    private GameWorld gameWorld;

    [SerializeField]
    private ScreenUI screenUI;

    [SerializeField]
    private PoolingDataScriptableObject poolingData;

    [SerializeField]
    private Transform poolingRoot;

    [SerializeField]
    private uint spawnByDefault;

    [SerializeField]
    private KeyCode spawnKeyCode;

    [SerializeField]
    private KeyCode despawnKeyCode;

    private Game game;

    private void Start()
    {
        game = new Game(this, gameWorld, screenUI, poolingData, poolingRoot, spawnByDefault, spawnKeyCode, despawnKeyCode);
        game.StateMachine.Enter<BootstrapState>();

        DontDestroyOnLoad(this);
    }
}