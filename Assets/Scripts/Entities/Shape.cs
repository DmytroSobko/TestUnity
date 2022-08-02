using UnityEngine;

[RequireComponent(typeof(RandomMovement))]
[RequireComponent(typeof(FindNearestNeighbour))]
public abstract class Shape : PoolableObject, IUpdatable
{
    public RandomMovement RandomMovement { get; private set; }
    public FindNearestNeighbour FindNearestNeighbour { get; private set; }

    private void Awake()
    {
        RandomMovement = GetComponent<RandomMovement>();
        FindNearestNeighbour = GetComponent<FindNearestNeighbour>();
    }

    public void Process()
    {
        RandomMovement.Move();
        FindNearestNeighbour.FindNearest();
    }
}