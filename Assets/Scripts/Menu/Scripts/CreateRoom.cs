using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using DemoAV.StartMenu.Keyboard;
using DemoAV.Editor.StorageUtility;

namespace DemoAV.StartMenu{
public class CreateRoom : MonoBehaviour {
	public GameObject keyboard;
	public GameObject error1, error2;

	void OnEnable(){
		keyboard.SetActive(true);
	}

	/// <summary>
	/// 	Creates and opens a new room.
	/// </summary>
	public void CreateNewRoom(){
		string roomName = transform.Find("Panel/Room Name").GetComponent<InputName>().roomName.ToLower();

		// Checks if a room has a name and if a room with the same name already exists.
		if(roomName.Length == 0){
			error1.SetActive(true);
			error2.SetActive(false);
		}
		else if(File.Exists(Application.persistentDataPath + "/Room_" + roomName + ".dat")){
			error2.SetActive(true);
			error1.SetActive(false);
		} 
		else{	// Otherwise create it.
			GameObject.Find("GlobalDictionary").GetComponent<PrefabDictionary>().Name = roomName;
			SceneManager.LoadScene("editorScene");
		}
		
	}

	private void OnDisable() {
		keyboard.SetActive(false);
	}
}
}
