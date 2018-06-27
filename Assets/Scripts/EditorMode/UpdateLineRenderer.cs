using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLineRenderer : MonoBehaviour {

	LineRenderer lr;
	Vector3 endPoint;

	void Start () {
		
		lr = GetComponent<LineRenderer>();
		endPoint = transform.position + transform.forward*20;
	}
	
	// Update is called once per frame
	void Update () {
		
		endPoint = transform.position + transform.forward*20;
		lr.SetPosition(0, transform.position);
		lr.SetPosition(1, endPoint);
	}

	public Vector3 GetForward() {

		return transform.forward;
	}

	public Vector3 GetPosition() {
		
		return transform.position;
	}

	public LineRenderer GetRenderer() {
		return lr;
	}

	public void UpdateEnd(Vector3 end){
		endPoint = end;
	}

	public void ResetEnd() {
		endPoint = transform.position + transform.forward*20;
	}
}
