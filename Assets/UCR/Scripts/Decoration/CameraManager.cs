using System;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera[] soloCameras;

    void Start()
    {
        ResetSoloCamera();
    }

    public void ResetSoloCamera()
    {
        EnableSoloCamera( 0 );
    }

    public void EnableSoloCamera(int index)
    {
        for ( int i = 0; i < soloCameras.Length; i++ )
        {
            CinemachineVirtualCamera virtualCamera = soloCameras[i];
            if ( virtualCamera != null )
            {
                virtualCamera.gameObject.SetActive( index == i );
            }
        }
    }

    public void DisableAllCamera()
    {
        EnableSoloCamera( -1 );
    }
}