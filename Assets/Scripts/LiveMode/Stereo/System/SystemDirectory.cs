using UnityEngine;

using DemoAV.Common;

namespace DemoAV.Live.Stereo.System{

public class SystemDirectory : MonoBehaviour {
	public GameObject floatingDir;
	public string directoryPath;

	/// <summary>
	/// 	OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other"> The other Collider involved in this collision. </param>
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Controller")
			other.gameObject.GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.GRIP, CreateFloatingDirectory);
	}

	/// <summary>
	/// 	OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Controller")
			other.gameObject.GetComponent<VRKeyHandler>().RemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.GRIP, CreateFloatingDirectory);
	}

	/// <summary>
	/// 	Creates a new floating directory.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void CreateFloatingDirectory(RaycastHit hit){
		GameObject newDir = Instantiate(floatingDir);
		GameObject button = Instantiate(gameObject);

		// Create new floating directory and bind it to the controller.
		floatingDir.GetComponent<FloatingDirectory>().directoryPath = directoryPath;
		button.transform.SetParent(floatingDir.transform.Find("Panel"));
	}
}
}