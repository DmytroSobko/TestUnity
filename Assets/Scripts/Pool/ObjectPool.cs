using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : PoolableObject
{
    private Transform poolNode;
    private PoolableObjectFactory factory;
    private PoolableObject poolableObject;
    private Queue<T> objects = new Queue<T>();

    public ObjectPool(PoolData objectData, PoolableObjectFactory factory, Transform poolNode)
    {
        this.poolNode = poolNode;
        this.factory = factory;
        poolableObject = objectData.PoolableObject;

        objects = new Queue<T>();

        for (int i = 0; i < objectData.NumberByDefault; i++)
        {
            CreateObject(objectData.PoolableObject);
        }
    }

    private void CreateObject(PoolableObject poolableObject)
    {
        T newObject = factory.Create<T>(poolableObject, poolNode);
        newObject.SetActive(false);
        objects.Enqueue(newObject);
    }

    public bool IsTypeOf<T1>()
    {
        return poolableObject.GetType() == typeof(T1);
    }

    public T GetObject()
    {
        if (objects.Count == 0)
        {
            CreateObject(poolableObject);
        }

        T poolObject = objects.Dequeue();
        poolObject.SetActive(true);

        return poolObject;
    }

    public void ReturnObject(T returnObject)
    {
        returnObject.transform.parent = poolNode;
        returnObject.transform.position = poolNode.position;
        returnObject.SetActive(false);
        objects.Enqueue(returnObject);
    }
}