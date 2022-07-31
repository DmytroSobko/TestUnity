using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct PoolData
{
    public PoolableObject PoolableObject
        => poolableObject;

    public int NumberByDefault
        => numberByDefault;

    [SerializeField]
    [Min(0)]
    private PoolableObject poolableObject;

    [SerializeField]
    [Min(0)]
    private int numberByDefault;
}

[CreateAssetMenu(fileName = "PoolingData", menuName = "ScriptableObjects/PoolingData", order = 2)]
public class PoolingDataScriptableObject : ScriptableObject
{
    public List<PoolData> PoolsData;
}
