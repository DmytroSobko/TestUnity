using UnityEngine;

public struct KDBounds
{
    public Vector3 min;
    public Vector3 max;

    public Vector3 size
    {
        get
        {
            return max - min;
        }
    }

    public Bounds Bounds
    {
        get
        {
            return new Bounds(
                (min + max) / 2,
                (max - min)
            );
        }
    }

    public Vector3 ClosestPoint(Vector3 point)
    {
        // X axis
        if (point.x < min.x) point.x = min.x;
        else
        if (point.x > max.x) point.x = max.x;

        // Y axis
        if (point.y < min.y) point.y = min.y;
        else
        if (point.y > max.y) point.y = max.y;

        // Z axis
        if (point.z < min.z) point.z = min.z;
        else
        if (point.z > max.z) point.z = max.z;

        return point;
    }
}