using UnityEngine;

public class GameWorld : MonoBehaviour, ITransformable
{
    [SerializeField]
    private Vector3 worldSize;

    public Vector3 Size => worldSize;
    public Vector3 Position => transform.position;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0.25f);
        Gizmos.DrawCube(Vector3.zero, worldSize);
    }
}

public interface ITransformable
{
    Vector3 Size { get; }
    Vector3 Position { get; }
}