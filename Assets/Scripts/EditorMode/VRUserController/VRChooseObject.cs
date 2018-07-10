using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoAV.Editor.ObjectUtil;
using UnityEngine.UI;
using TMPro;
using DemoAV.Editor.MenuUtil;
using DemoAV.Editor.SceneControl;

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
		SelectMenuItem selectingScript;

		// Masks
		int furnitureMask;
		private int menuMask;

		// Help Text
		GameObject dismissButton;
		GameObject helpPanel;

		void Awake() {
			trackedObj = GetComponent<SteamVR_TrackedObject>();
			placingScript = GetComponent<VRPlaceObject>();		
			selectingScript = GetComponent<SelectMenuItem>();
		}

		void Start () {
			furnitureMask = LayerMask.GetMask("FurnitureLayer");
			menuMask = LayerMask.GetMask("Menu Layer");
			dismissButton = GameObject.Find("HelpButton");
			helpPanel = GameObject.Find("HelpPanel");
		}
		
		void Update () {

			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit, 1000f, menuMask)) {

				// Check if the user is pointing the help button
				if(hit.transform.gameObject.tag == "Dismiss") {
					if(Controller.GetHairTriggerDown()) {
						
						if(helpPanel.activeInHierarchy) {

							helpPanel.SetActive(false);
							dismissButton.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Show help";
						}
						else {
							helpPanel.SetActive(true);
							dismissButton.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Dismiss";
						}
					}

				}
			}
			if(Physics.Raycast(ray, out hit, 1000f, furnitureMask)) {
				// Check if the user is aiming an already placed object

				GameObject obj = hit.transform.gameObject;

				if(deselect != null) deselect(); // Call Deselect event: otherwise if objects overlap and they all stay blue
				if(select != null) select(obj); // Call Select event

				if(Controller.GetHairTriggerDown()) {
					// Activate the modification phase for the object
					switchMode(obj, obj.name, "FIXME");
				}
			}
			else {
				if(deselect != null) deselect(); // Call Deselect event
			}
		}

		void OnEnable() {
			// When active, add listener for menu buttons
			SelectMenuItem.menuPress += buttonPressed;
			selectingScript.enabled = true;
			helpPanel.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Left trigger: Open menu to choose an object\n";
		}		

		void OnDisable() {
			// When not active, remove listener for menu buttons
			SelectMenuItem.menuPress -= buttonPressed;
			selectingScript.enabled = false;
		}


		/// <summary>
		/// Listener for menu button pressed: load the selected prefab and switch to placing phase, save or exit
		/// <para name="menuBtn">The pressed menu button</para>
		/// </summary>
		void buttonPressed(GameObject menuBtn) {

			string text = menuBtn.transform.GetChild(0).GetComponent<TextMeshPro>().text;

			switch (text)
			{
				case "Save":
					SceneController.Dictionary.Save();
					break;
				case "Exit":
//					SceneController.Dictionary.Load();
					Debug.Log("Esco");
					break;
				default:
					string objectPath = menuBtn.GetComponent<ButtonPathInfo>().MyPath;
					GameObject objToPlace = loadResource(objectPath + "/" + text);
					switchMode(objToPlace, text, objectPath);
					break;
			}
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
		private void switchMode(GameObject obj, string name, string path) {

			// Tell to the placing script the object to modify
			placingScript.setObject(obj, name, path);
			// Remove selection events for the object during the placing phase
			obj.GetComponent<Interactible>().RemoveSelectionEvent();

			// Update status: switch working scripts
			this.enabled = false;
			placingScript.enabled = true;
		}
	}
}
