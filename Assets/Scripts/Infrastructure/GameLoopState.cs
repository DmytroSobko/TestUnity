using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private readonly GameSetupScriptableObject gameSetup;

    private readonly UpdatableService updatableService;
    private readonly ISpawner<Shape> objectSpawner;
    private readonly IInputHandler<KeyCode> inputHandler;

    private List<Shape> activeObjects = new List<Shape>();

    private Coroutine updateCoroutine;

    public GameLoopState(GameStateMachine stateMachine, AllServices services, ICoroutineRunner coroutineRunner, GameWorld gameWorld, GameUI gameUI, GameSetupScriptableObject gameSetup)
    {
        this.stateMachine = stateMachine;
        this.coroutineRunner = coroutineRunner;
        this.gameWorld = gameWorld;
        this.gameUI = gameUI;
        this.gameSetup = gameSetup;

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
        objectSpawner.Spawn<Cube>(gameSetup.SpawnByDefault);
        objectSpawner.Spawn<Sphere>(gameSetup.SpawnByDefault);
    }

    private IEnumerator Update()
    {
        while (true)
        {
            updatableService.Update();

            yield return null;
        }
    }

    private void OnNumberOfSpawnedObjectsChanged(int value)
    {
        gameUI.UpdateSpawnedObjectsNumberText(value);
    }

    private void OnObjectSpawned(Shape spawned)
    {
        spawned.RandomMovement.SetBounds(gameWorld.Bounds);
        updatableService.AddUpdatable(spawned);
        AddNeighbourToActiveObjects(spawned);
        SetNeighboursForNewcomer(spawned);
        activeObjects.Add(spawned);
    }

    private void OnObjectDespawned(Shape despawned)
    {
        updatableService.RemoveUpdatable(despawned);
        activeObjects.Remove(despawned);
        RemoveNeighbourFromActiveObjects(despawned);
    }

    private void SetNeighboursForNewcomer(Shape newcomer)
    {
        List<Transform> newcomerNeighbours = activeObjects.Select(neighbour => neighbour.Transform).ToList();
        newcomer.FindNearestNeighbour.SetNeighbours(newcomerNeighbours);
    }

    private void AddNeighbourToActiveObjects(Shape neighbour)
    {
        foreach (Shape activeObject in activeObjects)
        {
            activeObject.FindNearestNeighbour.AddNeighbour(neighbour.Transform);
        }
    }

    private void RemoveNeighbourFromActiveObjects(Shape neighbour)
    {
        foreach (Shape activeObject in activeObjects)
        {
            activeObject.FindNearestNeighbour.RemoveNeighbour(neighbour.Transform);
        }
    }

    private void OnInputTookPlace(KeyCode keyCode)
    {
        if (gameUI.IsInputFieldFocused)
        {
            return;
        }

        if (keyCode == gameSetup.SpawnKeyCode)
        {
            SpawnOrDespawn(SPAWN_COMMAND);
        }
        else if (keyCode == gameSetup.DespawnKeyCode)
        {
            SpawnOrDespawn(DESPAWN_COMMAND);
        }
    }

    private void SpawnOrDespawn(string action)
    {
        try
        {
            Type poolableObjectType = gameUI.PoolingSystemDropdown.SelectedValue;
            var numberToSpawnOrDespawn = int.Parse(gameUI.NumberOfObjectsToSpawnOrDespawn);

            MethodInfo method = objectSpawner
                .GetType()
                .GetMethod(action)
                .MakeGenericMethod(new Type[] { poolableObjectType });

            method.Invoke(objectSpawner, new object[] { numberToSpawnOrDespawn });
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}