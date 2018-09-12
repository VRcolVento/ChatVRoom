using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DemoAV.Common;

namespace DemoAV.Live.Notification{
public class NotificationMenuItem : MonoBehaviour {
	public GameObject canvasPrefab;
	GameObject canvas;
	// The notification description.
	GameObject descriptionObj;
	public string descriptionText;
	public Color descriptionColor = new Color(0, 0, 0, 255);

	/// <summary>
	/// 	Removes the item from the menu and put it in another canvas.
	/// </summary>
	/// <param name="canvasPrefab"> The canavas prefab in which put the item. 
	/// 							The item will be append to an instance of this prefab.</param>
	public void RemoveFromMenu(){
		// Create new canvas.
		canvas = Instantiate(canvasPrefab, transform.position, transform.rotation);
		canvas.AddComponent<NotificationMenuMiniCanvas>();

		// Create text.
		Text title = transform.Find("Text").GetComponent<Text>();
		Text titleText = CreateText("Notification title", title.text, title.color, title.fontSize).GetComponent<Text>();
		titleText.alignment = TextAnchor.UpperCenter;
		titleText.fontStyle = FontStyle.Bold;

		// Remove collider.
		Destroy(GetComponent<Collider>());

		// Add notification description.
		descriptionObj = CreateText("Notification description", descriptionText, Color.black, 26);
		descriptionObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 430);
		descriptionObj.GetComponent<Text>().fontSize = 20;
		descriptionObj.SetActive(false);

		// Automatically grab it.
		canvas.GetComponent<NotificationMenuMiniCanvas>().Release();

		// Destroy this button.
		Destroy(gameObject);
	}

	GameObject CreateText(string name, string content, Color color, int fontSize){
		GameObject newObj = new GameObject(name);
		Text text = newObj.AddComponent<Text>();
		newObj.transform.SetParent(canvas.transform.Find("Layout"));
		newObj.transform.localScale = new Vector3(1, 1, 1);
		newObj.transform.localPosition = new Vector3(1, 1, 0);
		newObj.transform.localRotation = Quaternion.identity;
		text.text = content;
		text.color = color;
		text.fontSize = fontSize;
		newObj.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");

		return newObj;
	}

	/// <summary>
	/// 	The callback to call to remove this item from menu.
	/// </summary>
	/// <param name="hit"></param>
	public void TriggerEvent(){
		// If the trigger has been pressed, delete it from menu.
		if(transform.parent.gameObject.name != "Layout")
			RemoveFromMenu();
		enabled = false;
	}
}
}