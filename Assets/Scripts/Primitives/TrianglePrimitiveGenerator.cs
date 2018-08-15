using System.Collections.Generic;
using UnityEngine;

public struct MeshData
{
    public List<Vector3> Verts;
    public List<int> Triangles;

    public MeshData(List<Vector3> verts, List<int> tris)
    {
        Verts = verts;
        Triangles = tris;
    }
}

public class TrianglePrimitiveGenerator : IPrimitiveGenerator
{
    public MeshData Generate(Vector3 offset)
    {
        var verts = new List<Vector3>();
        var tris = new List<int>();

        verts.Add(offset + new Vector3(-0.5f, 0f, 0f));
        verts.Add(offset + new Vector3(0f, 1f, 0f));
        verts.Add(offset + new Vector3(0.5f, 0f, 0f));

        tris.Add(0);
        tris.Add(1);
        tris.Add(2);

        return new MeshData(verts, tris);
    }
}