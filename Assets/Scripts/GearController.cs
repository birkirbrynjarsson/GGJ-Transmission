using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearController : MonoBehaviour {

	public bool rotateClockwise;
	public float rotationSpeed;

	private float direction;

	// Use this for initialization
	void Start () {
		direction = (rotateClockwise) ? 1f : -1f;
		
	}

	// Update is called once per frame
	void FixedUpdate () {
		float targetRotation = transform.rotation.z + (rotationSpeed * Time.deltaTime * direction);
		//transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, targetRotation);
		
		//transform.rotation = Quaternion.Euler(-90f, 0f, targetRotation);
		GetComponent<Rigidbody>().AddTorque(transform.forward * rotationSpeed);

		//
		//transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime * direction));
	}
}
