using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPrimitiveGenerator
{
    MeshData Generate(Vector3 offset);
}
