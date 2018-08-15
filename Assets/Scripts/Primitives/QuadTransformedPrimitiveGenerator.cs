using System.Collections.Generic;
using UnityEngine;

public class QuadTransformedPrimitiveGenerator : IPrimitiveGenerator
{
    public MeshData Generate(Vector3 offset)
    {
        var verts = new List<Vector3>();
        var tris = new List<int>();

        verts.Add(offset + new Vector3(0f, 0f, 0f));
        verts.Add(offset + new Vector3(-0.5f, 0.5f, 0f));
        verts.Add(offset + new Vector3(0f, 1f, 0f));
        verts.Add(offset + new Vector3(0.5f, 0.5f, 0f));

        tris.Add(0);
        tris.Add(1);
        tris.Add(2);
        tris.Add(2);
        tris.Add(3);
        tris.Add(0);

        return new MeshData(verts, tris);
    }
}