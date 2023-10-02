using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	private GameObject[] carObject;
	private Transform[] carTransform;
	private bool isFindObject = false;
	[Range(1, 10)]
	public float followSpeed = 2;
	[Range(1, 10)]
	public float lookSpeed = 5;
	Vector3 initialCameraPosition;

	void Start(){
		FindPlayer();
	}

	void Update(){
		if (!isFindObject){
			FindPlayer();
			Debug.Log("OK");
		}
	}

	void FixedUpdate()
	{
		if (isFindObject){
			if (carTransform.Length > 0){
				Vector3 _lookDirection = (new Vector3(carTransform[0].position.x, carTransform[0].position.y, carTransform[0].position.z)) - transform.position;
				
				if (_lookDirection != Vector3.zero){
					Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
					transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);
				}

				//Move to car
				Vector3 _targetPos = initialCameraPosition + carTransform[0].transform.position;
				transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
			}
		}
	}

	void FindPlayer(){
		carObject = GameObject.FindGameObjectsWithTag("Player");
		carTransform = new Transform[carObject.Length];
		
		for (int i = 0; i < carObject.Length; i ++){
			carTransform[i] = carObject[i].GetComponent<Transform>();
		}
		initialCameraPosition = gameObject.transform.position;

		if (carObject.Length > 0){
			isFindObject = true;
		}
	}
}
