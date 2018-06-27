using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DemoAV.Common;

namespace DemoAV.Live.Notification{
/// <summary>
/// 	The class that manages the little notification menu of the controller.
/// </summary>
public class NotificationMenu : MonoBehaviour {
	public GameObject menuPrefab;
	// The canvas in which put item once removed from menu.
	public GameObject miniCanvas;
	// The notification popup object.
	Transform notificationPopup;
	bool hideNotification, isHidingNotification; 
	// The menu instance.
	GameObject menuObj;
	// The notification manager.
	NotificationManager manager;

	// Use this for initialization
	void Start () {
		// Create menu.
		menuObj = Instantiate(menuPrefab);
		menuObj.transform.SetParent(transform);	
		menuObj.transform.localPosition = new Vector3(0, 0, 0);
		menuObj.transform.localRotation = Quaternion.identity;

		// Register events.
		manager = GameObject.Find("CVRREventSystem").GetComponent<NotificationManager>();
		manager.onAdd.AddListener(AddNotification);
		manager.onAdd.AddListener(PopupNotification);

		// Get notification popup instance.
		notificationPopup = transform.Find("Popup");
	}
	
	/// <summary>
	/// 	Add a new notification in the menu as text.
	/// </summary>
	/// <param name="notification"> The notification to add. </param>
	void AddNotification(NotificationManager.notification notification){
		// Add a new text component with the notification title.
		GameObject newNot = new GameObject("Notification" + notification.id);
		RectTransform rect = newNot.AddComponent<RectTransform>();
		BoxCollider collider = newNot.AddComponent<BoxCollider>();
		Rigidbody body = newNot.AddComponent<Rigidbody>();
		NotificationMenuItem not = newNot.AddComponent<NotificationMenuItem>();
		Text text = newNot.AddComponent<Text>();

		rect.sizeDelta = new Vector2(menuObj.GetComponent<RectTransform>().sizeDelta.x, 40);

		collider.size = new Vector3(rect.sizeDelta.x, rect.sizeDelta.y, 0.01f);

		newNot.transform.SetParent(menuObj.transform.Find("Scroll View/Viewport/Content"));
		newNot.transform.localPosition = Vector3.zero;
		newNot.transform.localScale = new Vector3(1, 1, 1);
		newNot.transform.localRotation = Quaternion.identity;
		newNot.layer = 11;

		not.descriptionText = notification.text;
		not.descriptionColor = notification.textColor;
		not.canvasPrefab = miniCanvas;

		body.isKinematic = true;
		body.useGravity = false;

		text.text = notification.title;
		text.color = Color.black;
		text.fontSize = 26;
		text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
	}

	/// <summary>
	/// 	Shows a little notification on the controller.
	/// </summary>
	/// <param name="notification"> The notification to show. </param>
	void PopupNotification(NotificationManager.notification notification){
		// Edit the popup notification and show it.
		TextMeshProUGUI text = notificationPopup.Find("Text").GetComponent<TextMeshProUGUI>();
		text.text = notification.title;
		text.color = notification.titleColor;

		notificationPopup.gameObject.SetActive(true);

		hideNotification = false;
		if(!isHidingNotification)
			StartCoroutine(HidePopupNotification());

		// Send haptic feedback.
		GameObject.Find("LeftController").GetComponent<ControllerFunctions>().Vibrate(90, 4000);
	}

	/// <summary>
	/// 	Hides the popup notification.
	/// </summary>
	/// <returns></returns>
	IEnumerator HidePopupNotification(){
		isHidingNotification = true;
		while(!hideNotification){
			hideNotification = true;
			yield return new WaitForSeconds(2);
		}
		isHidingNotification = false;
		notificationPopup.gameObject.SetActive(false);
	}

	void OnDisable(){
		manager.onAdd.RemoveListener(AddNotification);
		manager.onAdd.RemoveListener(PopupNotification);
	}
}
}
