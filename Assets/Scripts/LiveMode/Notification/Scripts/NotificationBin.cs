using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DemoAV.Common;

namespace DemoAV.Live.Notification{

public class NotificationBin : MonoBehaviour {
	public VRKeyHandler controller;
	GameObject notification = null;

	/// <summary>
	/// 	This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable() {
		controller.AddCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.TRIGGER, DestroyNotification);
	}

	/// <summary>
	/// 	Destroy the colliding notification.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void DestroyNotification(RaycastHit hit){
		if (notification){
			Destroy(notification);
		}
	}
	
	/// <summary>
	/// 	Show the bin.
	/// </summary>
	/// <param name="position"> The position in which it appears. </param>
	public void Show(Vector3 position){
		transform.position = position;
		gameObject.SetActive(true);
	}

	/// <summary>
	/// 	Hide the bin.
	/// </summary>
	public void Hide(){
		gameObject.SetActive(false);
	}

	/// <summary>
	/// 	OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "FloatingNotification")
			notification = other.gameObject;
	}

	/// <summary>
	/// 	OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerExit(Collider other) {
		if(notification && other.gameObject.tag == "FloatingNotification")
			notification = null;
	}

	/// <summary>
	/// 	This function is called when the behaviour becomes disabled or inactive.
	/// </summary>
	void OnDisable() {
		controller.DeferredRemoveCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.TRIGGER, DestroyNotification);
	}
}

}