using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField]
    private uint numberSpawnedByDefault;

    [SerializeField]
    private Transform poolingRoot;

    [SerializeField]
    private PoolingDataScriptableObject poolingData;

    [SerializeField]
    private KeyCode spawnKeyCode;

    [SerializeField]
    private KeyCode despawnKeyCode;

    [SerializeField]
    private ITransformable transformable;

    private PoolingSystem poolingSystem;
    private PoolableObjectSpawner objectSpawner;

    private IInputHandler inputHandler;

    private void Awake()
    {
        poolingSystem = new PoolingSystem(poolingData, poolingRoot);
        objectSpawner = new PoolableObjectSpawner(poolingSystem);
        inputHandler = new DesktopInputHandler(spawnKeyCode, despawnKeyCode);
    }

    private void Start()
    {
        for (int i = 0; i < numberSpawnedByDefault; i++)
        {

        }
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
}