using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EditRoom : MonoBehaviour {
	public GameObject roomItem;

	// Use this for initialization
	void Awake () {
		// Process the list of files found in the directory.
		Transform content = transform.Find("Scroll View/Viewport/Content");
        string [] fileEntries = Directory.GetFiles(Application.persistentDataPath);
        foreach(string file in fileEntries){
			string fileName = file.Substring(file.LastIndexOf("\\") + 1);

			if (fileName.Substring(0, 5) == "Room_"){
				string roomName = fileName.Remove(fileName.LastIndexOf(".")).Remove(0, 5);

				GameObject newRoom = Instantiate(roomItem);
				newRoom.transform.SetParent(content);
				newRoom.transform.localScale = new Vector3(1, 1, 1);
				newRoom.transform.localPosition = Vector3.zero;
				newRoom.transform.localRotation = Quaternion.identity;
				
				newRoom.transform.Find("Text").GetComponent<Text>().text = roomName;
				newRoom.GetComponent<Button>().onClick.AddListener(delegate {StartRoom(fileName); });
			}
		}
	}

	void StartRoom(string fileName){
		Debug.Log(fileName);
	}
}
