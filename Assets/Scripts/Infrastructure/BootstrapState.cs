using System.Collections.Generic;
using UnityEngine;

public class BootstrapState : IState
{
    private readonly GameStateMachine stateMachine;

    private readonly GameWorld gameWorld;
    private readonly AllServices services;
    private readonly GameSetupScriptableObject gameSetupData;
    private readonly PoolingDataScriptableObject poolingData;

    public BootstrapState(GameStateMachine stateMachine, AllServices services, GameWorld gameWorld, GameSetupScriptableObject gameSetupData, PoolingDataScriptableObject poolingData)
    {
        this.stateMachine = stateMachine;
        this.services = services;
        this.gameWorld = gameWorld;
        this.poolingData = poolingData;
        this.gameSetupData = gameSetupData;

        RegisterServices();
    }

    public void Enter()
    {
        stateMachine.Enter<LoadGameState>();
    }

    public void Exit()
    {

    }

    private void RegisterServices()
    {
        services.RegisterSingle(new UpdatableService());
        services.RegisterSingle<ISpawnerResource<Shape>>(new PoolingSystem<Shape>(poolingData));
        services.RegisterSingle<ISpawner<Shape>>(new ObjectSpawner<Shape>(services.Single<ISpawnerResource<Shape>>(), gameWorld));
        services.RegisterSingle<IInputHandler<KeyCode>>(new DesktopInputHandler(new List<KeyCode>
        {
            gameSetupData.SpawnKeyCode, gameSetupData.DespawnKeyCode
        }));

        services.Single<UpdatableService>().AddUpdatable(services.Single<IInputHandler<KeyCode>>());
    }
}