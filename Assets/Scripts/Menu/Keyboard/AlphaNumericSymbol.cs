using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DemoAV.StartMenu.Keyboard{
public class AlphaNumericSymbol : MonoBehaviour {
	Button button;
	Color normalColor, highlightedColor;
	ColorBlock colors;
	public InputName input;

	// Use this for initialization
	void Start () {
		button = GetComponent<Button>();

		// Delay the collider re-dimensioning to allow the layout script to set the correct size of button
		// after a frame.
		Invoke("SetCollider", 0.2f);

		colors = button.colors;
		normalColor = button.colors.normalColor;
		highlightedColor = button.colors.highlightedColor;
		GetComponent<Button>().onClick.AddListener(() => {input.AddCharacter(GetSymbol());});
	}
	
	/// <summary>
	/// 	Sets the correct collider size.
	/// </summary>
	void SetCollider(){
		BoxCollider collider = gameObject.AddComponent<BoxCollider>();
		collider.size = GetComponent<RectTransform>().sizeDelta;
		collider.isTrigger = true;
	}

	/// <summary>
	/// 	Returns the character of the key.
	/// </summary>
	/// <returns> The character represented by the key. </returns>
	public char GetSymbol(){
		return transform.Find("Text").GetComponent<Text>().text != "SPACE" ? transform.Find("Text").GetComponent<Text>().text[0] : ' ';
	}

	// Called on entering collision with pad.
	void OnTriggerEnter(Collider collider) {
		colors.normalColor = highlightedColor;
		button.colors = colors;
		input.AddCharacter(GetSymbol());
	}

	// Called on exiting collision with pad.
	void OnTriggerExit(Collider collider) {
		colors.normalColor = normalColor;
		button.colors = colors;
	}
}
}