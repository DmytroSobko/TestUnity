using UnityEngine;

public class GameWorld : MonoBehaviour
{
    public Transform Transform { get; private set; }
    public Bounds Bounds { get; private set; }

    [SerializeField]
    private Vector3 size;

    private void Awake()
    {
        Transform = GetComponent<Transform>();
        Bounds = new Bounds(transform.position, size);
    }

    private void OnValidate()
    {
        Bounds = new Bounds(transform.position, size);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0.25f);
        Gizmos.DrawCube(transform.position, size);
    }
}