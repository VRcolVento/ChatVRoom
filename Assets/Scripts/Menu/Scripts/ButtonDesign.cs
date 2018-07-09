using UnityEngine;
using UnityEngine.UI;

namespace DemoAV.StartMenu{
[ExecuteInEditMode]
public class ButtonDesign : MonoBehaviour {

	public Color textColor;
	public ColorBlock colors;
	public Vector2 buttonSize;

	void OnValidate(){
		foreach (Transform child in transform){
			foreach(Transform grandchild in child){
				Button button = grandchild.GetComponent<Button>();

				if(button != null){
					button.colors = colors;
					grandchild.transform.Find("Text").GetComponent<Text>().color = textColor;
					grandchild.GetComponent<RectTransform>().sizeDelta = buttonSize;
				}
			}
		}
	}
}
}
