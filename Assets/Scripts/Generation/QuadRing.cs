using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class QuadRing : MonoBehaviour
{

    [Range(0.01f, 2)]
    [SerializeField]
    float radiusInner;

    [Range(0.01f, 2)]
    [SerializeField]
    float thickness;

    [Range(3, 32)]
    [SerializeField]
    int angularSegmentsCount = 3;

    Mesh mesh;

    float RadiusOuter => radiusInner + thickness;
    int VertexCount => angularSegmentsCount * 2;

    private void OnDrawGizmosSelected()
    {
        DrawWireCircle(transform.position, transform.rotation, radiusInner, angularSegmentsCount);
        DrawWireCircle(transform.position, transform.rotation, RadiusOuter, angularSegmentsCount);
    }


    public static Vector2 GetUnitVectorbyAngle(float angRad)
    {
        return new Vector2(
                Mathf.Cos(angRad),
                Mathf.Sin(angRad)
            );
    }

    const float TAU = Mathf.PI * 2;
    public static void DrawWireCircle(Vector3 position, Quaternion rotation, float radius, int detail = 32)
    {
        Vector3[] points3D = new Vector3[detail];
        for (int i = 0; i < detail; i++)
        {

            float t = i / (float)detail;
            float angRad = t * TAU;

            Vector2 point2D = GetUnitVectorbyAngle(angRad) * radius;


            points3D[i] = position + rotation * point2D;
        }

        for (int i = 0; i < detail - 1; i++)
        {
            Gizmos.DrawLine(points3D[i], points3D[i + 1]);
        }
        Gizmos.DrawLine(points3D[detail - 1], points3D[0]);
    }


    private void Awake()
    {
        mesh = new Mesh();
        mesh.name = "QuadRing";

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void Update()
    {
        GenerateMesh();
    }

    private void GenerateMesh()
    {

        mesh.Clear();

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();


        for (int i = 0; i < angularSegmentsCount + 1; i++)
        {
            float t = i / (float)angularSegmentsCount;
            float angRad = t * TAU;

            Vector2 dir = GetUnitVectorbyAngle(angRad);

            vertices.Add(dir * RadiusOuter);
            vertices.Add(dir * radiusInner);

            uvs.Add(new Vector2(t, 1));
            uvs.Add(new Vector2(t, 0));
        }

        List<int> triangleIndices = new List<int>();

        for (int i = 0; i < angularSegmentsCount; i++)
        {

            int indexRoot = i * 2;
            int indexInnerRoot = indexRoot + 1;
            int indexOuterNext = indexRoot + 2;
            int indexInnerNext = indexRoot + 3;


            triangleIndices.Add(indexRoot);
            triangleIndices.Add(indexOuterNext);
            triangleIndices.Add(indexInnerNext);

            triangleIndices.Add(indexRoot);
            triangleIndices.Add(indexInnerNext);
            triangleIndices.Add(indexInnerRoot);

        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangleIndices, 0);
        mesh.RecalculateNormals();
        mesh.SetUVs(0, uvs);

    }
}
