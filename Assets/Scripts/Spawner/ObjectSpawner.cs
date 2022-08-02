using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class ObjectSpawner<T> : ISpawner<T> where T : MonoBehaviour
{
    public event Action<int> NumberOfSpawnedObjectsChanged;
    public event Action<T> ObjectSpawned;
    public event Action<T> ObjectDespawned;

    public int ObjectsSpawned
        => queuesWithSpawnedObjects.Sum(queue => queue.Count);

    private GameWorld gameWorld;
    private ISpawnerResource<T> resource;
    private List<Queue<T>> queuesWithSpawnedObjects = new List<Queue<T>>();

    public ObjectSpawner(ISpawnerResource<T> resource, GameWorld gameWorld)
    {
        this.resource = resource;
        this.gameWorld = gameWorld;
    }

    public void Spawn<T1>(int numberOfObjects) where T1 : T
    {
        Queue<T> objectQueue = GetQueue<T1>();

        for (int i = 0; i < numberOfObjects; i++)
        {
            T1 spawnObject = resource.GetObject<T1>();
            spawnObject.transform.position = gameWorld.Bounds.GetRandomPointInside();
            spawnObject.transform.parent = gameWorld.transform;
            objectQueue.Enqueue(spawnObject);
            ObjectSpawned?.Invoke(spawnObject);
        }

        if (!queuesWithSpawnedObjects.Contains(objectQueue))
        {
            queuesWithSpawnedObjects.Add(objectQueue);
        }

        NumberOfSpawnedObjectsChanged?.Invoke(ObjectsSpawned);
    }

    public void Despawn<T1>(int numberOfObjects) where T1 : T
    {
        Queue<T> objectQueue = GetQueue<T1>();

        if (objectQueue.Count() == 0)
        {
            Debug.Log($"All objects of type {typeof(T1)} have been despawned!");
        }
        else
        {
            if (objectQueue.Count() < numberOfObjects)
            {
                numberOfObjects = objectQueue.Count();
            }

            for (int i = 0; i < numberOfObjects; i++)
            {
                T1 despawnObject = objectQueue.Dequeue() as T1;
                resource.ReturnObject(despawnObject);
                ObjectDespawned?.Invoke(despawnObject);
            }
        }

        NumberOfSpawnedObjectsChanged?.Invoke(ObjectsSpawned);
    }

    private Queue<T> GetQueue<T1>() where T1 : T
    {
        Queue<T> objectsQueue = queuesWithSpawnedObjects.FirstOrDefault(queue => queue.Count > 0 && queue.First() is T1);

        if (objectsQueue == null)
        {
            objectsQueue = new Queue<T>();
        }

        return objectsQueue;
    }
}