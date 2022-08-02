using UnityEngine;

public abstract class PoolableObject : MonoBehaviour
{
    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}