using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

	public Transform userTransform;
	private bool isIn = false;

	public bool IsIn {
		get { return isIn;  }
		set { isIn = value; }
	}

	private Vector3 targetPosition;
	private Vector3 spawnPosition;
	private Vector3 endPosition;

	void Start () {
		spawnPosition = userTransform.position + userTransform.forward*15f + new Vector3(-50, 0, 0);
		endPosition = -spawnPosition;
		targetPosition = spawnPosition;	
	}

	void Update () {
		
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime*5);
		transform.LookAt(userTransform.position);
	}

	public void swipeIn() {

		spawnPosition = userTransform.position + userTransform.right*50;
		endPosition = Vector3.Scale(spawnPosition, new Vector3(-1, 1, -1));
		transform.position = spawnPosition;
		targetPosition = userTransform.position + userTransform.forward*15f;
		targetPosition.y = 5;
		isIn = true;
	}

	public void swipeOut() {

		targetPosition = endPosition;
		isIn = false;
	}
}
