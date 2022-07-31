using System;
using System.Collections.Generic;

public class GameStateMachine
{
    private readonly Dictionary<Type, IExitableState> states;
    private IExitableState activeState;

    public GameStateMachine(AllServices services, ICoroutineRunner coroutineRunner, GameWorld gameWorld, GameUI gameUI, GameSetupScriptableObject gameSetupData, PoolingDataScriptableObject poolingData)
    {
        states = new Dictionary<Type, IExitableState>()
        {
            [typeof(BootstrapState)] = new BootstrapState(this, services, gameWorld, gameSetupData, poolingData),
            [typeof(LoadGameState)] = new LoadGameState(this, gameUI, poolingData),
            [typeof(GameLoopState)] = new GameLoopState(this, services, coroutineRunner, gameWorld, gameUI, gameSetupData),
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