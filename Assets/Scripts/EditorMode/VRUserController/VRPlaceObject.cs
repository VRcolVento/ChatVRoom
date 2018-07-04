using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoAV.Editor.ObjectUtil;
using DemoAV.Editor.StorageUtility;

namespace DemoAV.Editor.User{

	/// <summary>
	/// Class to handle the placement of an object by the user.
	/// This class is intended to use in VR environment.
	/// </summary>
	public class VRPlaceObject : MonoBehaviour {

		// The steam VR controller
		private SteamVR_TrackedObject trackedObj;
		private SteamVR_Controller.Device Controller {
			get { return SteamVR_Controller.Input((int)trackedObj.index); }
		}

		// Reference to the choosing script, to activate after the user has placed the object
		VRChooseObject chooseScript;

		// Information about the object to place
		GameObject objToPlace;
		string objName, objPath;
		
		// Refernce to the modification script internal to the object
		ModifyObject modifyObjScript;

		// Masks
		int roomMask;

		// Handle swipe for rotation
		Vector2 initSwipePos = Vector2.zero;	

		void Awake() {
			roomMask = LayerMask.GetMask("RoomLayer");
			trackedObj = GetComponent<SteamVR_TrackedObject>();
			chooseScript = GetComponent<VRChooseObject>();
		}



		void Update () {

			// Rotate if the user swipes

			// Check swipe for rotation
			Vector2 padPos = Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
			if(padPos != Vector2.zero && initSwipePos == Vector2.zero){
				// User presses first
				initSwipePos = padPos;
			}
			else if(padPos == Vector2.zero && initSwipePos != Vector2.zero) {
				// User swipes
				if(padPos.x > initSwipePos.x)
					modifyObjScript.RotateObject(-1);
				else
					modifyObjScript.RotateObject(1);
				
				initSwipePos = Vector2.zero;
			}
				

			// Update object position

			Vector3 size = objToPlace.GetComponent<Renderer>().bounds.size;
			size = Vector3.Scale(size, new Vector3(0.5f, 0.5f, 0.5f));

			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 1000f, roomMask)) {
				// If I am hitting the room (filtered by the layer mask)

				objToPlace.transform.position = hit.point;

				// Vector3.Scale() == element wise product
				objToPlace.transform.position += Vector3.Scale(size, hit.normal);

				if(objToPlace.tag == "Obj_Floor"){
					// Constrain a floor object to stay on the floor
					objToPlace.transform.position = new Vector3(objToPlace.transform.position.x,
																size.y,
																objToPlace.transform.position.z);
				}
			}


			// Place the object

			if(Controller.GetHairTriggerDown()) {

				if(!modifyObjScript.IsColliding){
					// Place only if is not colliding with any other objects

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
						objToPlace.GetComponent<DictonaryEntity>().AddEntity(objPath, objName, objToPlace.transform.position, objToPlace.transform.rotation);
					} else {
						// Already stored
						objToPlace.GetComponent<DictonaryEntity>().AddEntity(objEntity.ID, objToPlace.transform.position, objToPlace.transform.rotation);
					}
					
					switchMode();
				} 
			}

			// TODO add code to remove object
		}

		/// <summary>
		///	Set the information about the object to place
		/// <para name="obj">The object to place</para>
		/// <para name="name">The name of object to place</para>
		/// </summary>
		public void setObject(GameObject obj, string name, string path) {
			// Called by the VRChooseObject script to pass the data and activate this script

			objToPlace = obj;
			objName = name;
			objPath = path;
			modifyObjScript = objToPlace.GetComponent<ModifyObject>();
			modifyObjScript.enabled = true;
		}


		/// <summary>
		///	Trigger the choosing phase and stop this one 
		/// </summary>
		private void switchMode(){
			// Go back to choose mode

			objToPlace.GetComponent<Interactible>().AddSelectionEvent();
			chooseScript.enabled = true;
			modifyObjScript.enabled = false;
			this.enabled = false;
		}
	}
}