using UnityEngine;

public class Game
{
    public GameStateMachine StateMachine;

    public Game(ICoroutineRunner coroutineRunner, GameWorld gameWorld, ScreenUI screenUI, PoolingDataScriptableObject poolingData, Transform poolingRoot, uint spawnByDefault, KeyCode spawnKeyCode, KeyCode despawnKeyCode)
    {
        StateMachine = new GameStateMachine(AllServices.Container, coroutineRunner, gameWorld, screenUI, poolingData, poolingRoot, spawnByDefault, spawnKeyCode, despawnKeyCode);
    }
}