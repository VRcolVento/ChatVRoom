using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoAV.Live.Stereo.System{

public class FloatingDirectory : MonoBehaviour {
	GameObject songsList = null;
	public string directoryPath;

	/// <summary>
	/// 	Start is called on the frame when a script is enabled just before
	/// 	any of the Update methods is called the first time.
	/// </summary>
	void Start() {
		
	}

	/// <summary>
	/// 	Changes the directory in which the list of songs is taken.
	/// </summary>
	void ChangeDirectory(){
		if (songsList != null)
			songsList.GetComponent<SongsList>().ChangeCurrentDir(directoryPath);
	}

	/// <summary>
	/// 	OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other"> The other Collider involved in this collision. </param>
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Stereo")
			songsList = other.gameObject;
	}

	/// <summary>
	/// 	OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerExit(Collider other) {
		if (songsList != null && other.gameObject.tag == "Stereo")
			songsList = null;
	}
}

}