﻿using System.Collections;
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
	// The joint force when the object is grabbed.
	GameObject toucher = null;

	public void Start(){
		GameObject.Find("RightController").GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, TriggerEvent);
	}

	/// <summary>
	/// 	Removes the item from the menu and put it in another canvas.
	/// </summary>
	/// <param name="canvasPrefab"> The canavas prefab in which put the item. 
	/// 							The item will be append to an instance of this prefab.</param>
	public void RemoveFromMenu(){
		// Create new canvas and put the text as its son.
		canvas = Instantiate(canvasPrefab, transform.position, transform.rotation);
		canvas.AddComponent<NotificationMenuMiniCanvas>();

		transform.SetParent(canvas.transform.Find("Layout"));
		gameObject.name = "Notification title";

		// Remove collider.
		Destroy(GetComponent<Collider>());

		// Add notification description.
		descriptionObj = new GameObject("Notification description");
		Text text = descriptionObj.AddComponent<Text>();
		descriptionObj.transform.SetParent(transform.parent);
		descriptionObj.transform.localScale = new Vector3(1, 1, 1);
		descriptionObj.transform.localPosition = new Vector3(1, 1, 0);
		descriptionObj.transform.localRotation = Quaternion.identity;
		text.text = descriptionText;
		text.color = Color.black;
		text.fontSize = 26;
		text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		descriptionObj.SetActive(false);
	}

	/// <summary>
	/// 	The callback to call when right controller is trying to grab notification.
	/// </summary>
	/// <param name="hit"></param>
	void TriggerEvent(RaycastHit hit){
		// If the object has been touched and the mouse has been pressed, delete it from menu.
		if(enabled && toucher){
			if(transform.parent.gameObject.name != "Layout")
				RemoveFromMenu();

			canvas.GetComponent<NotificationMenuMiniCanvas>().Grab(toucher);
			enabled = false;
		}
	}

	void OnTriggerEnter(Collider collider){
		if(collider.gameObject.GetComponent<FixedJoint>() == null)
			toucher = collider.gameObject;
	}

	void OnTriggerExit(){
		toucher = null;
	}

	private void OnDisable() {
		GameObject.Find("RightController").GetComponent<VRKeyHandler>().RemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, TriggerEvent);
	}
}
}