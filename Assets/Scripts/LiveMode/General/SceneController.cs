using UnityEngine;
using UnityEngine.SceneManagement;

using DemoAV.Editor.StorageUtility;

namespace DemoAV.Live{

public class SceneController : MonoBehaviour {

	void Awake() {
		// Load all the furnitures.
		GameObject.Find("GlobalDictionary").GetComponent<PrefabDictionary>().Load();
	}

	public void ExitLiveScene(){
		GameObject.Find("GlobalDictionary").GetComponent<PrefabDictionary>().Clear();
		SceneManager.LoadScene("menuScene");
	}
}

}