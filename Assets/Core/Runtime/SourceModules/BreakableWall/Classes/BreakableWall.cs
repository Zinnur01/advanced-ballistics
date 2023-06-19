using UnityEngine;

public class BreakableWall : MonoBehaviour, IDamageable
{
    [SerializeField]
    private BreakableWall template;

    [SerializeField]
    private int maxDepth = 3;

    // Stored required properties.
    private int depth;

    public void Damage(Vector3 point, float radius)
    {
        if (depth < maxDepth)
        {
            CreateParts(transform, point, radius);
        }

        Destroy(gameObject);
    }

    public void Damage(RaycastHit hitInfo)
    {

    }

    public void Damage()
    {
        throw new System.NotImplementedException();
    }

    private void CreateParts(Transform transform, Vector3 point, float radius)
    {
        Vector3 side = new Vector3(Mathf.Sign(point.x - transform.position.x), Mathf.Sign(point.y - transform.position.y), Mathf.Sign(point.z - transform.position.z));
        Vector3 size = transform.lossyScale / 2f;
        Vector3 offset = size / 2f;

        for (int i = -1; i <= 1; i += 2)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                for (int k = -1; k <= 1; k += 2)
                {
                    Vector3 a = new Vector3(i, j, k);
                    Vector3 b = transform.position + Vector3.Scale(offset, a);
                    BreakableWall wall = Instantiate<BreakableWall>(template, b, transform.rotation);
                    wall.transform.localScale = size;
                    wall.depth = depth + 1;

                    if (side == a)
                    //if (Vector3.Distance(point, b) < radius + offset.magnitude)
                    {
                        wall.Damage(point, radius);
                    }
                }
            }
        }
    }
}
