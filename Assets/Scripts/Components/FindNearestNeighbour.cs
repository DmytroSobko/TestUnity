using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FindNearestNeighbour : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private KDTree tree;
    private KDQuery query = new KDQuery();
    private List<Transform> neighbours = new List<Transform>();

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
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

        if (neighbours.Count == 0)
        {
            DrawLineToNearest(transform.position);
        }
    }

    public void FindNearest()
    {
        if (neighbours.Count > 0)
        {
            tree.Rebuild();

            int closestIndex = query.ClosestPoint(tree, transform.position);
            Vector3 nearest = neighbours[closestIndex].position;
            DrawLineToNearest(nearest);
        }
    }

    private void DrawLineToNearest(Vector3 nearest)
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, nearest);
    }
}