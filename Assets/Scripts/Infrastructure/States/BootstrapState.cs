using System.Collections.Generic;
using UnityEngine;

public class BootstrapState : IState
{
    private readonly GameStateMachine stateMachine;

    private readonly GameWorld gameWorld;
    private readonly AllServices services;
    private readonly GameSetupScriptableObject gameSetup;
    private readonly PoolingDataScriptableObject poolingData;

    public BootstrapState(GameStateMachine stateMachine, AllServices services, GameWorld gameWorld, GameSetupScriptableObject gameSetup, PoolingDataScriptableObject poolingData)
    {
        this.stateMachine = stateMachine;
        this.services = services;
        this.gameWorld = gameWorld;
        this.poolingData = poolingData;
        this.gameSetup = gameSetup;

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
        services.RegisterSingle<ISpawner<Shape>>(new ObjectSpawner<Shape>(new PoolingSystem<Shape>(poolingData), gameWorld));
        services.RegisterSingle<InputHandler<KeyCode>>(new DesktopInputHandler(new List<KeyCode>
        {
            gameSetup.SpawnKeyCode,
            gameSetup.DespawnKeyCode
        }));
    }
}