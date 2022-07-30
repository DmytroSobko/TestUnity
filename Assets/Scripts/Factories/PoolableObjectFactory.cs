using UnityEngine;

public class PoolableObjectFactory
{
    public PoolableObject Create(PoolableObject sample, Transform parent)
    {
        return Object.Instantiate(sample, parent).GetComponent<PoolableObject>();
    }
}