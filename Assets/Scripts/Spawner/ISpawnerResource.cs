using UnityEngine;

public interface ISpawnerResource<T> : IService where T : MonoBehaviour
{
    T1 GetObject<T1>() where T1 : T;
    void ReturnObject<T1>(T1 returnObject) where T1 : T;
}