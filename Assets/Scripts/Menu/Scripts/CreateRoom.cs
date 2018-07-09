using UnityEngine;
using UnityEngine.UI;

namespace DemoAV.StartMenu{
public class CreateRoom : MonoBehaviour {

	/// <summary>
	/// 	Creates and opens a new room.
	/// </summary>
	public void CreateNewRoom(){
		PrefabDictonary dictonary = GameObject.Find("Dictonary").GetComponent<PrefabDictonary>();
		dictonary.name = transform.Find("Panel/Room Name").GetComponent<Text>().text;
	}
}
}
