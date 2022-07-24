using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class PoolableObjectSpawner
{
    private PoolingSystem poolingSystem;

    private List<Queue<PoolableObject>> queuesWithSpawnedObjects = new List<Queue<PoolableObject>>();

    public PoolableObjectSpawner(PoolingSystem poolingSystem)
    {
        this.poolingSystem = poolingSystem;
    }

    public void Spawn<T1>(Vector3 position) where T1 : PoolableObject
    {
        PoolableObject spawnObject = poolingSystem.GetObject<T1>();
        Queue<PoolableObject> objectQueue = GetQueue<T1>();
        spawnObject.gameObject.transform.position = position;
        objectQueue.Enqueue(spawnObject);

        if (!queuesWithSpawnedObjects.Contains(objectQueue))
        {
            queuesWithSpawnedObjects.Add(objectQueue);
        }
    }

    public void Despawn<T>() where T : PoolableObject
    {
        Queue<PoolableObject> objectQueue = GetQueue<T>();

        if (objectQueue.Count() == 0)
        {
            throw new ArgumentException($"No {typeof(T)} type found!");
        }
        else
        {
            PoolableObject despawnObject = objectQueue.Dequeue();
            poolingSystem.ReturnObject<T>(despawnObject);
        }
    }

    private Queue<PoolableObject> GetQueue<T>() where T : PoolableObject
    {
        Queue<PoolableObject> objectsQueue = queuesWithSpawnedObjects.FirstOrDefault(queue => queue.First() is T);

        if (objectsQueue == null)
        {
            objectsQueue = new Queue<PoolableObject>();
        }

        return objectsQueue;
    }
}