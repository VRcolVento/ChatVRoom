using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Instantiate the furnitures objsects
*/
public class Creator : MonoBehaviour {

	void Start(){
//		PrefabDictonary.Instance.Name = "SignoraStanza";
	}

	// Save the current room.
	public void SaveRoom(){
//		PrefabDictonary.Instance.Save();
		SceneController.Dictionary.Save();
	}

	// Load the last saved room.
	public void LoadRoom(){
//		PrefabDictonary.Instance.Load();
		SceneController.Dictionary.Load();
	}
}
