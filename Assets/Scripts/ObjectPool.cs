using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool
{
    private PoolableObject poolableObject;
    private PoolableObjectFactory factory;
    private Transform poolNode;
    private Queue<PoolableObject> objects = new Queue<PoolableObject>();

    public ObjectPool(PoolData objectData, PoolableObjectFactory factory, Transform poolNode)
    {
        this.factory = factory;
        this.poolNode = poolNode;
        poolableObject = objectData.PoolableObject;

        for (int i = 0; i < objectData.NumberByDefault; i++)
        {
            CreateObject(objectData.PoolableObject);
        }
    }

    private void CreateObject(PoolableObject poolableObject)
    {
        PoolableObject newObject = factory.Create(poolableObject, poolNode);
        objects.Enqueue(newObject);
    }

    public bool IsTypeOf<T1>()
    {
        return objects.First() is T1;
    }

    public PoolableObject GetObject()
    {
        if (objects.Count == 0)
        {
            CreateObject(poolableObject);
        }
 
        return objects.Dequeue();
    }

    public void ReturnObject(PoolableObject returnObject)
    {
        objects.Enqueue(returnObject);
    }
}