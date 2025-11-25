using Cinemachine;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

[RequireComponent( typeof( Collider ) )]
public class AutoTriggerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;

    private CameraManager cameraManager;
    private CarController car;
    private Collider collider;


    void Awake()
    {
        if ( camera.enabled ) camera.enabled = false;
        collider = GetComponent<Collider>();
        collider.isTrigger = true;

        cameraManager = FindAnyObjectByType<CameraManager>();
        car = FindAnyObjectByType<CarController>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log( $"Trigegr enter {other.name}" );
        if ( !camera.enabled && other.CompareTag( "Player" ) )
        {
            cameraManager.DisableAllCamera();
            camera.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log( $"Trigegr exit {other.name}" );
        if ( camera.enabled && other.CompareTag( "Player" ) )
        {
            camera.enabled = false;
            cameraManager.ResetSoloCamera();
        }
    }
}