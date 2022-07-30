using System.Collections.Generic;
using UnityEngine;

public class BootstrapState : IState
{
    private readonly GameStateMachine stateMachine;

    private readonly GameWorld gameWorld;
    private readonly AllServices services;
    private readonly PoolingDataScriptableObject poolingData;
    private readonly Transform poolingRoot;
    private readonly KeyCode spawnKeyCode;
    private readonly KeyCode despawnKeyCode;

    public BootstrapState(GameStateMachine stateMachine, AllServices services, GameWorld gameWorld, PoolingDataScriptableObject poolingData, Transform poolingRoot, KeyCode spawnKeyCode, KeyCode despawnKeyCode)
    {
        this.stateMachine = stateMachine;
        this.services = services;
        this.gameWorld = gameWorld;
        this.poolingData = poolingData;
        this.poolingRoot = poolingRoot;
        this.spawnKeyCode = spawnKeyCode;
        this.despawnKeyCode = despawnKeyCode;

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
        services.RegisterSingle(new PoolingSystem(poolingData, poolingRoot));
        services.RegisterSingle<ISpawner<PoolableObject>>(new PoolableObjectSpawner(services.Single<PoolingSystem>(), gameWorld));
        services.RegisterSingle<IInputHandler<KeyCode>>(new DesktopInputHandler(new List<KeyCode>
        {
            spawnKeyCode, despawnKeyCode
        }));
    }
}