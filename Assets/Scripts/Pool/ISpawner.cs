using System;

public interface ISpawner<TPayload> : IService
{
    event Action<int> NumberOfSpawnedObjectsChanged;

    void Spawn(Type poolableObjectType, uint numberOfObjects);
    void Despawn(Type poolableObjectType, uint numberOfObjects);
    void Spawn<T>(uint numberOfObjects) where T : TPayload;
    void Despawn<T>(uint numberOfObjects) where T : TPayload;
}