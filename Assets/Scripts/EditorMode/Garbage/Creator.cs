using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Instantiate the furnitures objsects
*/

namespace DemoAV.Editor.SceneControl{

	public class Creator : MonoBehaviour {

		void Start(){
		}

		// Save the current room.
		public void SaveRoom(){
			SceneController.Dictionary.Save();
		}

		// Load the last saved room.
		public void LoadRoom(){
			SceneController.Dictionary.Load();
		}
	}
}
