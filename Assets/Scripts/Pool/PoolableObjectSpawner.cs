using System.Collections.Generic;
using System.Linq;
using System;

public class PoolableObjectSpawner : ISpawner<PoolableObject>
{
    public event Action<int> NumberOfSpawnedObjectsChanged;

    public int ObjectsSpawned
        => queuesWithSpawnedObjects.Sum(queue => queue.Count);

    private PoolingSystem poolingSystem;
    private GameWorld gameWorld;
    private List<Queue<PoolableObject>> queuesWithSpawnedObjects = new List<Queue<PoolableObject>>();

    public PoolableObjectSpawner(PoolingSystem poolingSystem, GameWorld gameWorld)
    {
        this.poolingSystem = poolingSystem;
        this.gameWorld = gameWorld;
    }

    public void Spawn(Type poolableObjectType, uint numberOfObjects)
    {
        Queue<PoolableObject> objectQueue = GetQueue(poolableObjectType);

        for (int i = 0; i < numberOfObjects; i++)
        {
            PoolableObject spawnObject = poolingSystem.GetObject(poolableObjectType);
            spawnObject.Transform.position = gameWorld.Bounds.GetRandomPointInside();
            spawnObject.Transform.parent = gameWorld.Transform;
            objectQueue.Enqueue(spawnObject);
        }

        if (!queuesWithSpawnedObjects.Contains(objectQueue))
        {
            queuesWithSpawnedObjects.Add(objectQueue);
        }

        NumberOfSpawnedObjectsChanged?.Invoke(ObjectsSpawned);
    }

    public void Despawn(Type poolableObjectType, uint numberOfObjects)
    {
        Queue<PoolableObject> objectQueue = GetQueue(poolableObjectType);

        for (int i = 0; i < numberOfObjects; i++)
        {
            if (objectQueue.Count() == 0)
            {
                throw new ArgumentException($"No {poolableObjectType} type found!");
            }
            else
            {
                PoolableObject despawnObject = objectQueue.Dequeue();
                poolingSystem.ReturnObject(despawnObject);
            }
        }

        NumberOfSpawnedObjectsChanged?.Invoke(ObjectsSpawned);
    }

    public void Spawn<T>(uint numberOfObjects) where T : PoolableObject
    {
        Queue<PoolableObject> objectQueue = GetQueue<T>();

        for (int i = 0; i < numberOfObjects; i++)
        {
            PoolableObject spawnObject = poolingSystem.GetObject<T>();
            spawnObject.Transform.position = gameWorld.Bounds.GetRandomPointInside();
            spawnObject.Transform.parent = gameWorld.Transform;
            objectQueue.Enqueue(spawnObject);
        }

        if (!queuesWithSpawnedObjects.Contains(objectQueue))
        {
            queuesWithSpawnedObjects.Add(objectQueue);
        }

        NumberOfSpawnedObjectsChanged?.Invoke(ObjectsSpawned);
    }

    public void Despawn<T>(uint numberOfObjects) where T : PoolableObject
    {
        Queue<PoolableObject> objectQueue = GetQueue<T>();

        for (int i = 0; i < numberOfObjects; i++)
        {
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

        NumberOfSpawnedObjectsChanged?.Invoke(ObjectsSpawned);
    }

    private Queue<PoolableObject> GetQueue(Type poolableObjectType)
    {
        Queue<PoolableObject> objectsQueue = queuesWithSpawnedObjects.FirstOrDefault(queue => queue.Count > 0 && queue.First().GetType() == poolableObjectType);

        if (objectsQueue == null)
        {
            objectsQueue = new Queue<PoolableObject>();
        }

        return objectsQueue;
    }

    private Queue<PoolableObject> GetQueue<T>() where T : PoolableObject
    {
        Queue<PoolableObject> objectsQueue = queuesWithSpawnedObjects.FirstOrDefault(queue => queue.Count > 0 && queue.First() is T);

        if (objectsQueue == null)
        {
            objectsQueue = new Queue<PoolableObject>();
        }

        return objectsQueue;
    }
}