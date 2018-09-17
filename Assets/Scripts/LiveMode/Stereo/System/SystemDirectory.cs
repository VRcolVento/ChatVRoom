using UnityEngine;

using DemoAV.Common;

namespace DemoAV.Live.Stereo.System{

public class SystemDirectory : MonoBehaviour {
	public GameObject floatingDir;
	public string directoryPath;
	GameObject currentController;

	/// <summary>
	/// 	OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other"> The other Collider involved in this collision. </param>
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Controller"){
			currentController = other.gameObject;
			currentController.GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.GRIP, CreateFloatingDirectory);
		}
	}

	/// <summary>
	/// 	OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Controller"){
			other.gameObject.GetComponent<VRKeyHandler>().RemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.GRIP, CreateFloatingDirectory);
		}
	}

	/// <summary>
	/// 	Creates a new floating directory.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void CreateFloatingDirectory(RaycastHit hit){
		Transform tr = currentController.GetComponent<Transform>();
		GameObject newDir = Instantiate(floatingDir, tr.position, tr.rotation);
		GameObject button = Instantiate(gameObject);

		// Create new floating directory and bind it to the controller.
		newDir.GetComponent<FloatingDirectory>().directoryPath = directoryPath;
		button.transform.SetParent(newDir.transform.Find("Panel"));
		button.transform.localPosition = Vector3.zero;
		button.transform.localRotation = Quaternion.identity;
		button.transform.localScale = new Vector3(1, 1, 1);

		FixedJoint fx = newDir.AddComponent<FixedJoint>();
		fx.breakForce = 20000;
    	fx.breakTorque = 20000;
		fx.connectedBody = currentController.GetComponent<Rigidbody>();

		// Add callback.
	}

	/// <summary>
	/// 	This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	void OnDestroy() {
		if (currentController)
			currentController.GetComponent<VRKeyHandler>().RemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.GRIP, CreateFloatingDirectory);
	}
}
}