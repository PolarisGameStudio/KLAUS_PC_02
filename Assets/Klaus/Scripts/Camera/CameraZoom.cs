//
// CameraZoom.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;

public class CameraZoom : Singleton<CameraZoom>
{


    CameraZoomPlugin _camZ = null;

    public CameraZoomPlugin cameraZoom
    {

        get
        {
            if (_camZ == null)
                _camZ = GetComponent<CameraZoomPlugin>();

            return _camZ;
        }
    }
    protected float storeZoom = 0;
    void Start()
    {
        storeZoom = cameraZoom.ZoomSmoothness;
    }

    public void SetTargetSizeIndme(float newSize)
    {
        cameraZoom.SetZoom(newSize);

    }

    public void SetTargetSize(float newSize, float newTime)
    {
        cameraZoom.ChangeZoom(newSize, newTime);

    }
    public void SetTargetSize(float newSize)
    {
        SetTargetSize(newSize, storeZoom);

    }
    public void FinishZoom()
    {
        cameraZoom.FinishZoom();
    }

    public void StopZoom()
    {
        cameraZoom.StopZoom();
    }

}

