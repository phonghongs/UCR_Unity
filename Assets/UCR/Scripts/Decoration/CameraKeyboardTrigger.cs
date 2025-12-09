using System;
using Cinemachine;
using UnityEngine;

public class CameraKeyboardTrigger : MonoBehaviour
{
    [SerializeField] KeyCode[] keys;

    CameraManager cameraManager;

    void Awake()
    {
        cameraManager = FindAnyObjectByType<CameraManager>();
    }

    void Update()
    {
        for ( int i = 0; i < keys.Length; i++ )
        {
            if ( Input.GetKeyUp( keys[i] ) )
            {
                cameraManager.EnableSoloCamera( i );
                break;
            }
        }
    }
}