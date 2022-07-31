using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PoolingSystem<T> : ISpawnerResource<T> where T : PoolableObject
{
    private const string POOLING_ROOT_NAME = "PoolingRoot";

    public List<ObjectPool<T>> ObjectPools
        => objectPools;

    private readonly Transform poolingRoot;
    private readonly PoolableObjectFactory factory;

    public readonly List<ObjectPool<T>> objectPools = new List<ObjectPool<T>>();

    public PoolingSystem(PoolingDataScriptableObject poolingData)
    {
        poolingRoot = new GameObject(POOLING_ROOT_NAME).transform;
        factory = new PoolableObjectFactory();

        CreatePools(poolingData.PoolsData);
    }

    private void CreatePools(List<PoolData> poolsData)
    {
        foreach (PoolData poolData in poolsData)
        {
            string poolNodeName = poolData.PoolableObject.name + "s";
            Transform poolNode = new GameObject(poolNodeName).transform;
            poolNode.SetParent(poolingRoot);
            CreatePool(poolData, poolNode);
        }
    }

    private void CreatePool(PoolData poolData, Transform poolNode)
    {
        var objectPool = new ObjectPool<T>(poolData, factory, poolNode);
        objectPools.Add(objectPool);
    }

    public T1 GetObject<T1>() where T1 : T
    {
        return GetPool<T1>().GetObject() as T1;
    }

    public void ReturnObject<T1>(T1 returnObject) where T1 : T
    {
        GetPool<T1>().ReturnObject(returnObject);
    }

    private ObjectPool<T> GetPool<T1>() where T1 : T
    {
        return objectPools.First(pool => pool.IsTypeOf<T1>());
    }
}