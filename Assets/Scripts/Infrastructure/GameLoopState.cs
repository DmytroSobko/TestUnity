using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class GameLoopState : IState
{
    private const string SPAWN_COMMAND = "Spawn";
    private const string DESPAWN_COMMAND = "Despawn";

    private readonly GameStateMachine stateMachine;

    private readonly ICoroutineRunner coroutineRunner;
    private readonly GameWorld gameWorld;
    private readonly GameUI gameUI;
    private readonly GameSetupScriptableObject gameSetupData;

    private readonly UpdatableService updatableService;
    private readonly ISpawner<Shape> objectSpawner;
    private readonly IInputHandler<KeyCode> inputHandler;

    private Coroutine updateCoroutine;

    public GameLoopState(GameStateMachine stateMachine, AllServices services, ICoroutineRunner coroutineRunner, GameWorld gameWorld, GameUI gameUI, GameSetupScriptableObject gameSetupData)
    {
        this.stateMachine = stateMachine;
        this.coroutineRunner = coroutineRunner;
        this.gameWorld = gameWorld;
        this.gameUI = gameUI;
        this.gameSetupData = gameSetupData;

        updatableService = services.Single<UpdatableService>();
        objectSpawner = services.Single<ISpawner<Shape>>();
        inputHandler = services.Single<IInputHandler<KeyCode>>();
    }
     
    public void Enter()
    {
        Subscribe();
        SpawnDefault();

        updateCoroutine = coroutineRunner.StartCoroutine(Update());
    }

    public void Exit()
    {
        Unsubscribe();

        coroutineRunner.StopCoroutine(updateCoroutine);
    }

    private void Subscribe()
    {
        objectSpawner.NumberOfSpawnedObjectsChanged += OnNumberOfSpawnedObjectsChanged;
        objectSpawner.ObjectSpawned += OnObjectSpawned;
        objectSpawner.ObjectDespawned += OnObjectDespawned;
        inputHandler.InputTookPlace += OnInputTookPlace;
    }

    private void Unsubscribe()
    {
        objectSpawner.NumberOfSpawnedObjectsChanged -= OnNumberOfSpawnedObjectsChanged;
        objectSpawner.ObjectSpawned -= OnObjectSpawned;
        objectSpawner.ObjectDespawned -= OnObjectDespawned;
        inputHandler.InputTookPlace -= OnInputTookPlace;
    }

    private void SpawnDefault()
    {
        objectSpawner.Spawn<Cube>(gameSetupData.SpawnByDefault);
        objectSpawner.Spawn<Sphere>(gameSetupData.SpawnByDefault);
    }

    private IEnumerator Update()
    {
        while (true)
        {
            updatableService.Update();

            yield return null;
        }
    }

    private void OnObjectSpawned(Shape spawned)
    {
        spawned.RandomMovement.SetBounds(gameWorld.Bounds);
        updatableService.AddUpdatable(spawned);
    }

    private void OnObjectDespawned(Shape despawned)
    {
        updatableService.RemoveUpdatable(despawned);
    }

    private void OnInputTookPlace(KeyCode keyCode)
    {
        if (keyCode == gameSetupData.SpawnKeyCode)
        {
            SpawnOrDespawn(SPAWN_COMMAND);
        }
        else if (keyCode == gameSetupData.DespawnKeyCode)
        {
            SpawnOrDespawn(DESPAWN_COMMAND);
        }
    }

    private void SpawnOrDespawn(string action)
    {
        Type poolableObjectType = gameUI.PoolingSystemDropdown.SelectedValue;
        var numberToSpawnOrDespawn = int.Parse(gameUI.NumberOfObjectsToSpawnOrDespawn);

        MethodInfo method = objectSpawner
            .GetType()
            .GetMethod(action)
            .MakeGenericMethod(new Type[]
            {
                poolableObjectType
            });

        method.Invoke(objectSpawner, new object[]
        {
            numberToSpawnOrDespawn
        });
    }

    private void OnNumberOfSpawnedObjectsChanged(int value)
    {
        gameUI.UpdateSpawnedObjectsText(value);
    }
}