using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoAV.Editor.User{
	public class VRPlaceObject : MonoBehaviour {

		private SteamVR_TrackedObject trackedObj;
		private SteamVR_Controller.Device Controller {
			get { return SteamVR_Controller.Input((int)trackedObj.index); }
		}

		VRChooseObject chooseScript;

		GameObject objToPlace;
		string objName;
		ModifyObject modifyObjScript;
		UpdateLineRenderer lineRenderer;

		int roomMask;

		void Awake() {
			roomMask = LayerMask.GetMask("RoomLayer");
			Debug.Log("Room layer " + roomMask);
			trackedObj = GetComponent<SteamVR_TrackedObject>();
			chooseScript = GetComponent<VRChooseObject>();
			lineRenderer = GetComponent<UpdateLineRenderer>();
		}


		
		void Update () {

			// If I have chosen an obj and I need to place it

			Vector3 size = objToPlace.GetComponent<Renderer>().bounds.size;
			size = Vector3.Scale (size, new Vector3(0.5f, 0.5f, 0.5f));

			Ray ray = new Ray(lineRenderer.GetPosition(), lineRenderer.GetForward());

			RaycastHit hit;
			bool hitSomething = Physics.Raycast(ray, out hit, 1000f, roomMask);

			if (hitSomething) {

				Debug.Log("dasgdgdgdgdgd");
				// If I am hitting the room (filtered by the layer mask)

				objToPlace.transform.position = hit.point;

				// Vector3.Scale() == element wise product
				objToPlace.transform.position += Vector3.Scale (size, hit.normal);

				if(objToPlace.tag == "Obj_Floor"){

					objToPlace.transform.position = new Vector3(objToPlace.transform.position.x,
																size.y,
																objToPlace.transform.position.z);
				}
			}

			if(Controller.GetHairTriggerDown()) {
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
}