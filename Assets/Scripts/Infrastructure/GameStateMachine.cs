using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine
{
    private readonly Dictionary<Type, IExitableState> states;
    private IExitableState activeState;

    public GameStateMachine(AllServices services, ICoroutineRunner coroutineRunner, GameWorld gameWorld, ScreenUI screenUI, PoolingDataScriptableObject poolingData, Transform poolingRoot, uint spawnByDefault, KeyCode spawnKeyCode, KeyCode despawnKeyCode)
    {
        states = new Dictionary<Type, IExitableState>()
        {
            [typeof(BootstrapState)] = new BootstrapState(this, services, gameWorld, poolingData, poolingRoot, spawnKeyCode, despawnKeyCode),
            [typeof(LoadGameState)] = new LoadGameState(this, services, screenUI),
            [typeof(GameLoopState)] = new GameLoopState(this, services, coroutineRunner, gameWorld, screenUI, spawnByDefault, spawnKeyCode, despawnKeyCode),
        };
    }

    public void Enter<TState>() where TState : class, IState
    {
        IState state = ChangeState<TState>();
        state.Enter();
    }

    public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
    {
        TState state = ChangeState<TState>();
        state.Enter(payload);
    }

    private TState ChangeState<TState>() where TState : class, IExitableState
    {
        activeState?.Exit();
        TState state = GetState<TState>();
        activeState = state;

        return state;
    }

    private TState GetState<TState>() where TState : class, IExitableState
    {
        return states[typeof(TState)] as TState;
    }
}