using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProceduralMesh
{
    public Mesh Mesh { get; set; }
    public Dictionary<string, float> Parameters { get; set; }
    public bool Flipped { get; set; }

    public ProceduralMesh()
    {
        Parameters = new Dictionary<string, float>();
        Flipped = false;
    }

    private int SubdivideGetNewVertex(int i1, int i2, Dictionary<uint, int> newVertices, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs)
    {
        // We have to test both directions since the edge
        // could be reversed in another triangle
        uint t1 = ((uint)i1 << 16) | (uint)i2;
        uint t2 = ((uint)i2 << 16) | (uint)i1;
        if (newVertices.ContainsKey(t2))
            return newVertices[t2];
        if (newVertices.ContainsKey(t1))
            return newVertices[t1];
        // generate vertex:
        int newIndex = vertices.Count;
        newVertices.Add(t1, newIndex);

        // calculate new vertex
        vertices.Add((vertices[i1] + vertices[i2]) * 0.5f);
        normals.Add((normals[i1] + normals[i2]).normalized);
        uvs.Add((uvs[i1] + uvs[i2]) * 0.5f);

        return newIndex;
    }

    public void Subdivide()
    {
        Dictionary<uint, int> newVertices = new Dictionary<uint, int>();

        List<Vector3> vertices = new List<Vector3>(Mesh.vertices);
        List<Vector3> normals = new List<Vector3>(Mesh.normals);
        List<Vector2> uvs = new List<Vector2>(Mesh.uv);
        List<int> indices = new List<int>();

        int[] triangles = Mesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int i1 = triangles[i + 0];
            int i2 = triangles[i + 1];
            int i3 = triangles[i + 2];

            int a = SubdivideGetNewVertex(i1, i2, newVertices, vertices, normals, uvs);
            int b = SubdivideGetNewVertex(i2, i3, newVertices, vertices, normals, uvs);
            int c = SubdivideGetNewVertex(i3, i1, newVertices, vertices, normals, uvs);
            indices.Add(i1); indices.Add(a); indices.Add(c);
            indices.Add(i2); indices.Add(b); indices.Add(a);
            indices.Add(i3); indices.Add(c); indices.Add(b);
            indices.Add(a); indices.Add(b); indices.Add(c); // center triangle
        }
        Mesh.Clear();
        Mesh.vertices = vertices.ToArray();
        Mesh.normals = normals.ToArray();
        Mesh.triangles = indices.ToArray();
        Mesh.uv = uvs.ToArray();
        Mesh.RecalculateNormals();
        Mesh.RecalculateBounds();
        Mesh.RecalculateTangents();
    }

    public void FlipNormals()
    {
        Vector3[] normals = Mesh.normals;
        for (int i = 0; i < normals.Length; i++)
            normals[i] = -normals[i];

        Mesh.normals = normals;

        for (int m = 0; m < Mesh.subMeshCount; m++)
        {
            int[] triangles = Mesh.GetTriangles(m);
            
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int temp = triangles[i + 0];
                triangles[i + 0] = triangles[i + 1];
                triangles[i + 1] = temp;
            }

            Mesh.SetTriangles(triangles, m);
        }

        Flipped = !Flipped;
    }

    public void Generate(int subdivisions)
    {
        Mesh = GenerateMesh();

        Mesh.RecalculateNormals();
        Mesh.RecalculateBounds();
        Mesh.RecalculateTangents();

        for (int i = 0; i < subdivisions; i++)
            Subdivide();
    }

    protected abstract Mesh GenerateMesh();
}
