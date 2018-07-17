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

		// Events
		public static event SelectAction menuSelect;
		public static event DeselectAction menuDeselect;
		public static event SelectAction menuPress;

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


		// Reference to the placing script, to activate after the user has chosen the object
		VRPlaceObject placingScript;

		// Masks
		int raycastMask;

		// Help
		GameObject helpPanel;
		GameObject confirmationPanel;

		// For menu
		public OpenFurnitureMenuList menu;

		void Awake() {
			trackedObj = GetComponent<SteamVR_TrackedObject>();
			placingScript = GetComponent<VRPlaceObject>();		
			helpPanel = GameObject.Find("HelpCanvas");
			confirmationPanel = GameObject.Find("SaveConfirmationCanvas");
			confirmationPanel.SetActive(false);
		}

		void Start () {
			raycastMask = LayerMask.GetMask("FurnitureLayer", "Menu Layer");
		}
		
		void Update () {

			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit, 1000f, raycastMask)) {
				// Check if the user is aiming an already placed object

				GameObject obj = hit.transform.gameObject;

				// Hit an object
				if(obj.layer == 9 && Controller.GetHairTriggerDown()) {
					// Activate the modification phase for the object
					switchMode(obj, obj.name, "FIXME");
				}

				// Hit a menu button
				else if(obj.layer == 11) {

					if(menuDeselect != null) menuDeselect(); // Call Deselect event: otherwise if objects overlap they all stay blue
					if(menuSelect != null) menuSelect(obj); // Call Select event

					if(Controller.GetHairTriggerDown())
						if(menuPress != null) menuPress(obj);
				}
			}
			else
				if(menuDeselect != null) menuDeselect(); // Call Deselect event
		}

		void OnEnable() {
			// When active, add listener for menu buttons
			VRChooseObject.menuPress += buttonPressed;
		}		

		void OnDisable() {
			// When not active, remove listener for menu buttons
			VRChooseObject.menuPress -= buttonPressed;
		}


		/// <summary>
		/// Listener for menu button pressed: load the selected prefab and switch to placing phase, save or exit
		/// <para name="menuBtn">The pressed menu button</para>
		/// </summary>
		void buttonPressed(GameObject menuBtn) {

			string tag = menuBtn.tag;
			string text = menuBtn.transform.GetChild(0).GetComponent<TextMeshPro>().text;

			switch (tag)
			{
				case "Save":
					SceneController.Dictionary.Save();
					confirmationPanel.transform.position = menu.transform.position;
					StartCoroutine("showSaveNotification");
					break;
				case "Exit":
					Debug.Log("Esco");
					break;
				case "MenuObject":
					menu.OpenSubMenu(menuBtn.name);
					break;
				case "MenuFurniture":
					string objectPath = menuBtn.GetComponent<ButtonPathInfo>().MyPath;
					GameObject objToPlace = loadResource(objectPath + "/" + text);
					objToPlace.transform.rotation = Quaternion.Euler(-90, 0, 0);
					switchMode(objToPlace, text, objectPath);
					break;
				case "MenuBack":
					menu.OpenMenu();
					break;
				case "Dismiss":
					if(helpPanel.activeInHierarchy){
						menuBtn.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Show Help";
						helpPanel.SetActive(false);
					}
					else {
						menuBtn.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Dismiss";
						helpPanel.SetActive(true);
					}
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
//			obj.GetComponent<Interactible>().RemoveSelectionEvent();

			// Update status: switch working scripts
			this.enabled = false;
			placingScript.enabled = true;
		}

		private IEnumerator showSaveNotification() {

			confirmationPanel.SetActive(true);
			yield return new WaitForSeconds(2);
			confirmationPanel.SetActive(false);

			yield return null;
		}
	}
}
