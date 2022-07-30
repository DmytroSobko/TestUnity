using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public Type PoolableObjectType { get; private set; }

    private Transform poolNode;
    private PoolableObjectFactory factory;
    private PoolableObject poolableObject;
    private Queue<PoolableObject> objects = new Queue<PoolableObject>();

    public ObjectPool(PoolData objectData, PoolableObjectFactory factory, Transform poolNode)
    {
        this.poolNode = poolNode;
        this.factory = factory;

        PoolableObjectType = objectData.PoolableObject.GetType();
        objects = new Queue<PoolableObject>();
        poolableObject = objectData.PoolableObject;

        for (int i = 0; i < objectData.NumberByDefault; i++)
        {
            CreateObject(objectData.PoolableObject);
        }
    }

    private void CreateObject(PoolableObject poolableObject)
    {
        PoolableObject newObject = factory.Create(poolableObject, poolNode);
        newObject.SetActive(false);
        objects.Enqueue(newObject);
    }

    public bool IsTypeOf<T>()
    {
        return PoolableObjectType == typeof(T);
    }

    public bool IsTypeOf(Type type)
    {
        return PoolableObjectType == type;
    }

    public PoolableObject GetObject()
    {
        if (objects.Count == 0)
        {
            CreateObject(poolableObject);
        }

        PoolableObject poolObject = objects.Dequeue();
        poolObject.SetActive(true);

        return poolObject;
    }

    public void ReturnObject(PoolableObject returnObject)
    {
        returnObject.Transform.parent = poolNode;
        returnObject.Transform.position = poolNode.position;
        returnObject.SetActive(false);
        objects.Enqueue(returnObject);
    }
}