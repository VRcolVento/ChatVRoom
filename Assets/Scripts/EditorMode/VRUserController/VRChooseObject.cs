using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoAV.Editor.ObjectUtil;
using UnityEngine.UI;
using TMPro;

namespace DemoAV.Editor.User{

	/// <summary>
	/// Class to handle the object selection by the user.
	/// This class is intended to use in VR environment.
	/// </summary>
	public class VRChooseObject : MonoBehaviour {

		// The steam VR controller
		private SteamVR_TrackedObject trackedObj;
		private SteamVR_Controller.Device Controller {
			get { return SteamVR_Controller.Input((int)trackedObj.index); }
		}

		// Delegates

		/// <summary>
		/// Select an object
		/// <para name="obj">The object to selct</para>
		/// </summary>
		public delegate void SelectAction(GameObject obj);

		/// <summary>
		/// Deselect
		/// </summary>
		public delegate void DeselectAction();

		// Events
		public static event SelectAction select;
		public static event DeselectAction deselect;

		// Reference to the placing script, to activate after the user has chosen the object
		VRPlaceObject placingScript;

		// Masks
		int furnitureMask;
		private int menuMask;

//		UpdateLineRenderer lineRenderer;

		void Awake() {
			trackedObj = GetComponent<SteamVR_TrackedObject>();
			placingScript = GetComponent<VRPlaceObject>();		
		}

		void Start () {
			furnitureMask = LayerMask.GetMask("FurnitureLayer");
			menuMask = LayerMask.GetMask("Menu Layer");
//			lineRenderer = GetComponent<UpdateLineRenderer>();
		}
		
		void Update () {

//			Ray ray = new Ray(lineRenderer.GetPosition(), lineRenderer.GetForward());
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit, 1000f, furnitureMask)) {
				// Check if the user is aiming an already placed object

				GameObject obj = hit.transform.gameObject;

				if(deselect != null) deselect(); // Call Deselect event: otherwise if objects overlap and they all stay blue
				if(select != null) select(obj); // Call Select event

				if(Controller.GetHairTriggerDown()) {
					// Activate the modification phase for the object
					switchMode(obj, obj.name);
					Debug.Log("swithcin");
				}
			}
			else {
				if(deselect != null) deselect(); // Call Deselect event
			}


			// TODO Add action to fire UI
			// TODO UI to left controller

		}

		void OnEnable() {
			// When active, add listener for menu buttons
			SelectMenuItem.menuPress += chooseObjectFromEvent;
		}		

		void OnDisable() {
			// When not active, remove listener for menu buttons
			SelectMenuItem.menuPress -= chooseObjectFromEvent;
		}


		/// <summary>
		/// Listener for menu button pressed: load the selected prefab and switch to placing phase
		/// <para name="menuBtn">The pressed menu button</para>
		/// </summary>
		void chooseObjectFromEvent(GameObject menuBtn) {

			Debug.Log(menuBtn.name);
			string text = menuBtn.transform.GetChild(0).GetComponent<TextMeshPro>().text;
			GameObject objToPlace = loadResource(text);
			switchMode(objToPlace, text);
		}

		/// <summary>
		/// Helper to load a prefab from Resources folder
		/// <para name="res">The name of the prefab</para>
		/// </summary>
		private GameObject loadResource(string res) {

			return Instantiate(Resources.Load("EditorPrefabs/Furnitures/" + res, typeof(GameObject)),
					new Vector3(0, 5, 0), Quaternion.identity) as GameObject;
		}

		/// <summary>
		/// Helper to pass data to placing script
		/// <para name="obj">The object to place</para>
		/// <para name="name">The name of object to place</para>
		/// </summary>
		private void switchMode(GameObject obj, string name) {

			// Tell to the placing script the object to modify
			placingScript.setObject(obj, name); // TODO cambiare btnText.text con objToPlace.name??
			// Remove selection events for the object during the placing phase
			obj.GetComponent<Interactible>().RemoveSelectionEvent();

			// Update status: switch working scripts
			this.enabled = false;
			placingScript.enabled = true;	
		}
	}
}
