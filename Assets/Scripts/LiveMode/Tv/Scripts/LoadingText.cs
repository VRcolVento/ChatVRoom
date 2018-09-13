using UnityEngine;

using TMPro;

public class LoadingText : MonoBehaviour {
	static byte FRAME_DELAY = 50;
	TextMeshPro text;
	byte frameCount;

	/// <summary>
	/// 	Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake() {
		text = GetComponent<TextMeshPro>();
	}

	/// <summary>
	/// 	This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable() {
		text.text = "Loading";
	}

	/// <summary>
	/// 	Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update() {
		if(++frameCount >= FRAME_DELAY){
			if(text.text != "Loading...")	text.text += ".";
			else							text.text = "Loading";
			frameCount = 0;
		}
	}
}
