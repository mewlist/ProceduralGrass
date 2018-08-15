using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PrimitiveGenerator : MonoBehaviour
{
    public enum GeneratorType
    {
        Triangle,
        TriangleTransformed,
        Quad,
        QuadTransformed,
    }

    [SerializeField] private GeneratorType generatorType;

    private void OnValidate()
    {
        CreateMesh();
    }

    private void Start()
    {
    }

    private void CreateMesh()
    {
        var mesh = new Mesh();
        var generator = GetGenerator(generatorType);
        var meshData = generator.Generate(Vector3.zero);
        mesh.SetVertices(meshData.Verts);
        mesh.SetTriangles(meshData.Triangles, 0);

        GetComponent<MeshFilter>().mesh = mesh;
    }

    private IPrimitiveGenerator GetGenerator(GeneratorType generatorType)
    {
        switch (generatorType)
        {
            case GeneratorType.Triangle:
                return new TrianglePrimitiveGenerator();
            case GeneratorType.TriangleTransformed:
                return new TriangleTransformedPrimitiveGenerator();
            case GeneratorType.Quad:
                return new QuadPrimitiveGenerator();
            case GeneratorType.QuadTransformed:
                return new QuadTransformedPrimitiveGenerator();
            default:
                throw new ArgumentOutOfRangeException("generatorType", generatorType, null);
        }
    }
}