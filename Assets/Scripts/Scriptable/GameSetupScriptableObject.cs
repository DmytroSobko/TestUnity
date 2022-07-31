using UnityEngine;

[CreateAssetMenu(fileName = "GameSetup", menuName = "ScriptableObjects/GameSetup", order = 1)]
public class GameSetupScriptableObject : ScriptableObject
{
    public int SpawnByDefault
        => spawnByDefault;

    public KeyCode SpawnKeyCode
        => spawnKeyCode;

    public KeyCode DespawnKeyCode
        => despawnKeyCode;

    [SerializeField]
    [Min(0)]
    private int spawnByDefault;

    [SerializeField]
    private KeyCode spawnKeyCode;

    [SerializeField]
    private KeyCode despawnKeyCode;
}