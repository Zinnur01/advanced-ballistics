using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
    public Vector3 a;
    public Vector3 b;
    public Vector3 c;

    public Triangle(Vector3 a, Vector3 b, Vector3 c)
    {
        this.a = a;
        this.b = b;
        this.c = c;
    }

    public IEnumerable<Vector3> Vertices
    {
        get
        {
            yield return a;
            yield return b;
            yield return c;
        }
    }
}
