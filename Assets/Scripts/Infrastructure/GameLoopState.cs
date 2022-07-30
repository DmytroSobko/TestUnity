using System;
using System.Collections;
using UnityEngine;

public class GameLoopState : IState
{
    private readonly GameStateMachine stateMachine;

    private readonly ICoroutineRunner coroutineRunner;
    private readonly GameWorld gameWorld;
    private readonly ScreenUI screenUI;
    private readonly uint spawnByDefault;
    private readonly KeyCode spawnKeyCode;
    private readonly KeyCode despawnKeyCode;

    private readonly ISpawner<PoolableObject> objectSpawner;
    private readonly IInputHandler<KeyCode> inputHandler;

    private Coroutine updateCoroutine;

    public GameLoopState(GameStateMachine stateMachine, AllServices services, ICoroutineRunner coroutineRunner, GameWorld gameWorld, ScreenUI screenUI, uint spawnByDefault, KeyCode spawnKeyCode, KeyCode despawnKeyCode)
    {
        this.stateMachine = stateMachine;
        this.coroutineRunner = coroutineRunner;
        this.gameWorld = gameWorld;
        this.screenUI = screenUI;
        this.spawnByDefault = spawnByDefault;
        this.spawnKeyCode = spawnKeyCode;
        this.despawnKeyCode = despawnKeyCode;

        objectSpawner = services.Single<ISpawner<PoolableObject>>();
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
        inputHandler.InputTookPlace += OnInputTookPlace;
    }

    private void Unsubscribe()
    {
        objectSpawner.NumberOfSpawnedObjectsChanged -= OnNumberOfSpawnedObjectsChanged;
        inputHandler.InputTookPlace -= OnInputTookPlace;
    }

    private void SpawnDefault()
    {
        objectSpawner.Spawn<Cube>(spawnByDefault);
        objectSpawner.Spawn<Sphere>(spawnByDefault);
    }

    private IEnumerator Update()
    {
        while (true)
        {
            if (screenUI.IsInputFieldFocused)
                yield break;

            inputHandler.Update();

            yield return null;
        }
    }

    private void OnInputTookPlace(KeyCode keyCode)
    {
        Type poolableObjectType = screenUI.PoolingSystemDropdown.SelectedValue;
        var numberToSpawnOrDespawn = uint.Parse(screenUI.NumberOfObjectsToSpawnOrDespawn);

        if (keyCode == spawnKeyCode)
        {
            objectSpawner.Spawn(poolableObjectType, numberToSpawnOrDespawn);
        }
        else if (keyCode == despawnKeyCode)
        {
            objectSpawner.Despawn(poolableObjectType, numberToSpawnOrDespawn);
        }
    }

    private void OnNumberOfSpawnedObjectsChanged(int value)
    {
        screenUI.UpdateSpawnedObjectsText(value);
    }
}