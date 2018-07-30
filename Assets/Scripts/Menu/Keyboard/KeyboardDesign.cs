using UnityEngine;
using UnityEngine.UI;

namespace DemoAV.StartMenu.Keyboard{
[ExecuteInEditMode]
public class KeyboardDesign : MonoBehaviour {
	public Color textColor;
	public ColorBlock colors;

	void OnValidate(){
		foreach (Transform child in transform){
			foreach(Transform button in child){
				button.GetComponent<Button>().colors = colors;
				button.transform.Find("Text").GetComponent<Text>().color = textColor;
			}
		}
	}
}
}
