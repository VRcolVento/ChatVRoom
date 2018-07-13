using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using TMPro;
using DemoAV.Common;

namespace DemoAV.Live.Notification{
/// <summary>
/// 	The class that manages the little notification menu of the controller.
/// </summary>
public class NotificationMenu : MonoBehaviour {
	public GameObject menuPrefab;
	public GameObject menuItem;
	// The canvas in which put item once removed from menu.
	public GameObject miniCanvas;
	// The notification popup object.
	Transform notificationPopup;
	bool hideNotification, isHidingNotification; 
	// The menu instance.
	GameObject menuObj;
	// The notification manager.
	NotificationManager manager;
	// Current selected notification.
	int currentNotification;

	// Use this for initialization
	void Awake () {
		// Create menu and hide it.
		menuObj = Instantiate(menuPrefab);
		menuObj.transform.SetParent(transform);	
		menuObj.transform.localPosition = new Vector3(-0.0628f, -0.0306f, 0.0045f);
		menuObj.transform.localRotation = Quaternion.Euler(-2.701f, 92.84f, 174.068f);

		// Register events.
		manager = GameObject.Find("CVRREventSystem").GetComponent<NotificationManager>();
		manager.onAdd.AddListener(AddNotification);
		manager.onAdd.AddListener(PopupNotification);

		// Get notification popup instance.
		notificationPopup = transform.Find("Notification Popup");
	}

	void Start(){
		// Set first notification highlighted.
		currentNotification = 0;
		transform.GetChild(0).GetComponent<Button>().Select();
	}

	// private void Update() {

	// }
	
	/// <summary>
	/// 	Add a new notification in the menu as text.
	/// </summary>
	/// <param name="notification"> The notification to add. </param>
	void AddNotification(NotificationManager.notification notification){
		// Add a new text component with the notification title.
		GameObject newNot = Instantiate(menuItem);
		RectTransform rect = newNot.GetComponent<RectTransform>();
		NotificationMenuItem not = newNot.GetComponent<NotificationMenuItem>();
		Text text = newNot.transform.Find("Text").GetComponent<Text>();

		rect.sizeDelta = new Vector2(menuObj.GetComponent<RectTransform>().sizeDelta.x, 40);

		newNot.GetComponent<BoxCollider>().size = new Vector3(rect.sizeDelta.x, rect.sizeDelta.y, 0.0003f);

		newNot.transform.SetParent(menuObj.transform.Find("Scroll View/Viewport/Content"));
		newNot.transform.localPosition = Vector3.zero;
		newNot.transform.localScale = new Vector3(1, 1, 1);
		newNot.transform.localRotation = Quaternion.identity;

		not.descriptionText = notification.text;
		not.descriptionColor = notification.textColor;
		not.canvasPrefab = miniCanvas;

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

	/// <summary>
	/// 	Highlights the next notification.
	/// </summary>
	void NextNotification(){
		currentNotification = currentNotification + 1 >= transform.childCount ? 0 : currentNotification + 1;

		EventSystem.current.SetSelectedGameObject(null);
		transform.GetChild(currentNotification).GetComponent<Button>().Select();
	}

	/// <summary>
	/// 	Highlights the previous notification.
	/// </summary>
	void PreviousNotification(){
		currentNotification = currentNotification - 1 < 0 ? transform.childCount - 1 : currentNotification - 1;

		EventSystem.current.SetSelectedGameObject(null);
		transform.GetChild(currentNotification).GetComponent<Button>().Select();
	}

	/// <summary>
	/// 	Selects current notification.
	/// </summary>
	void SelectNotification(){
		transform.GetChild(currentNotification).GetComponent<Button>().onClick.Invoke();

		// Make another notification to be highlighted.
		if(currentNotification != 0)	--currentNotification;
		transform.GetChild(currentNotification).GetComponent<Button>().Select();
	}

	void OnDisable(){
		// Deactivate previous button and activate first.
		EventSystem.current.SetSelectedGameObject(null);
	}

	void OnDestroy(){
		manager.onAdd.RemoveListener(AddNotification);
		manager.onAdd.RemoveListener(PopupNotification);
	}
}
}
