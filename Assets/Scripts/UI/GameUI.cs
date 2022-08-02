using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public PoolingSystemDropdown PoolingSystemDropdown
     => poolingSystemDropdown;

    public string NumberOfObjectsToSpawnOrDespawn
        => numberOfObjectsToSpawnOrDespawnInputField.text;

    public bool IsInputFieldFocused
        => numberOfObjectsToSpawnOrDespawnInputField.isFocused;

    [SerializeField]
    private TextMeshProUGUI spawnKeyCodeText;

    [SerializeField]
    private TextMeshProUGUI despawnKeyCodeText;

    [SerializeField]
    private TextMeshProUGUI spawnedObjectsNumberText;

    [SerializeField]
    private PoolingSystemDropdown poolingSystemDropdown;

    [SerializeField]
    private TMP_InputField numberOfObjectsToSpawnOrDespawnInputField;

    public void Init(GameSetupScriptableObject gameSetup, PoolingDataScriptableObject poolingData)
    {
        poolingSystemDropdown.Init(poolingData.PoolsData);
        spawnKeyCodeText.text = gameSetup.SpawnKeyCode.ToString();
        despawnKeyCodeText.text = gameSetup.DespawnKeyCode.ToString();
    }

    public void UpdateSpawnedObjectsNumberText(int value)
    {
        spawnedObjectsNumberText.text = value.ToString();
    }
}