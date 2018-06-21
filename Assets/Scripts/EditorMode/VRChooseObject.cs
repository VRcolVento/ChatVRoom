using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoAV.Editor.User{

	public class VRChooseObject : MonoBehaviour {

		private SteamVR_TrackedObject trackedObj;
		private SteamVR_Controller.Device Controller {
			get { return SteamVR_Controller.Input((int)trackedObj.index); }
		}

		UpdateLineRenderer lineRenderer;
		string prefabName = "";
		GameObject objToPlace = null;
		VRPlaceObject placingScript;

		// Mask
		int furnitureMask;
		
		void Awake() {
			trackedObj = GetComponent<SteamVR_TrackedObject>();
			placingScript = GetComponent<VRPlaceObject>();		
		}

		// Use this for initialization
		void Start () {
			furnitureMask = LayerMask.GetMask("FurnitureLayer");
			lineRenderer = GetComponent<UpdateLineRenderer>();
//			lineRenderer.enabled = false;
		}
		
		// Update is called once per frame
		void Update () {
			
			if (chooseFurniture(out objToPlace)) {

				placingScript.setObject(objToPlace, prefabName);
				objToPlace.GetComponent<Interactible>().RemoveSelectionEvent();

				// Update status: switch working scripts
				this.enabled = false;
				placingScript.enabled = true;
			}
		}




		void OnEnable(){
			objToPlace = null;
		}		






		bool chooseFurniture(out GameObject newObject){

			/* 
			if(Input.GetKeyDown ("1")){
				newObject = loadResource("Tavolo");
				return true;
			}

			newObject = null;
			return false;
			*/

			if(Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad)){
				// TODO scegliere oggetto tramite menu a bottoni?
				newObject = loadResource("Tavolo");
				return true;
			}

			newObject = null;
			return false;
		}


		private GameObject loadResource(string res) {

			prefabName = res;
			return Instantiate(Resources.Load("EditorPrefabs/" + res, typeof(GameObject)),
					new Vector3(0, 5, 0), Quaternion.identity) as GameObject;
		}	
	}


}
