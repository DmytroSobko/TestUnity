using UnityEngine;

public class PoolableObjectFactory : IFactory<PoolableObject>
{
    public PoolableObject Create(PoolableObject sample)
    {
        return Object.Instantiate(sample).GetComponent<PoolableObject>();
    }

    public PoolableObject Create(PoolableObject sample, Transform parent)
    {
        return Object.Instantiate(sample, parent).GetComponent<PoolableObject>();
    }
}