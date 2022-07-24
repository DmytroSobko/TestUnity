using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolingSystem
{
    private PoolableObjectFactory factory = new PoolableObjectFactory();
    private List<ObjectPool> objectPools = new List<ObjectPool>();

    public PoolingSystem(PoolingDataScriptableObject poolingData, Transform poolingRoot)
    {
        foreach (PoolData poolData in poolingData.PoolsData)
        {
            string poolNodeName = poolData.PoolableObject.name + "s";
            Transform poolNode = new GameObject(poolNodeName).transform;
            poolNode.SetParent(poolingRoot);

            var objectPool = new ObjectPool(poolData, factory, poolNode);
            objectPools.Add(objectPool);
        }
    }

    public PoolableObject GetObject<T1>() where T1 : PoolableObject
    {
        return GetPool<T1>().GetObject();
    }

    public void ReturnObject<T1>(PoolableObject returnObject) where T1 : PoolableObject
    {
        GetPool<T1>().ReturnObject(returnObject);
    }

    private ObjectPool GetPool<T1>() where T1 : PoolableObject
    {
        return objectPools.First(pool => pool.IsTypeOf<T1>());
    }
}