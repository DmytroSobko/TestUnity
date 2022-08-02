using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FindNearestNeighbour : MonoBehaviour
{
    public LineRenderer LineRenderer { get; private set; }

    private KDTree tree;
    private KDQuery query = new KDQuery();
    private List<Transform> neighbours = new List<Transform>();

    private void Awake()
    {
        LineRenderer = GetComponent<LineRenderer>();
    }

    public void SetNeighbours(List<Transform> neighbours)
    {
        this.neighbours = neighbours;
        tree = new KDTree(neighbours);
    }

    public void AddNeighbour(Transform neighbour)
    {
        neighbours.Add(neighbour);
        tree.Build(neighbours);
    }

    public void RemoveNeighbour(Transform neighbour)
    {
        neighbours.Remove(neighbour);
        tree.Build(neighbours);
    }

    public void FindNearest()
    {
        tree.Rebuild();

        int closestIndex = query.ClosestPoint(tree, transform.position);
        if (closestIndex != -1)
        {
            Vector3 nearest = neighbours[closestIndex].position;
            LineRenderer.SetPosition(0, transform.position);
            LineRenderer.SetPosition(1, nearest);
        }
    }
}