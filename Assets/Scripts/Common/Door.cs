using UnityEngine;
using UnityEngine.SceneManagement;

using DemoAV.Common;

namespace DemoAV.Common{

public class Door : MonoBehaviour {
	bool isColliding;
	public VRKeyHandler controller;

	/// <summary>
	/// 	Start is called on the frame when a script is enabled just before
	/// 	any of the Update methods is called the first time.
	/// </summary>
	void Start() {
		controller.AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, Exit);
	}

	/// <summary>
	/// 	Exits the level.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void Exit(RaycastHit hit){
		if (isColliding)	SceneManager.LoadScene("menuScene");
	}

	/// <summary>
	/// 	OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerEnter(Collider other) {
		if (other.gameObject == controller.gameObject){
			isColliding = true;
			controller.GetComponent<ControllerFunctions>().Vibrate(50, 5000);
		}
	}

	/// <summary>
	/// 	OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerExit(Collider other) {
		if (other.gameObject == controller.gameObject && isColliding)
			isColliding = false;
	}

	/// <summary>
	/// 	This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	void OnDestroy() {
		controller.RemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, Exit);
	}
}

}