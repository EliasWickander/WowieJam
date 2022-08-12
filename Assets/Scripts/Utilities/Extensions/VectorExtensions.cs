using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 Vector2To3(Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }

    public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
    }

    //Returns a vector pointing in a specified angle
    public static Vector3 AngleVector(float angleInDeg)
    {
        float angleInRad = angleInDeg * Mathf.Deg2Rad;

        return new Vector3(Mathf.Sin(angleInRad), 0, Mathf.Cos(angleInRad));
    }

    public static float AngleBetweenPoints(Vector3 p1, Vector3 p2)
    {
        return Mathf.Atan2(p2.x - p1.x, p2.z - p1.z);
    }
}
