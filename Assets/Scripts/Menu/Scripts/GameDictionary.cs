using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoAV.Editor.StorageUtility;

public class GameDictionary : MonoBehaviour {

	// List of rooms
	//private Dictionary<string, PrefabDictionary> dictionary;
	private List<string> roomList;

	private string currentRoom;

	// Current room, to read once in editor/live
	public string CurrentRoom {
		get { return currentRoom; }
		set { currentRoom = value; }
	}

	void Awake() {
		
		DontDestroyOnLoad(this.gameObject);
		//dictionary = new Dictionary<string, PrefabDictionary>();
		roomList = new List<string>();

		// Get the already existing rooms
		string [] fileEntries = Directory.GetFiles(Application.persistentDataPath);

        foreach(string file in fileEntries) {
			string fileName = file.Substring(file.LastIndexOf("\\") + 1);

			if (fileName.Substring(0, 5) == "Room_") {
				string roomName = fileName.Remove(fileName.LastIndexOf(".")).Remove(0, 5);
				roomList.Add(roomName);
			}
		}
	}
	
	// Create a new room and instantiate its own PrefabDictionary
	public void CreateRoom(string name) {

		if(!roomList.Contains(name)) {
//		if(dictionary[name] == null) {

			roomList.Add(name);
			Debug.Log("Room " + name + " created!");
		}
		else  {
			Debug.Log("Room already exists");
		}
	}

}
