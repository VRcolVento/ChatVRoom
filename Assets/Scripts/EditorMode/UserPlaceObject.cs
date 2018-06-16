using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlaceObject : MonoBehaviour {


	UserChooseObject chooseScript;

	GameObject objToPlace;
	string objName;
	ModifyObject modifyObjScript;
	UpdateLineRenderer lineRenderer;

	int roomMask;
	
	void Awake () {
		chooseScript = GetComponent<UserChooseObject>();
	}
	
	void Start() {
		roomMask = LayerMask.GetMask("RoomLayer");
		lineRenderer = GameObject.Find("Mano").GetComponent<UpdateLineRenderer>();
	}

	void Update () {
		// If I have chosen an obj and I need to place it

		Vector3 size = objToPlace.GetComponent<Renderer>().bounds.size;
		size = Vector3.Scale (size, new Vector3(0.5f, 0.5f, 0.5f));

		Ray ray = new Ray(lineRenderer.GetPosition(), lineRenderer.GetForward());

		RaycastHit hit;
		bool hitSomething = Physics.Raycast(ray, out hit, 1000f, roomMask);

		if (hitSomething) {
			// If I am hitting the room (filtered by the layer mask)

			objToPlace.transform.position = hit.point;

			// Vector3.Scale() == element wise product
			objToPlace.transform.position += Vector3.Scale (size, hit.normal);

			if(objToPlace.tag == "Obj_Floor"){

				objToPlace.transform.position = new Vector3(objToPlace.transform.position.x,
															size.y,
															objToPlace.transform.position.z);
			}

			if(objToPlace.tag == "Obj_Ceiling") {

				// TODO attaccarlo solo al soffitto?
			}
		}

		if(Input.GetButtonDown("Fire1")) {
			// Left mouse button, place the object

			if(!modifyObjScript.IsColliding){

				// Freeze the position and rotation of the placed object (altrimenti quando si cozzano si spostano)
				Rigidbody objRb = objToPlace.GetComponent<Rigidbody>();
				objRb.constraints = RigidbodyConstraints.FreezePositionX | 
									RigidbodyConstraints.FreezePositionY |
									RigidbodyConstraints.FreezePositionZ |
									RigidbodyConstraints.FreezeRotationX | 
									RigidbodyConstraints.FreezeRotationY |
									RigidbodyConstraints.FreezeRotationZ;
				
				// Save the object.
				DictonaryEntity objEntity = objToPlace.GetComponent<DictonaryEntity>();
				if(objEntity.ID == -1){
					// Not already stored
					objToPlace.GetComponent<DictonaryEntity>().AddEntity(objName, objToPlace.transform.position, objToPlace.transform.rotation);
				} else {
					// Already stored
					objToPlace.GetComponent<DictonaryEntity>().AddEntity(objEntity.ID, objName, objToPlace.transform.position, objToPlace.transform.rotation);
				}
				
				switchMode();
			} 
		}

		if(Input.GetKeyDown(KeyCode.Backspace)){
			// Delete the object
			
			DictonaryEntity objEntity = objToPlace.GetComponent<DictonaryEntity>();
			if(objEntity.ID != -1){
				// The object was previuosly stored, delete the entry at the dictionary
				objEntity.RemoveEntity(objEntity.ID);
			}

			objToPlace.GetComponent<Interactible>().RemoveSelectionEvent();	// Unsubscribe to changing color events
			Destroy(objToPlace);
			switchMode();
		}	
	}


	public void setObject(GameObject obj, string name) {
		// Called by the UserController script to pass the data and activate this script

		objToPlace = obj;
		objName = name;
		modifyObjScript = objToPlace.GetComponent<ModifyObject>();
		modifyObjScript.enabled = true;
	}


	// Helpers
	private void switchMode(){
		// Go back to choose mode

		objToPlace.GetComponent<Interactible>().AddSelectionEvent();
		chooseScript.enabled = true;
		modifyObjScript.enabled = false;
		this.enabled = false;
	}
}
