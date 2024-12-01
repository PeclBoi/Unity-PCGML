using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

	[Range(0f, 1f)]
	[SerializeField] float t = 0;

	public Transform[] controlPoints = new Transform[4];

	public Mesh2D shape2D;

	[Range(2, 32)]
	[SerializeField] int edgeRingCount = 8;

	public GameObject Marker;

	Vector3 GetPos(int i) => controlPoints[i].position;
	Mesh mesh;

	List<BoxCollider> colliders = new List<BoxCollider>();

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		for (int i = 0; i < controlPoints.Length; i++)
		{
			Gizmos.DrawSphere(GetPos(i), 01f);
		}
		Gizmos.color = Color.white;

		Handles.DrawBezier(GetPos(0), GetPos(3), GetPos(1), GetPos(2), Color.green, EditorGUIUtility.whiteTexture, 4);

		var op = GetBezierOP(t);

		var vertInLocalSpace = shape2D.vertices.Select(v => op.LocalToWorldPosition(v.point)).ToArray();

		//for (int i = 0; i < shape2D.lineIndices.Length - 1; i += 2)
		//{
		//	Gizmos.DrawLine(vertInLocalSpace[shape2D.lineIndices[i]], vertInLocalSpace[shape2D.lineIndices[i + 1]]);
		//}
	}

	private void Awake()
	{
		mesh = new Mesh();
		mesh.name = "Segment";

		//gameObject.layer = LayerMask.NameToLayer("Vision");

		GetComponent<MeshFilter>().sharedMesh = mesh;
		//GenerateMesh(Quaternion.identity);
	}

	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.F1))
		{
			GenerateMesh(Quaternion.identity);
			var meshCollider = gameObject.GetComponent<MeshCollider>();
			meshCollider.sharedMesh = mesh;
		}
	}

	public void GenerateMesh(Quaternion rotation)
	{
		Debug.Log("GenMesh");
		mesh.Clear();
		transform.rotation = rotation;


		//Verts
		List<Vector3> verts = new List<Vector3>();
		for (int ring = 0; ring < edgeRingCount; ring++)
		{

			float t = ring / (edgeRingCount - 1f);

			OrientedPoint op = GetBezierOP(t);

			for (int i = 0; i < shape2D.vertices.Length; i++)
			{
				verts.Add(op.LocalToWorldPosition(shape2D.vertices[i].point));
			}
		}

		////Markers
		//for (int ring = 0; ring < edgeRingCount; ring++)
		//{
		//	float t = ring / (edgeRingCount - 1f);

		//	OrientedPoint op = GetBezierOP(t);

		//	Instantiate(Marker, op.pos + transform.position, op.rotation);
		//}


		//Triangle
		List<int> triangleIndicies = new List<int>();
		for (int ring = 0; ring < edgeRingCount - 1; ring++)
		{
			int rootIndex = ring * shape2D.vertices.Length;
			int rootIndexNext = (ring + 1) * shape2D.vertices.Length;

			for (int line = 0; line < shape2D.lineIndices.Length - 1; line += 2)
			{
				int lineIndexA = shape2D.lineIndices[line];
				int lineIndexB = shape2D.lineIndices[line + 1];

				int currentA = rootIndex + lineIndexA;
				int currentB = rootIndex + lineIndexB;
				int nextA = rootIndexNext + lineIndexA;
				int nextB = rootIndexNext + lineIndexB;


				triangleIndicies.Add(currentA);
				triangleIndicies.Add(nextA);
				triangleIndicies.Add(nextB);

				triangleIndicies.Add(currentA);
				triangleIndicies.Add(nextB);
				triangleIndicies.Add(currentB);
			}
		}


		mesh.SetVertices(verts);
		mesh.SetTriangles(triangleIndicies, 0);
		mesh.RecalculateNormals();

        var meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }




	OrientedPoint GetBezierOP(float t)
	{
		Vector3 p0 = GetPos(0);
		Vector3 p1 = GetPos(1);
		Vector3 p2 = GetPos(2);
		Vector3 p3 = GetPos(3);

		Vector3 a = Vector3.Lerp(p0, p1, t);
		Vector3 b = Vector3.Lerp(p1, p2, t);
		Vector3 c = Vector3.Lerp(p2, p3, t);

		Vector3 d = Vector3.Lerp(a, b, t);
		Vector3 e = Vector3.Lerp(b, c, t);

		Vector3 point = Vector3.Lerp(d, e, t);

		return new OrientedPoint(point - transform.position, (e - d).normalized);
	}

}
public struct OrientedPoint
{
	private Vector3 pos;
	private Quaternion rotation;

	public OrientedPoint(Vector3 pos, Quaternion rotation)
	{
		this.pos = pos;
		this.rotation = rotation;
	}

	public OrientedPoint(Vector3 pos, Vector3 forward)
	{
		this.pos = pos;
		this.rotation = Quaternion.LookRotation(forward);
	}

	public Vector3 LocalToWorldPosition(Vector3 localSpacePosition)
	{
		return pos + rotation * localSpacePosition;
	}

	public Vector3 LocalToWorldVector(Vector3 localSpacePosition)
	{
		return rotation * localSpacePosition;
	}

}

