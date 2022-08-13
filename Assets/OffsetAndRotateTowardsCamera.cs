using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetAndRotateTowardsCamera : MonoBehaviour
{
    private Camera m_camera;

    public Vector3 m_cameraOffset = new Vector3(0, 0, 5);

    private void Awake()
    {
        m_camera = Camera.main;
    }

    private void Start()
    {
        transform.position = m_camera.transform.position + 
                             m_camera.transform.forward * m_cameraOffset.z +
                             m_camera.transform.up * m_cameraOffset.y +
                             m_camera.transform.right * m_cameraOffset.x;
    }

    private void Update()
    {
        Vector3 m_dirToCamera = m_camera.transform.position - transform.position;
        m_dirToCamera.Normalize();
        m_dirToCamera.x = 0;
        transform.rotation = Quaternion.LookRotation(-m_dirToCamera);
    }
}
