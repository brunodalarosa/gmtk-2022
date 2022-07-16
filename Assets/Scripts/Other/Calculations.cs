using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Calculations
{
    public static float GetRotationZToTarget(Vector2 origin, Vector2 target)
    {
        Vector2 midPoint = target - origin;
        float rotationZ = Mathf.Atan2(midPoint.y, midPoint.x) * Mathf.Rad2Deg;

        return rotationZ;
    }

    public static float GetRotation3dToTarget(Vector3 origin, Vector3 target)
    {
        Vector3 midPoint = target - origin;
        float rotation = Mathf.Atan2(midPoint.y, midPoint.x) * Mathf.Rad2Deg;

        return rotation;
    }

    public static Vector2 GetDirectionToTarget(Vector2 origin, Vector2 target)
    {
        Vector2 midPoint = target - origin;
        float distance = midPoint.magnitude;
        Vector2 direction = midPoint / distance;
        direction.Normalize();

        return direction;
    }


}
