using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoAV.Common {
public class ControllerFunctions : MonoBehaviour {
	// Get controller object.
	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	// Use this for initialization
	void Awake () {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	public void Vibrate(ushort microSecond){
		controller.TriggerHapticPulse(microSecond);
	}
}
	
}
