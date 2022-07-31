using System;
using UnityEngine;

public interface ISpawner<T> : IService where T : MonoBehaviour
{
    event Action<int> NumberOfSpawnedObjectsChanged;
    event Action<T> ObjectSpawned;
    event Action<T> ObjectDespawned;

    void Spawn<T1>(int numberOfObjects) where T1 : T;
    void Despawn<T1>(int numberOfObjects) where T1 : T;
}