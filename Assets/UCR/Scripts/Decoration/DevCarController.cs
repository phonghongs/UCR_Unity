using UnityEngine;
using PathCreation;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DevCarController : MonoBehaviour
{
    private PathCreator path;
    private PrometeoCarController car;

    [SerializeField][Range( 0.1f, 1f )] float flySpeed;
    [SerializeField] float y;
    [SerializeField] float lerp;

    List<Vector3> wps = new List<Vector3>();

    void Awake()
    {
#if !UNITY_EDITOR
        DestroyImmediate(gameObject);
#endif
    }

    void Start()
    {
        path = FindAnyObjectByType<PathCreator>();
        car = FindAnyObjectByType<PrometeoCarController>();
        car.enabled = false;

        foreach ( var rb in GetComponentsInChildren<Rigidbody>() )
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        path.pathUpdated += () => LoadPathPoints();
        LoadPathPoints();

        var pos = car.transform.position;
        pos.y = y;
        car.transform.position = pos;
    }


    void LoadPathPoints()
    {
        VertexPath path = this.path.path;
        wps.Clear();
        for ( int i = 0; i < path.NumPoints; i++ )
        {
            wps.Add( path.GetPoint( i ) );
        }
    }

    void Update()
    {
        while ( wps.Count > 0 )
        {
            Vector3 dir = wps[0] - car.transform.position;
            if ( dir.magnitude <= 10f )
            {
                wps.RemoveAt( 0 );
            }
            else break;
        }

        if ( wps.Count > 0 )
        {
            var speedMult = Input.GetKey( KeyCode.LeftShift )
                ? 1f
                : flySpeed;

            var dir = wps[0] - car.transform.position;
            dir.y = 0;

            car.transform.rotation = Quaternion.Lerp( car.transform.rotation, Quaternion.LookRotation( dir ), lerp );
            car.transform.Translate( 0f, 0f, car.maxSpeed * Time.deltaTime * speedMult );
        }
    }
}