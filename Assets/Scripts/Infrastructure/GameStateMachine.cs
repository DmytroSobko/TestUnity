using System;
using System.Collections.Generic;

public class GameStateMachine
{
    private readonly Dictionary<Type, IState> states;
    private IState activeState;

    public GameStateMachine(AllServices services, ICoroutineRunner coroutineRunner, GameWorld gameWorld, GameUI gameUI, GameSetupScriptableObject gameSetup, PoolingDataScriptableObject poolingData)
    {
        states = new Dictionary<Type, IState>()
        {
            [typeof(BootstrapState)] = new BootstrapState(this, services, gameWorld, gameSetup, poolingData),
            [typeof(LoadGameState)] = new LoadGameState(this, gameUI, gameSetup, poolingData),
            [typeof(GameLoopState)] = new GameLoopState(this, services, coroutineRunner, gameWorld, gameUI, gameSetup),
        };
    }

    public void Enter<TState>() where TState : class, IState
    {
        IState state = ChangeState<TState>();
        state.Enter();
    }

    private TState ChangeState<TState>() where TState : class, IState
    {
        activeState?.Exit();
        TState state = GetState<TState>();
        activeState = state;

        return state;
    }

    private TState GetState<TState>() where TState : class, IState
    {
        return states[typeof(TState)] as TState;
    }
}