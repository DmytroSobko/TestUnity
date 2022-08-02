using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public Transform Transform { get; private set; }
    public Bounds Bounds { get; private set; }

    [SerializeField]
    private float speed;

    private Vector3 currentTargetPoint;

    private void Awake()
    {
        Transform = GetComponent<Transform>();
    }

    public void SetBounds(Bounds bounds)
    {
        Bounds = bounds;
        currentTargetPoint = Bounds.GetRandomPointInside();
    }

    public void Move()
    {
        Vector3 direction = currentTargetPoint - Transform.position;

        if (direction.sqrMagnitude < Constants.DELTA)
        {
            currentTargetPoint = Bounds.GetRandomPointInside();
        }
        else
        {
            direction.Normalize();
            Transform.position += speed * Time.deltaTime * direction;
        }
    }
}