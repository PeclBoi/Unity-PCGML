using System;
using UnityEngine;

[CreateAssetMenu]
public class Mesh2D : ScriptableObject
{
	[Serializable]
	public class Vertex
	{
		public Vector2 point;
		public Vector2 normal;
		public float uvs;
	}

	public Vertex[] vertices;
	public int[] lineIndices;

}
