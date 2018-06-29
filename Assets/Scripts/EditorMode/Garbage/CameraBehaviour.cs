using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Script to attach to the Main Camera
	Handles:
		- Rotation with the mouse
		- Movement with WASD
*/
public class CameraBehaviour : MonoBehaviour {

	public float speed = 1.0f;

	public float speedH = 2.0f;
	public float speedV = 2.0f;

	private float yaw = 0.0f;
	private float pitch = 0.0f;

	private float yPos;
	bool hasCollided;

	private Rigidbody rb;

	void Start(){
	
		yPos = transform.position.y;
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		// Rotate
		yaw += speedH * Input.GetAxis("Mouse X");
		pitch -= speedV * Input.GetAxis("Mouse Y");

		// Avoid 360.
		if(pitch > 85)			pitch = 85;
		else if(pitch < -85) 	pitch = -85;

		transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
		
		// Move
		Vector3 position = transform.position;

		if (Input.GetKey ("w"))
			position += transform.forward * Time.deltaTime*speed;
		else if (Input.GetKey ("s"))
			position -= transform.forward * Time.deltaTime*speed;

		if (Input.GetKey ("a"))
			position -= transform.right * Time.deltaTime*speed;
		else if (Input.GetKey ("d"))
			position += transform.right * Time.deltaTime*speed;

		position.y = yPos;
		transform.position = position;
	}

	void OnCollisionExit(Collision collision){
		rb.velocity = new Vector3(0,0,0);
	}

}
