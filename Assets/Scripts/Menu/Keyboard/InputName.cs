using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DemoAV.StartMenu.Keyboard{
public class InputName : MonoBehaviour {

	string roomName;
	Text roomNameText;
	// How many frames have to pass between ticks.
	public byte tickSpeed;
	byte currFrame;

	// Use this for initialization
	void Start () {
		roomNameText = GetComponent<Text>();
		roomName = "";
		currFrame = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(++currFrame >= tickSpeed){
			char lastChar = roomName.Length > 0 ? roomName[roomName.Length - 1] : '\0';

			if(lastChar == '_')
				roomName = roomName.Remove(roomName.Length - 1);
			else
				roomName += "_";

			roomNameText.text = roomName;
			currFrame = 0;
		}
	}

	/// <summary>
	/// 	Add a character to the room name.
	/// </summary>
	/// <param name="ch"> The caracter to add. </param>
	public void AddCharacter(char ch){
		char lastChar = roomName.Length > 0 ? roomName[roomName.Length - 1] : '\0';
		bool hasUnderscore = lastChar == '_';

		if (hasUnderscore)	roomName = roomName.Remove(roomName.Length - 1);
		roomName += ch;
		if (hasUnderscore)	roomName += '_';

		roomNameText.text = roomName;
	}

	/// <summary>
	/// 	Remove last character from the room name.
	/// </summary>
	public void RemoveLastCharacter(){
		char lastChar = roomName.Length > 0 ? roomName[roomName.Length - 1] : '\0';
		bool hasUnderscore = lastChar == '_';

		if (hasUnderscore)			roomName = roomName.Remove(roomName.Length - 1);
		if (roomName.Length > 0)	roomName = roomName.Remove(roomName.Length - 1);
		if (hasUnderscore)			roomName += '_';

		roomNameText.text = roomName;
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