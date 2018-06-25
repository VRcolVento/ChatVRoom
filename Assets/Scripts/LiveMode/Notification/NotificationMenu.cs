using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace DemoAV.Live.Notification{
/// <summary>
/// 	The class that manages the little notification menu of the controller.
/// </summary>
public class NotificationMenu : MonoBehaviour {
	// The menu prefab.
	public GameObject menuPrefab;
	// The canvas in which put item once removed from menu.
	public GameObject miniCanvas;
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
		menuObj.transform.localScale = new Vector3(0.002f, 0.005f, 0);

		// Register events.
		manager = GameObject.Find("CVRREventSystem").GetComponent<NotificationManager>();
		manager.onAdd.AddListener(AddNotification);
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
		newNot.transform.localScale = new Vector3(1, 1, 1);

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

	void OnDestroy(){
		manager.onAdd.RemoveListener(AddNotification);
	}
}
}
