using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DemoAV.Common;

namespace DemoAV.Live.Stereo.System{

public class FloatingDirectory : MonoBehaviour {
	public GameObject songsList;
	public string directoryPath;
	bool isInStereo;

	/// <summary>
	/// 	Start is called on the frame when a script is enabled just before
	/// 	any of the Update methods is called the first time.
	/// </summary>
	void Start() {
		songsList = GameObject.Find("Songs List");
		VRKeyHandler handler = GetComponent<FixedJoint>().connectedBody.gameObject.GetComponent<VRKeyHandler>();
		handler.AddCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.GRIP, ChangeDirectory);
	}
	
	/// <summary>
	/// 	Changes the directory in which the list of songs is taken.
	/// 	Then the floatingDir is destroyed.
	/// </summary>
	/// <param name="hit"></param>
	void ChangeDirectory(RaycastHit hit){
		if (isInStereo)
			songsList.GetComponent<SongsList>().ChangeCurrentDir(directoryPath);

		VRKeyHandler handler = GetComponent<FixedJoint>().connectedBody.gameObject.GetComponent<VRKeyHandler>();
		handler.DeferredRemoveCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.GRIP, ChangeDirectory);

		Destroy(gameObject);
	}

	/// <summary>
	/// 	OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other"> The other Collider involved in this collision. </param>
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Stereo Layer"))
			isInStereo = true;
	}

	/// <summary>
	/// 	OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerExit(Collider other) {
		if (isInStereo && other.gameObject.layer == LayerMask.NameToLayer("Stereo Layer"))
			isInStereo = false;
	}
}

}