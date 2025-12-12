using Cinemachine;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

[RequireComponent( typeof( Collider ) )]
public class AutoTriggerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private bool autoDisable = true;

    private CameraManager cameraManager;
    private CarController car;
    private Collider collider;

    void Awake()
    {
        if ( autoDisable && camera.enabled ) camera.enabled = false;
        collider = GetComponent<Collider>();
        collider.isTrigger = true;

        cameraManager = FindAnyObjectByType<CameraManager>();
        car = FindAnyObjectByType<CarController>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log( $"Trigegr enter {other.name}" );
        if ( other.CompareTag( "Player" ) )
        {
            cameraManager.DisableAllCamera();
            camera.enabled = true;
            camera.gameObject.SetActive( true );
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log( $"Trigegr exit {other.name}" );
        if ( other.CompareTag( "Player" ) )
        {
            camera.enabled = false;
            camera.gameObject.SetActive( false );
            cameraManager.ResetSoloCamera();
        }
    }
}