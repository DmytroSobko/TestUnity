using UnityEngine;

public class Bounds
{
	private Vector3 center;
	private Vector3 extents;

	public Vector3 Min
		=> center - extents;

	public Vector3 Max
		=> center + extents;

	public Bounds(Vector3 center, Vector3 size)
	{
		this.center = center;
		extents = size * 0.5f;
	}

	public Vector3 GetRandomPointInside()
	{
		return new Vector3(
			Random.Range(Min.x, Max.x),
			Random.Range(Min.y, Max.y),
			Random.Range(Min.z, Max.z)
		);
	}
}