using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    public static Vector3 LinearCurve(Vector3 p0, Vector3 p1, float t)
    {
        Vector3 pFinal = new Vector3();

        pFinal.x = (1 - t) * p0.x + t * p1.x;
        pFinal.y = (1 - t) * p0.y + t * p1.y;
        pFinal.z = (1 - t) * p0.z + t * p1.z;

        return pFinal;
    }
    
    public static Vector3 QuadraticCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 pFinal = new Vector3();

        pFinal.x = Mathf.Pow(1 - t, 2) * p0.x + (1 - t) * 2 * t * p1.x + t * t * p2.x;
        pFinal.y = Mathf.Pow(1 - t, 2) * p0.y + (1 - t) * 2 * t * p1.y + t * t * p2.y;
        pFinal.z = Mathf.Pow(1 - t, 2) * p0.z + (1 - t) * 2 * t * p1.z + t * t * p2.z;

        return pFinal;
    }

    public static Vector3 CubicCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 pFinal = new Vector3();

        pFinal.x = Mathf.Pow(1 - t, 3) * p0.x + Mathf.Pow(1 - t, 2) * 3 * t * p1.x + (1 - t) * 3 * t * t * p2.x +
                   t * t * t * p3.x; 
        pFinal.y = Mathf.Pow(1 - t, 3) * p0.y + Mathf.Pow(1 - t, 2) * 3 * t * p1.y + (1 - t) * 3 * t * t * p2.y +
                   t * t * t * p3.y; 
        pFinal.z = Mathf.Pow(1 - t, 3) * p0.z + Mathf.Pow(1 - t, 2) * 3 * t * p1.z + (1 - t) * 3 * t * t * p2.z +
                   t * t * t * p3.z; 

        return pFinal;
    }
}
