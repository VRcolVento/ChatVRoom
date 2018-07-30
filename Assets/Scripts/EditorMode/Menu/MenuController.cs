using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoAV.Editor.MenuUtil {

	/// <summary>
	/// Class to detect menu open/close actions.
	/// This class is intended to use in VR environment.
	/// </summary>
	public class MenuController : MonoBehaviour {

		private SteamVR_TrackedObject trackedObj;
		private SteamVR_Controller.Device Controller {
			get { return SteamVR_Controller.Input((int)trackedObj.index); }
		}

		public Canvas menuCanvas;
		public OpenFurnitureMenuList menuScript;

		void Start () {
			trackedObj = GetComponent<SteamVR_TrackedObject>();
		}

		void Update () {
			
			if(Controller.GetHairTriggerDown()) {

				if(!menuCanvas.gameObject.activeInHierarchy)
					menuScript.OpenMenu();
				else
					menuScript.CloseMenu();		
			}
		}
	}
}