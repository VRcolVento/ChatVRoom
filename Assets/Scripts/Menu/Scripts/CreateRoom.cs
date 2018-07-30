using UnityEngine;
using UnityEngine.UI;
using DemoAV.Editor.StorageUtility;

namespace DemoAV.StartMenu{
public class CreateRoom : MonoBehaviour {
	public GameObject keyboard;

	void OnEnable(){
		keyboard.SetActive(true);
	}

	/// <summary>
	/// 	Creates and opens a new room.
	/// </summary>
	public void CreateNewRoom(){
		PrefabDictionary dictonary = GameObject.Find("Dictionary").GetComponent<PrefabDictionary>();
		dictonary.name = transform.Find("Panel/Room Name").GetComponent<Text>().text;
	}

	private void OnDisable() {
		keyboard.SetActive(false);
	}
}
}
