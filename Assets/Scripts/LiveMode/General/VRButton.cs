using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using DemoAV.Common;

namespace DemoAV.Live{

/// <summary>
/// 	A class to allow VR controller to interact with UI button.
/// 	Remember to edit the BoxCollider to well fit the button.
/// </summary>	
[RequireComponent(typeof(BoxCollider))]
public class VRButton : MonoBehaviour {
	Button button;
	VRKeyHandler[] controllers = new VRKeyHandler[2];

	bool isActive;

	/// <summary>
	/// 	Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake() {
		button = GetComponent<Button>();
		controllers[0] = null;
		controllers[1] = null;
	}

	/// <summary>
	/// 	OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other"> The other Collider involved in this collision. </param>
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Controller"){
			button.Select();
			isActive = true;
			other.gameObject.GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, PressButton);
			controllers[other.gameObject.name.Contains("right") ? 0 : 1] = other.gameObject.GetComponent<VRKeyHandler>();
		}
	}

	/// <summary>
	/// 	OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerExit(Collider other) {
		if (isActive && other.gameObject.tag == "Controller"){
			EventSystem.current.SetSelectedGameObject(null);
			isActive = false;
			other.gameObject.GetComponent<VRKeyHandler>().RemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, PressButton);
			controllers[other.gameObject.name.Contains("right") ? 0 : 1] = null;
		}
	}

	/// <summary>
	/// 	Activates the callback bound to onclick event.
	/// </summary>
	/// <param name="hit"></param>
	void PressButton(RaycastHit hit) {
		button.onClick.Invoke();
	}

	/// <summary>
	/// This function is called when the behaviour becomes disabled or inactive.
	/// </summary>
	void OnDisable() {
		if (controllers[0] != null)		controllers[0].RemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, PressButton);
		if (controllers[1] != null)		controllers[1].RemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, PressButton);
	}
}

}