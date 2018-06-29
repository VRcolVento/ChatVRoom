using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoAV.Editor.MenuUtil {
	public class MenuController : MonoBehaviour {

		private SteamVR_TrackedObject trackedObj;
		private SteamVR_Controller.Device Controller {
			get { return SteamVR_Controller.Input((int)trackedObj.index); }
		}

		public Canvas menuCanvas;
		public OpenFurnitureMenu menuScript;

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