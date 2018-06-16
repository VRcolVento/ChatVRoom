using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictonaryEntity : MonoBehaviour {

	int id = -1; 

	// Use these for initialization
		// Modify the element
	public void AddEntity(int id, string prefabName, Vector3 position, Quaternion rotation) {

//		SceneController.Dictionary.AddEntity(id, prefabName, position, rotation);
		UpdatePosition(position);
		UpdateRotation(rotation);

		Debug.Log("updated id: " + id);
	}
		// Add a new element
	public void AddEntity (string prefabName, Vector3 position, Quaternion rotation) {
		id = SceneController.Dictionary.AddEntity(prefabName, position, rotation);
		Debug.Log("new id: " + id);
	}


	public void RemoveEntity (int id) {
		SceneController.Dictionary.RemoveEntity(id);
		Debug.Log("Removed id " + id);
	}


	public void UpdatePosition (Vector3 position) {
		SceneController.Dictionary.UpdatePosition(id, position);
	}

	public void UpdateRotation (Quaternion rotation) {
		SceneController.Dictionary.UpdateRotation(id, rotation);
	}

	public int ID{
		get{ return id; }
		set{ this.id = value; }
	}
}
