using System.Collections.Generic;
using UnityEngine;

public partial class KDQuery
{
    public int ClosestPoint(KDTree tree, Vector3 queryPosition)
    {
        Reset();

        List<Transform> points = tree.Points;
        List<int> permutation = tree.Permutation;

        if (points.Count == 0)
        {
            return -1;
        }

        int smallestIndex = 0;
        /// Smallest Squared Radius
        float SSR = float.PositiveInfinity;

        var rootNode = tree.RootNode;

        Vector3 rootClosestPoint = rootNode.bounds.ClosestPoint(queryPosition);

        PushToHeap(rootNode, rootClosestPoint, queryPosition);

        KDQueryNode queryNode;
        KDNode node;

        int partitionAxis;
        float partitionCoord;

        Vector3 tempClosestPoint;

        // searching
        while (minHeap.Count > 0)
        {
            queryNode = PopFromHeap();

            if (queryNode.distance > SSR)
                continue;

            node = queryNode.node;

            if (!node.Leaf)
            {
                partitionAxis = node.partitionAxis;
                partitionCoord = node.partitionCoordinate;

                tempClosestPoint = queryNode.tempClosestPoint;

                if ((tempClosestPoint[partitionAxis] - partitionCoord) < 0)
                {
                    // we already know we are on the side of negative bound/node,
                    // so we don't need to test for distance
                    // push to stack for later querying

                    PushToHeap(node.negativeChild, tempClosestPoint, queryPosition);
                    // project the tempClosestPoint to other bound
                    tempClosestPoint[partitionAxis] = partitionCoord;

                    if (node.positiveChild.Count != 0)
                    {
                        PushToHeap(node.positiveChild, tempClosestPoint, queryPosition);
                    }
                }
                else
                {
                    // we already know we are on the side of positive bound/node,
                    // so we don't need to test for distance
                    // push to stack for later querying

                    PushToHeap(node.positiveChild, tempClosestPoint, queryPosition);
                    // project the tempClosestPoint to other bound
                    tempClosestPoint[partitionAxis] = partitionCoord;

                    if (node.positiveChild.Count != 0)
                    {

                        PushToHeap(node.negativeChild, tempClosestPoint, queryPosition);
                    }
                }
            }
            else
            {
                float sqrDist;
                // LEAF
                for (int i = node.start; i < node.end; i++)
                {
                    int index = permutation[i];

                    sqrDist = Vector3.SqrMagnitude(points[index].position - queryPosition);

                    if (sqrDist <= SSR)
                    {
                        SSR = sqrDist;
                        smallestIndex = index;
                    }
                }
            }
        }

        return smallestIndex;
    }
}