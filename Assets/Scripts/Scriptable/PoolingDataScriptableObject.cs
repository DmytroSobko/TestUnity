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
    private PoolableObject poolableObject;

    [SerializeField]
    private int numberByDefault;
}

[CreateAssetMenu(fileName = "PoolingData", menuName = "ScriptableObjects/PoolingData", order = 1)]
public class PoolingDataScriptableObject : ScriptableObject
{
    public List<PoolData> PoolsData;
}
