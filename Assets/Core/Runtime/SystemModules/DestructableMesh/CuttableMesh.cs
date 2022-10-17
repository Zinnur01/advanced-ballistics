using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CuttableMesh : MonoBehaviour
{
    [SerializeField]
    private Transform planeObject;

    // Stored required properties.
    private Mesh mesh;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
    }

    private void OnDrawGizmos()
    {
        if (mesh == null || planeObject == null) return;

        DrawPlane(planeObject.position, planeObject.forward);

        Plane plane = new Plane(planeObject.forward, planeObject.position);

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 A = mesh.vertices[mesh.triangles[i]];
            Vector3 B = mesh.vertices[mesh.triangles[i + 1]];
            Vector3 C = mesh.vertices[mesh.triangles[i + 2]];

            bool sideA = plane.GetSide(A);
            bool sideB = plane.GetSide(B);
            bool sideC = plane.GetSide(C);

            int sideCount = (sideA ? 1 : 0) + (sideB ? 1 : 0) + (sideC ? 1 : 0);

            if (sideCount == 1 || sideCount == 2)
            {
                int singleIndex = sideB == sideC ? 0 : sideA == sideC ? 1 : 3;

                Vector3 AA = mesh.vertices[mesh.triangles[i + singleIndex]];
                Vector3 BB = mesh.vertices[mesh.triangles[i + (singleIndex + 1) % 3]];
                Vector3 CC = mesh.vertices[mesh.triangles[i + (singleIndex + 2) % 3]];

                Vector3 dirAB = BB - AA;
                Vector3 dirAC = CC - AA;

                Ray rayAB = new Ray(AA, dirAB);
                Ray rayAC = new Ray(AA, dirAC);

                plane.Raycast(rayAB, out float enterAB);
                float lerpAB = enterAB / dirAB.magnitude;

                plane.Raycast(rayAC, out float enterAC);
                float lerpAC = enterAC / dirAC.magnitude;

                if (lerpAB > 0 && lerpAB < dirAB.magnitude)
                {
                    Vector3 APB = Vector3.Lerp(AA, BB, lerpAB);
                    DrawPoint(APB);
                }

                if (lerpAC > 0 && lerpAC < dirAC.magnitude)
                {
                    Vector3 APC = Vector3.Lerp(AA, CC, lerpAC);
                    DrawPoint(APC);
                }
            }
        }
    }

    private void DrawPlane(Vector3 position, Vector3 normal)
    {
        Vector3 r = Vector3.Cross(Vector3.up, normal).normalized;
        Vector3 u = Vector3.Cross(normal, r).normalized;

        Vector3 aa = position + r + u;
        Vector3 ab = position + r - u;
        Vector3 ba = position - r + u;
        Vector3 bb = position - r - u;

        Gizmos.DrawLine(aa, ab);
        Gizmos.DrawLine(ab, bb);
        Gizmos.DrawLine(bb, ba);
        Gizmos.DrawLine(ba, aa);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(position, r);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(position, u);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(position, normal);
    }

    private void DrawPoint(Vector3 position)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(position, .02f);
    }
}
