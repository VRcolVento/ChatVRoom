using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DemoAV.Editor.MenuUtil {


	/// <summary>
	/// Class to handle user interaction with the menu.
	/// This class is intended to use in VR environment.
	/// </summary>
	public class SelectMenuItem : MonoBehaviour {

		public delegate void SelectAction(GameObject obj);
		public delegate void DeselectAction();
		public static event SelectAction menuSelect;
		public static event DeselectAction menuDeselect;
		public static event SelectAction menuPress;

		private SteamVR_TrackedObject trackedObj;

		GameObject helpCanvas;


		private SteamVR_Controller.Device Controller {
			get { return SteamVR_Controller.Input((int)trackedObj.index); }
		}

		private int menuMask;

		void Start () {
			menuMask = LayerMask.GetMask("Menu Layer");
			trackedObj = GetComponent<SteamVR_TrackedObject>();
			helpCanvas = GameObject.Find("HelpPanel");
		}

		void Update () {

			Ray ray = new Ray(transform.position, transform.forward);

			RaycastHit hit;

			if(Physics.Raycast(ray, out hit, Mathf.Infinity, menuMask)) {

				if(hit.transform.gameObject.tag == "Dismiss") {
					if(Controller.GetHairTriggerDown()) {

						if(helpCanvas.activeInHierarchy){
							hit.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Show Help";
							helpCanvas.SetActive(false);
						}
						else {
							hit.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Dismiss";
							helpCanvas.SetActive(true);
						}
					}
				}
				GameObject obj = hit.transform.gameObject;

				if(menuDeselect != null) menuDeselect(); // Call Deselect event: otherwise if objects overlap they all stay blue
				if(menuSelect != null) menuSelect(obj); // Call Select event

				if(Controller.GetHairTriggerDown()) {
					if(menuPress != null) menuPress(obj);
				}
			}
			else {
				if(menuDeselect != null) menuDeselect(); // Call Deselect event: otherwise if objects overlap they all stay blue
			}
		}
	}
}
