using UnityEngine;

[RequireComponent(typeof(RandomMovement))]
public class Sphere : PoolableObject
{
    public RandomMovement RandomMovement { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        RandomMovement = GetComponent<RandomMovement>();
    }
}