using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public Transform Transform { get; private set; }

    protected virtual void Awake()
    {
        Transform = GetComponent<Transform>();
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}