using ApexInspector;
using csDelaunay;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shatter : MonoBehaviour
{
    [SerializeField]
    private int fragmentCount = 30;

    [SerializeField]
    private int lloydIterations = 0;

    [SerializeField]
    private Vector2 size = Vector2.one;

    [SerializeField]
    private float depth = .1f;

    [SerializeField]
    [MinMaxSlider(0f, 1f)]
    private float previewSize = 1f;

    [SerializeField]
    private bool drawPattern;

    // Stored required properties.
    private Voronoi voronoi;

    private Mesh[] meshes = null;
    private Vector3[] pivots = null;
    private GameObject[] lastFragments;

    [Button("Fragment")]
    private void Fragment()
    {
        if (voronoi == null) CreateVoronoi();

        CalculateFragments(ref meshes, ref pivots, voronoi);

        if (lastFragments != null)
        {
            for (int i = 0; i < lastFragments.Length; i++)
            {
                DestroyImmediate(lastFragments[i]);
            }
            lastFragments = null;
        }

        lastFragments = CreateFragments();
    }

    private void CalculateFragments(ref Mesh[] meshes, ref Vector3[] pivots, Voronoi voronoi)
    {
        List<List<Vector2>> regions = voronoi.Regions();

        meshes = new Mesh[regions.Count];
        pivots = new Vector3[regions.Count];

        for (int i = 0; i < regions.Count; i++)
        {
            Vector3 pivot = Vector3.zero;

            Mesh mesh = CreateMesh(ref pivot, regions[i], depth);

            meshes[i] = mesh;
            pivots[i] = pivot;
        }
    }

    private Mesh CreateMesh(ref Vector3 pivot, List<Vector2> region, float depth)
    {
        Mesh mesh = new Mesh();

        float halfDepth = depth * .5f;

        List<Vector3> front = new List<Vector3>();
        List<Vector3> back = new List<Vector3>();

        for (int i = 0; i < region.Count; i++)
        {
            int invIndex = region.Count - i - 1;
            front.Add(new Vector3(region[i].x, region[i].y, halfDepth));
            back.Add(new Vector3(region[invIndex].x, region[invIndex].y, -halfDepth));
        }

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        Triangulation.SplitTriangles(Triangulation.TriangulateConvexPolygon(front), ref vertices, ref triangles);
        Triangulation.SplitTriangles(Triangulation.TriangulateConvexPolygon(back), ref vertices, ref triangles);

        for (int i = 0; i < region.Count; i++)
        {
            List<Vector3> points = new List<Vector3>();

            int aa = i;
            int ab = (i + 1) % region.Count;
            int ba = (region.Count - (i + 1) % region.Count) % region.Count;
            int bb = (region.Count - (i + 2) % region.Count) % region.Count;

            points.Add(front[aa]);
            points.Add(back[ba]);
            points.Add(back[bb]);
            points.Add(front[ab]);

            Triangulation.SplitTriangles(Triangulation.TriangulateConvexPolygon(points), ref vertices, ref triangles);
        }

        for (int i = 0; i < vertices.Count; i++)
        {
            pivot += vertices[i];
        }
        pivot = pivot / vertices.Count;
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] -= pivot;
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }

    private GameObject[] CreateFragments()
    {
        if (meshes == null)
            return null;

        GameObject[] fragments = new GameObject[meshes.Length];

        GameObject fragmentInstance = new GameObject();
        fragmentInstance.AddComponent<MeshFilter>();
        fragmentInstance.AddComponent<MeshRenderer>();
        fragmentInstance.AddComponent<MeshCollider>();
        fragmentInstance.AddComponent<Rigidbody>();

        for (int i = 0; i < meshes.Length; i++)
        {
            GameObject fragment = Instantiate(fragmentInstance);

            fragment.transform.position = transform.position + pivots[i];
            fragment.transform.parent = transform;

            MeshFilter filter = fragment.GetComponent<MeshFilter>();
            filter.sharedMesh = meshes[i];
            filter.sharedMesh.name = fragment.name;

            MeshRenderer renderer = fragment.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = new Material(Shader.Find("HDRP/Lit"));

            MeshCollider collider = fragment.GetComponent<MeshCollider>();
            collider.sharedMesh = meshes[i];
            collider.convex = true;

            Rigidbody rigidbody = fragment.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;

            fragments[i] = fragment;
        }

        DestroyImmediate(fragmentInstance);

        meshes = null;
        pivots = null;

        return fragments;
    }

    [Button("Create Pattern")]
    private void CreateVoronoi()
    {
        if (lastFragments != null)
        {
            for (int i = 0; i < lastFragments.Length; i++)
            {
                DestroyImmediate(lastFragments[i]);
            }
            lastFragments = null;
        }

        List<Vector2> points = CreateRandomPoints(fragmentCount);
        Rect bounds = new Rect(-size.x / 2, -size.y / 2, size.x, size.y);
        voronoi = new Voronoi(points, bounds, lloydIterations);

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            SceneView.RepaintAll();
        }
#endif
    }

    private void OnValidate()
    {
        if (lastFragments != null)
        {
            foreach (GameObject frag in lastFragments)
            {
                frag.transform.localScale = Vector3.one * previewSize;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, depth));

        if (drawPattern && voronoi != null)
        {
            Gizmos.color = Color.blue;
            foreach (var item in voronoi.SitesIndexedByLocation)
            {
                Vector3 point = new Vector3(item.Key.x, item.Key.y, 0);
                Gizmos.DrawSphere(transform.position + point, 0.01f);
            }

            Gizmos.color = Color.black;
            foreach (Edge edge in voronoi.Edges)
            {
                if (edge.ClippedEnds == null) continue;

                Vector3 a = new Vector3(edge.ClippedEnds[LR.LEFT].x, edge.ClippedEnds[LR.LEFT].y, 0);
                Vector3 b = new Vector3(edge.ClippedEnds[LR.RIGHT].x, edge.ClippedEnds[LR.RIGHT].y, 0);

                Gizmos.DrawLine(transform.position + a, transform.position + b);
            }

            Gizmos.color = Color.yellow;
            foreach (var item in voronoi.Regions())
            {
                foreach (var e in item)
                {
                    Gizmos.DrawSphere(transform.position + new Vector3(e.x, e.y, 0), 0.01f);
                }
            }
        }
    }

    private List<Vector2> CreateRandomPoints(int count)
    {
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(-size.x / 2f, size.x / 2f);
            float y = Random.Range(-size.y / 2f, size.y / 2f);

            points.Add(new Vector2(x, y));
        }
        return points;
    }
}
