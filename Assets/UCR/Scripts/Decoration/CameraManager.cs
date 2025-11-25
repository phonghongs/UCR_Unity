using System;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Serializable]
    public class SoloCameraProfile
    {
        public KeyCode enableKey;
        public CinemachineVirtualCamera camera;
    }

    [SerializeField] SoloCameraProfile[] cameraProfiles;

    CinemachineVirtualCamera[] soloCameras;

    void Start()
    {
        soloCameras = cameraProfiles
            .Select( cp => cp.camera )
            .ToArray();

        ResetSoloCamera();
    }

    void Update()
    {
        for ( int i = 0; i < cameraProfiles.Length; i++ )
        {
            if ( Input.GetKeyUp( cameraProfiles[i].enableKey ) )
            {
                EnableSoloCamera( i );
                break;
            }
        }
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