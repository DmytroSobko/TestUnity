#define KDTREE_DUPLICATES

using System.Collections.Generic;
using System;
using UnityEngine;

public class KDTree
{
    public KDNode RootNode { get; private set; }

    public List<Transform> Points { get { return points; } } // points on which kd-tree will build on. This array will stay unchanged when re/building kdtree!
    private List<Transform> points;

    public List<int> Permutation { get { return permutation; } } // index aray, that will be permuted
    private List<int> permutation = new List<int>();

    private int maxPointsPerLeafNode = 32;

    private KDNode[] kdNodesStack;
    private int kdNodesCount = 0;

    public KDTree(List<Transform> points, int maxPointsPerLeafNode = 32)
    {
        this.points = points;
        kdNodesStack = new KDNode[64];

        this.maxPointsPerLeafNode = maxPointsPerLeafNode;

        Rebuild();
    }

    public void Build(List<Transform> newPoints, int maxPointsPerLeafNode = -1)
    {
        points = newPoints;
        permutation.Clear();

        Rebuild(maxPointsPerLeafNode);
    }

    public void Rebuild(int maxPointsPerLeafNode = -1)
    {
        for (int i = 0; i < points.Count; i++)
        {
            permutation.Add(i);
        }

        if (maxPointsPerLeafNode > 0)
        {
            this.maxPointsPerLeafNode = maxPointsPerLeafNode;
        }

        BuildTree();
    }

    private void BuildTree()
    {
        ResetKDNodeStack();

        RootNode = GetKDNode();
        RootNode.bounds = MakeBounds();
        RootNode.start = 0;
        RootNode.end = points.Count;

        SplitNode(RootNode);
    }

    private KDNode GetKDNode()
    {
        KDNode node;
        if (kdNodesCount < kdNodesStack.Length)
        {
            if (kdNodesStack[kdNodesCount] == null)
            {
                kdNodesStack[kdNodesCount] = node = new KDNode();
            }
            else
            {
                node = kdNodesStack[kdNodesCount];
                node.partitionAxis = -1;
            }
        }
        else
        {
            // automatic resize of KDNode pool array
            Array.Resize(ref kdNodesStack, kdNodesStack.Length * 2);
            node = kdNodesStack[kdNodesCount] = new KDNode();
        }

        kdNodesCount++;

        return node;
    }

    private void ResetKDNodeStack()
    {
        kdNodesCount = 0;
    }

    /// <summary>
    /// For calculating root node bounds
    /// </summary>
    /// <returns>Boundary of all Vector3 points</returns>
    private KDBounds MakeBounds()
    {
        Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        int even = points.Count & ~1; // calculate even Length

        // min, max calculations
        // 3n/2 calculations instead of 2n
        for (int i0 = 0; i0 < even; i0 += 2)
        {

            int i1 = i0 + 1;

            // X Coords
            if (points[i0].position.x > points[i1].position.x)
            {
                // i0 is bigger, i1 is smaller
                if (points[i1].position.x < min.x)
                    min.x = points[i1].position.x;

                if (points[i0].position.x > max.x)
                    max.x = points[i0].position.x;
            }
            else
            {
                // i1 is smaller, i0 is bigger
                if (points[i0].position.x < min.x)
                    min.x = points[i0].position.x;

                if (points[i1].position.x > max.x)
                    max.x = points[i1].position.x;
            }

            // Y Coords
            if (points[i0].position.y > points[i1].position.y)
            {
                // i0 is bigger, i1 is smaller
                if (points[i1].position.y < min.y)
                    min.y = points[i1].position.y;

                if (points[i0].position.y > max.y)
                    max.y = points[i0].position.y;
            }
            else
            {
                // i1 is smaller, i0 is bigger
                if (points[i0].position.y < min.y)
                    min.y = points[i0].position.y;

                if (points[i1].position.y > max.y)
                    max.y = points[i1].position.y;
            }

            // Z Coords
            if (points[i0].position.z > points[i1].position.z)
            {
                // i0 is bigger, i1 is smaller
                if (points[i1].position.z < min.z)
                    min.z = points[i1].position.z;

                if (points[i0].position.z > max.z)
                    max.z = points[i0].position.z;
            }
            else
            {
                // i1 is smaller, i0 is bigger
                if (points[i0].position.z < min.z)
                    min.z = points[i0].position.z;

                if (points[i1].position.z > max.z)
                    max.z = points[i1].position.z;
            }
        }

        // if array was odd, calculate also min/max for the last element
        if (even != points.Count)
        {
            // X
            if (min.x > points[even].position.x)
                min.x = points[even].position.x;

            if (max.x < points[even].position.x)
                max.x = points[even].position.x;
            // Y
            if (min.y > points[even].position.y)
                min.y = points[even].position.y;

            if (max.y < points[even].position.y)
                max.y = points[even].position.y;
            // Z
            if (min.z > points[even].position.z)
                min.z = points[even].position.z;

            if (max.z < points[even].position.z)
                max.z = points[even].position.z;
        }

        KDBounds b = new KDBounds();
        b.min = min;
        b.max = max;

        return b;
    }

    /// <summary>
    /// Recursive splitting procedure
    /// </summary>
    /// <param name="parent">This is where root node goes</param>
    /// <param name="depth"></param>
    ///
    private void SplitNode(KDNode parent)
    {
        // center of bounding box
        KDBounds parentBounds = parent.bounds;
        Vector3 parentBoundsSize = parentBounds.size;

        // Find axis where bounds are largest
        int splitAxis = 0;
        float axisSize = parentBoundsSize.x;

        if (axisSize < parentBoundsSize.y)
        {
            splitAxis = 1;
            axisSize = parentBoundsSize.y;
        }

        if (axisSize < parentBoundsSize.z)
        {
            splitAxis = 2;
        }

        // Our axis min-max bounds
        float boundsStart = parentBounds.min[splitAxis];
        float boundsEnd = parentBounds.max[splitAxis];

        // Calculate the spliting coords
        float splitPivot = CalculatePivot(parent.start, parent.end, boundsStart, boundsEnd, splitAxis);

        parent.partitionAxis = splitAxis;
        parent.partitionCoordinate = splitPivot;

        // 'Spliting' array to two subarrays
        int splittingIndex = Partition(parent.start, parent.end, splitPivot, splitAxis);

        // Negative / Left node
        Vector3 negMax = parentBounds.max;
        negMax[splitAxis] = splitPivot;

        KDNode negNode = GetKDNode();
        negNode.bounds = parentBounds;
        negNode.bounds.max = negMax;
        negNode.start = parent.start;
        negNode.end = splittingIndex;
        parent.negativeChild = negNode;

        // Positive / Right node
        Vector3 posMin = parentBounds.min;
        posMin[splitAxis] = splitPivot;

        KDNode posNode = GetKDNode();
        posNode.bounds = parentBounds;
        posNode.bounds.min = posMin;
        posNode.start = splittingIndex;
        posNode.end = parent.end;
        parent.positiveChild = posNode;

        // check if we are actually splitting it anything
        // this if check enables duplicate coordinates, but makes construction a bit slower
#if KDTREE_DUPLICATES
        if (negNode.Count != 0 && posNode.Count != 0)
        {
#endif
            // Constraint function deciding if split should be continued
            if (ContinueSplit(negNode))
                SplitNode(negNode);


            if (ContinueSplit(posNode))
                SplitNode(posNode);

#if KDTREE_DUPLICATES
        }
#endif
    }

    /// <summary>
    /// Sliding midpoint splitting pivot calculation
    /// 1. First splits node to two equal parts (midPoint)
    /// 2. Checks if elements are in both sides of splitted bounds
    /// 3a. If they are, just return midPoint
    /// 3b. If they are not, then points are only on left or right bound.
    /// 4. Move the splitting pivot so that it shrinks part with points completely (calculate min or max dependent) and return.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="boundsStart"></param>
    /// <param name="boundsEnd"></param>
    /// <param name="axis"></param>
    /// <returns></returns>
    private float CalculatePivot(int start, int end, float boundsStart, float boundsEnd, int axis)
    {
        //! sliding midpoint rule
        float midPoint = (boundsStart + boundsEnd) / 2f;

        bool negative = false;
        bool positive = false;

        float negMax = float.MinValue;
        float posMin = float.MaxValue;

        // this for loop section is used both for sorted and unsorted data
        for (int i = start; i < end; i++)
        {
            if (points[permutation[i]].position[axis] < midPoint)
                negative = true;
            else
                positive = true;

            if (negative == true && positive == true)
                return midPoint;
        }

        if (negative)
        {
            for (int i = start; i < end; i++)
                if (negMax < points[permutation[i]].position[axis])
                    negMax = points[permutation[i]].position[axis];

            return negMax;
        }
        else
        {
            for (int i = start; i < end; i++)
                if (posMin > points[permutation[i]].position[axis])
                    posMin = points[permutation[i]].position[axis];

            return posMin;
        }
    }

    /// <summary>
    /// Similar to Hoare partitioning algorithm (used in Quick Sort)
    /// Modification: pivot is not left-most element but is instead argument of function
    /// Calculates splitting index and partially sorts elements (swaps them until they are on correct side - depending on pivot)
    /// Complexity: O(n)
    /// </summary>
    /// <param name="start">Start index</param>
    /// <param name="end">End index</param>
    /// <param name="partitionPivot">Pivot that decides boundary between left and right</param>
    /// <param name="axis">Axis of this pivoting</param>
    /// <returns>
    /// Returns splitting index that subdivides array into 2 smaller arrays
    /// left = [start, pivot),
    /// right = [pivot, end)
    /// </returns>
    private int Partition(int start, int end, float partitionPivot, int axis)
    {
        // note: increasing right pointer is actually decreasing!
        int LP = start - 1; // left pointer (negative side)
        int RP = end;       // right pointer (positive side)

        int temp;           // temporary var for swapping permutation indexes

        while (true)
        {

            do
            {
                // move from left to the right until "out of bounds" value is found
                LP++;
            }
            while (LP < RP && points[permutation[LP]].position[axis] < partitionPivot);

            do
            {
                // move from right to the left until "out of bounds" value found
                RP--;
            }
            while (LP < RP && points[permutation[RP]].position[axis] >= partitionPivot);

            if (LP < RP)
            {
                // swap
                temp = permutation[LP];
                permutation[LP] = permutation[RP];
                permutation[RP] = temp;
            }
            else
            {

                return LP;
            }
        }
    }

    /// <summary>
    /// Constraint function. You can add custom constraints here - if you have some other data/classes binded to Vector3 points
    /// Can hardcode it into
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private bool ContinueSplit(KDNode node)
    {
        return (node.Count > maxPointsPerLeafNode);
    }
}