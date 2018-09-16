using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoAV.Common {

/// <summary>
/// 	Class that exposes some basic functionality of the controller
/// 	without directly access the SteamVR object.
/// </summary>
public class ControllerFunctions : MonoBehaviour {
	// Vibration.
	struct Vibration{
		public ushort duration, intensity;
	};
	Vibration vibration;
	// Collision
	GameObject collidingObject = null;

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

	void Update(){
		// Vibrate the controller.
		if(vibration.duration > 0){
			controller.TriggerHapticPulse(vibration.intensity);
			--vibration.duration;
		}
	}
	
	/// <summary>
	/// 	Makes the controller vibrate.
	/// </summary>
	/// <param name="cycle"> The number of updates it has to vibrate (90 = ~1sec)</param>
	/// <param name="intensity"> The intensity of the vibration. </param>
	public void Vibrate(ushort cycle, ushort intensity){
		vibration.duration = cycle;
		vibration.intensity = intensity;
	}

	/// <summary>
	/// 	Returns the current axis of the trackpad in controller.
	/// </summary>
	/// <returns> The current axis of trackpad. </returns>
	public Vector3 GetAxis(){
		return controller.GetAxis();
	}

	/// <summary>
	/// 	Returns the object that is currently colliding with the controller.
	/// 	Null is returned if there is no collision.
	/// </summary>
	/// <returns> The object with which the controller is eventually colliding. </returns>
	public GameObject GetCollidingObject(){
		return collidingObject;
	}

	/// <summary>
	/// 	OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerEnter(Collider other){
		collidingObject = other.gameObject;
	}

	/// <summary>
	/// 	OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerExit(Collider other){
		if(collidingObject && collidingObject.transform == other.transform)
			collidingObject = null;
	}
}
	
}
