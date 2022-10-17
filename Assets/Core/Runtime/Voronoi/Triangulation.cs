using System.Collections.Generic;
using UnityEngine;

public static class Triangulation
{
    public static List<Triangle> TriangulateConvexPolygon(List<Vector3> convexHullpoints)
    {
        List<Triangle> triangles = new List<Triangle>(convexHullpoints.Count - 2);
        for (int i = 0; i < convexHullpoints.Count - 2; i++)
        {
            triangles.Add(new Triangle(convexHullpoints[0], convexHullpoints[i + 1], convexHullpoints[i + 2]));
        }
        return triangles;
    }

    public static void SplitTriangles(List<Triangle> list, ref List<Vector3> vertices, ref List<int> triangles)
    {
        List<Vector3> newVertices = new List<Vector3>();

        foreach (Triangle triangle in list)
        {
            foreach (Vector3 vertex in triangle.Vertices)
            {
                if (!newVertices.Contains(vertex))
                {
                    newVertices.Add(vertex);
                }
                triangles.Add(newVertices.IndexOf(vertex) + vertices.Count);
            }
        }

        vertices.AddRange(newVertices);
    }
}
