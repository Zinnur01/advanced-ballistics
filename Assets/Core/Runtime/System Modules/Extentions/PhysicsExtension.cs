using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsExtension
{
    public static bool RaycastBoth(Vector3 start, Vector3 direction, out RaycastBothHit hitInfo)
    {
        hitInfo = new RaycastBothHit();

        if (Physics.Raycast(start, direction, out hitInfo.inHit))
        {
            if (Physics.Raycast(hitInfo.inHit.point + direction * 0.001f, direction, out RaycastHit nextHit))
            {
                Physics.Raycast(nextHit.point - direction * 0.001f, -direction, out hitInfo.outHit);
            }
            else
            {
                Physics.Raycast(hitInfo.inHit.point + direction * (hitInfo.inHit.collider.bounds.size.magnitude + 0.001f), -direction, out hitInfo.outHit);
            }

            hitInfo.gap = Vector3.Distance(hitInfo.inHit.point, hitInfo.outHit.point);

            return true;
        }
        return false;
    }

    public static bool LinecastBoth(Vector3 start, Vector3 end, out RaycastBothHit linecastBothHit)
    {
        Vector3 e = end - start;
        if (RaycastBoth(start, e.normalized, out RaycastBothHit raycastBothHit))
        {
            if (raycastBothHit.inHit.distance <= e.magnitude)
            {
                linecastBothHit = raycastBothHit;
                return true;
            }
        }

        linecastBothHit = new RaycastBothHit();
        return false;
    }

    public static RaycastHit[] LinecastAll(Vector3 start, Vector3 end, LayerMask mask)
    {
        Vector3 e = end - start;
        return Physics.RaycastAll(start, e.normalized, e.magnitude, mask);
    }
}

public struct RaycastBothHit
{
    public RaycastHit inHit;
    public RaycastHit outHit;

    public float gap;
}