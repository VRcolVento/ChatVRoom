using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoAV.Editor.User;

namespace DemoAV.Editor.ObjectUtil {

	/// <summary>
	/// Class to handle the interaction with an object.
	/// This class pushes the object out the walls after placement
	/// </summary>
	public class Interactible : MonoBehaviour {

		// Store the walls I am colliding with
		private HashSet<GameObject> wallCollisionList = new HashSet<GameObject>();

		// Materials
//		public Material selectedMaterial;
//		public Material defaultMaterial;
//		public Material DefaultMaterial {
//			get { return defaultMaterial; }
//		}
		
		void Update () {
			
			if(wallCollisionList.Count > 0){
				// Push me outside the wall
				
				foreach(GameObject obj in wallCollisionList){

//					if(obj.GetComponent<Renderer>().bounds.Intersects(this.GetComponent<Renderer>().bounds))
							this.transform.position += Vector3.Scale(Vector3.one * Time.deltaTime, obj.transform.up);
				}
			}	
		}


		void OnCollisionEnter(Collision collision){

			if(collision.gameObject.layer == LayerMask.NameToLayer("RoomLayer") && collision.gameObject.tag != "Floor"){
				
				wallCollisionList.Add(collision.gameObject);
//				if(collision.gameObject.GetComponent<Renderer>().bounds.Intersects(this.GetComponent<Renderer>().bounds))
			}		
		}


		void OnCollisionExit(Collision collision) {

			if(collision.gameObject.layer == LayerMask.NameToLayer("RoomLayer")){
					
				wallCollisionList.Remove(collision.gameObject);
			}		
		}

		// Event's delegates
		void selectColor(GameObject obj){
			// Am I the selected object?
//			if(obj != null && GameObject.ReferenceEquals(obj, this.gameObject))
//				GetComponent<MeshRenderer>().material = selectedMaterial;
		}

		void defaultColor() {
//			GetComponent<MeshRenderer>().material = defaultMaterial;
		}

/* 
		// Subscribe / Unsubscribe delegates
		public void AddSelectionEvent(){
			VRChooseObject.select += selectColor;
			VRChooseObject.deselect += defaultColor;
		}

		public void RemoveSelectionEvent() {
			VRChooseObject.select -= selectColor;
			VRChooseObject.deselect -= defaultColor;
		}

		public void OnDestroy() {

			RemoveSelectionEvent();
		}
*/
	}
}