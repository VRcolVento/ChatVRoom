using UnityEngine;
using UnityEngine.UI;

namespace DemoAV.StartMenu{
[ExecuteInEditMode]
public class ButtonDesign : MonoBehaviour {
	// Button;
	public Sprite buttonBackground;
	public ColorBlock colors;
	public Vector2 buttonSize;
	// Text.
	public Color textColor;
	public Font textFont;
	public int fontSize;

	void OnValidate(){
		foreach (Transform child in transform){
			foreach(Transform grandchild in child){
				Button button = grandchild.GetComponent<Button>();

				if(button != null){
					button.GetComponent<Image>().sprite = buttonBackground;
					button.colors = colors;
					grandchild.GetComponent<RectTransform>().sizeDelta = buttonSize;
					grandchild.GetComponent<BoxCollider>().size = new Vector3(buttonSize.x, buttonSize.y, 0.0003f);

					Text text = grandchild.transform.Find("Text").GetComponent<Text>();
					text.color = textColor;
					text.font = textFont;
					text.fontSize = fontSize;
				}
			}
		}
	}
}
}
