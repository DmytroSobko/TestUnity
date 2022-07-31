using UnityEngine;

[RequireComponent(typeof(RandomMovement))]
public abstract class Shape : PoolableObject, IUpdatable
{
    public RandomMovement RandomMovement { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        RandomMovement = GetComponent<RandomMovement>();
    }

    public void Process()
    {
        RandomMovement.Move();
    }
}