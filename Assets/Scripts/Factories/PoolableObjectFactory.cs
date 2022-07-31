using UnityEngine;

public class PoolableObjectFactory
{
    public T Create<T>(PoolableObject sample, Transform parent) where T : PoolableObject
    {
        return Object.Instantiate(sample, parent).GetComponent<T>();
    }
}