using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldFaceCanvas : MonoBehaviour
{
    void Update()
    {
        Vector3 m_dirToCamera = Camera.main.transform.position - transform.position;
        m_dirToCamera.Normalize();
        m_dirToCamera.x = 0;
        transform.rotation = Quaternion.LookRotation(-m_dirToCamera);
    }
}
