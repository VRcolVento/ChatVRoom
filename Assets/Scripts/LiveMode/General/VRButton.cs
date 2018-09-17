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
	bool isActive;

	void Awake() {
		button = GetComponent<Button>();
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
		}
	}

	/// <summary>
	/// 	Activates the callback bound to onclick event.
	/// </summary>
	/// <param name="hit"></param>
	void PressButton(RaycastHit hit) {
		button.onClick.Invoke();
	}
}

}