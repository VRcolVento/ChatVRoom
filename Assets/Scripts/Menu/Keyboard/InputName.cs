using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DemoAV.StartMenu.Keyboard{
public class InputName : MonoBehaviour {

	string _roomName;
	Text _roomNameText;
	// How many frames have to pass between ticks.
	public byte tickSpeed;
	byte currFrame;
	bool hasUnderscore = false;

	//
	public string roomName{
		get{ return _roomName; }
	}

	// Use this for initialization
	void Start () {
		_roomNameText = GetComponent<Text>();
		_roomName = "";
		currFrame = 0;
		_roomNameText.text = _roomName;
	}
	
	// Update is called once per frame
	void Update () {
		// Add final underscore.
		if(++currFrame >= tickSpeed){
			char lastChar = _roomName.Length > 0 ? _roomName[_roomName.Length - 1] : '\0';
			_roomNameText.text = _roomName;

			if (!hasUnderscore)
				_roomNameText.text += "_";

			hasUnderscore ^= true;
			currFrame = 0;
		}
	}

	/// <summary>
	/// 	Add a character to the room name.
	/// </summary>
	/// <param name="ch"> The caracter to add. </param>
	public void AddCharacter(char ch){
		_roomName += ch;

		// Update room name.
		_roomNameText.text = _roomName;
		if (hasUnderscore)	_roomNameText.text += "_";
	}

	/// <summary>
	/// 	Remove last character from the room name.
	/// </summary>
	public void RemoveLastCharacter(){
		_roomName = _roomName.Remove(_roomName.Length - 1);

		// Update room name.
		_roomNameText.text = _roomName;
		if (hasUnderscore)	_roomNameText.text += "_";
	}

	/// <summary>
	/// 	Press the current selected key if one.
	/// </summary>
	public void PressKey(){
		if (EventSystem.current.currentSelectedGameObject){
			AlphaNumericSymbol symbol = EventSystem.current.currentSelectedGameObject.GetComponent<AlphaNumericSymbol>();

			if (symbol != null){
				AddCharacter(symbol.GetSymbol());
			}
			else{
				if (EventSystem.current.currentSelectedGameObject.GetComponent<AlphaNumericSymbol>())
					RemoveLastCharacter();
			}
		}
		
	}
}
}