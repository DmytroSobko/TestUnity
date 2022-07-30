using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PoolingSystem : IService
{
    public List<ObjectPool> ObjectPools
        => objectPools;

    private Transform poolingRoot;
    private PoolableObjectFactory factory;

    private List<ObjectPool> objectPools = new List<ObjectPool>();

    public PoolingSystem(PoolingDataScriptableObject poolingData, Transform poolingRoot)
    {
        this.poolingRoot = poolingRoot;
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
        var objectPool = new ObjectPool(poolData, factory, poolNode);
        objectPools.Add(objectPool);
    }

    public PoolableObject GetObject(Type poolableObjectType)
    {
        return GetPool(poolableObjectType).GetObject();
    }

    public PoolableObject GetObject<T>() where T : PoolableObject
    {
        return GetPool<T>().GetObject();
    }

    public void ReturnObject(PoolableObject returnObject)
    {
        GetPool(returnObject.GetType()).ReturnObject(returnObject);
    }

    public void ReturnObject<T>(PoolableObject returnObject) where T : PoolableObject
    {
        GetPool<T>().ReturnObject(returnObject);
    }

    private ObjectPool GetPool<T>()
    {
        return objectPools.First(pool => pool.IsTypeOf<T>());
    }

    private ObjectPool GetPool(Type poolableObjectType)
    {
        return objectPools.First(pool => pool.IsTypeOf(poolableObjectType));
    }
}