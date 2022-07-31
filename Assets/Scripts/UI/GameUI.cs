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
    private TextMeshProUGUI spawnedObjectsText;

    [SerializeField]
    private PoolingSystemDropdown poolingSystemDropdown;

    [SerializeField]
    private TMP_InputField numberOfObjectsToSpawnOrDespawnInputField;

    public void UpdateSpawnedObjectsText(int value)
    {
        spawnedObjectsText.text = value.ToString();
    }
}