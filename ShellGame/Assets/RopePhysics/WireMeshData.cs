using System;
using System.Drawing;
using UnityEngine;

public class WireMeshData
{
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private Vector3[] bakedNormals;

    private int triangleIndex = 0;
    private bool useFlagShading;
    public WireMeshData(int numVertsPerCircle, int numVertsAlongRope, bool useFlagShading)
    {
        this.useFlagShading = useFlagShading;

        vertices = new Vector3[numVertsPerCircle * numVertsAlongRope];
        uvs = new Vector2[vertices.Length];

        int numMainTriangles = (numVertsPerCircle * (numVertsAlongRope - 1)) * 2;
        triangles = new int[numMainTriangles * 3];
        triangleIndex = 0;
    }
    public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex)
    {
        vertices[vertexIndex] = vertexPosition;
        uvs[vertexIndex] = uv;
    }
    public void AddTriangles(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }
    private Vector3[] CalculateNormals()
    {
        Vector3[] vertexNormals = new Vector3[vertices.Length];
        int triangleCount = triangles.Length / 3;
        for (int i = 0; i < triangleCount; i++)
        {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = triangles[normalTriangleIndex];
            int vertexIndexB = triangles[normalTriangleIndex + 1];
            int vertexIndexC = triangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            vertexNormals[vertexIndexA] += triangleNormal;
            vertexNormals[vertexIndexB] += triangleNormal;
            vertexNormals[vertexIndexC] += triangleNormal;
        }
        for (int i = 0; i < vertexNormals.Length; i++)
        {
            vertexNormals[i].Normalize();
        }

        return vertexNormals;
    }

    private Vector3 SurfaceNormalFromIndices(int vertexIndexA, int vertexIndexB, int vertexIndexC)
    {
        Vector3 pointA = vertices[vertexIndexA];
        Vector3 pointB = vertices[vertexIndexB];
        Vector3 pointC = vertices[vertexIndexC];

        Vector3 sideAB = pointB - pointA;
        Vector3 sideAC = pointC - pointA;

        return Vector3.Cross(sideAB, sideAC).normalized;
    }

    public void ProcessMesh()
    {
        if (useFlagShading)
            FlatShading();
        else
            BakeNormals();

    }

    private void BakeNormals()
    {
        bakedNormals = CalculateNormals();
    }

    private void FlatShading()
    {
        Vector3[] flatShadedVertices = new Vector3[triangles.Length];
        Vector2[] flatShadedUvs = new Vector2[triangles.Length];

        for (int i = 0; i < triangles.Length; i++)
        {
            flatShadedVertices[i] = vertices[triangles[i]];
            flatShadedUvs[i] = uvs[triangles[i]];
            triangles[i] = i;
        }

        vertices = flatShadedVertices;
        uvs = flatShadedUvs;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        if (useFlagShading)
        {
            mesh.RecalculateNormals();
        }
        else
        {
            mesh.normals = bakedNormals;
        }
        return mesh;
    }

    internal void ResetMesh(int numVertsPerCircle, int numVertsAlongRope, bool useFlatShading)
    {
        this.useFlagShading = useFlatShading;
        vertices = new Vector3[numVertsPerCircle * numVertsAlongRope];
        uvs = new Vector2[vertices.Length];

        int numMainTriangles = (numVertsPerCircle * (numVertsAlongRope - 1)) * 2;
        triangles = new int[numMainTriangles * 3];
        triangleIndex = 0;
    }
}
