using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{

    [SerializeField] private InputActionAsset inputProvider;
    [SerializeField] private CinemachineVirtualCamera freeLookCameraToZoom;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float zoomAcceleration = 2.5f;
    [SerializeField] private float zoomInnerRange = 3;
    [SerializeField] private float zoomOuterRange = 50;

    private float currentMiddleRigRadius = 10f;
    private float newMiddleRigRadius=10f;
    [SerializeField] private float zoomYAxis = 0f;

    public float ZoomYAxis
    {
        get  { return zoomYAxis; }
        set
            {
            if (zoomYAxis == value) return;
            zoomYAxis = value;
            AdjustCameraZoomIndex(ZoomYAxis);
            }
    }

    private void Awake()
    {
        inputProvider.FindActionMap("Camera Controls").FindAction("Mouse Zoom").performed += cntxt => ZoomYAxis = cntxt.ReadValue<float>();
        inputProvider.FindActionMap("Camera Controls").FindAction("Mouse Zoom").canceled += cntxt => ZoomYAxis = 0f;
    }

    private void OnEnable()
    {
        inputProvider.FindAction("Mouse Zoom").Enable();

    }

    private void OnDisable()
    {
        inputProvider.FindAction("Mouse Zoom").Disable();
    }

    private void LateUpdate()
    {
        UpdateZoomLevel();
    }

    private void UpdateZoomLevel()
    {
        if (currentMiddleRigRadius == newMiddleRigRadius) { return; }

        currentMiddleRigRadius = Mathf.Lerp(currentMiddleRigRadius, newMiddleRigRadius, zoomAcceleration * Time.deltaTime);
        currentMiddleRigRadius = Mathf.Clamp(currentMiddleRigRadius, zoomInnerRange, zoomOuterRange);

        
        //freeLookCameraToZoom.m_Orbits[1].m_Radius = currentMiddleRigRadius;
        //freeLookCameraToZoom.m_Orbits[0].m_Height = freeLookCameraToZoom.m_Orbits[1].m_Radius;
        //freeLookCameraToZoom.m_Orbits[1].m_Height = -freeLookCameraToZoom.m_Orbits[1].m_Radius;
    }

    public void AdjustCameraZoomIndex(float zoomYAxis)
    {
        if (ZoomYAxis == 0) { return; }
        if (zoomYAxis < 0)
        {
            newMiddleRigRadius = currentMiddleRigRadius + zoomSpeed;
        }
        if (zoomYAxis > 0)
        {
            newMiddleRigRadius = currentMiddleRigRadius - zoomSpeed;
        }
    }

    //private void Update()
    //{
    //    //if (componentBase == null)
    //    //{
    //    //    componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
    //    //}

    //    //if( !=0)
    //    //{
    //    //    cameraDistance = Input.GetAxis("Mouse ScrollWheel") * sensitivity;
    //    //    if(componentBase is CinemachineFramingTransposer)
    //    //    {
    //    //        (componentBase as CinemachineFramingTransposer).m_CameraDistance -= cameraDistance;
    //    //    }
    //    //}
    //}
}
