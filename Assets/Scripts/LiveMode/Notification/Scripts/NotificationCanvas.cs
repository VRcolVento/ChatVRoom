using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using TMPro;
using DemoAV.Common;

namespace DemoAV.Live.Notification{

/// <summary>
/// 	The canvas for the on screen notification.
/// 	NOT USED IN VR.
/// </summary>
public class NotificationCanvas : MonoBehaviour {
	public GameObject notificationObj;
	NotificationManager manager;

	/// <summary>
	/// 	This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable () {
		manager = GameObject.Find("CVRREventSystem").GetComponent<NotificationManager>();
		manager.onAdd.AddListener(AddNotification);
	}
	
	// Update is called once per frame
	void Update () {
		Transform not = transform.Find("Notification1");
	}

	/// <summary>
	/// 	Add a new notification to the Canvas.
	/// </summary>
	/// <param name="text"> The text of the notification. </param>
	/// <param name="color"> The color of the notification. </param>
	public void AddNotification(NotificationManager.notification notification){
		// Shift the children.
		foreach (Transform child in transform)
			child.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, child.GetComponent<RectTransform>().rect.height);

		// Create new notification.
		GameObject newNot = Instantiate(notificationObj);
		newNot.transform.SetParent(transform);
		newNot.GetComponent<RectTransform>().anchoredPosition = new Vector2(newNot.GetComponent<RectTransform>().anchoredPosition.x, -20);
		newNot.name = "Notification" + transform.childCount;

		TextMeshProUGUI tmp = newNot.transform.Find("Text").GetComponent<TextMeshProUGUI>();
		tmp.text = notification.title;
		tmp.color = notification.titleColor;
	}

	void OnDisable(){
		manager.onAdd.RemoveListener(AddNotification);
	}
}
}
