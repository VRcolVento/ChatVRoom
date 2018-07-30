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
	Canvas menuCanvas;
	// The notification popup object.
	Transform notificationPopup;
	bool hideNotification, isHidingNotification; 
	// The menu instance.
	GameObject menuObj;
	// The notification manager.
	NotificationManager manager;
	// Current selected notification.
	int currentNotification;
	// Shortcut to son.
	Transform content;
	ScrollRect rect;

	// Use this for initialization
	void Awake () {
		// Create menu and hide it.
		menuObj = Instantiate(menuPrefab);
		menuObj.transform.SetParent(transform);	
		menuObj.transform.localPosition = new Vector3(-0.077f, -0.03061f, 0.05f);
		menuObj.transform.localRotation = Quaternion.Euler(-2.701f, 92.84f, 174.068f);
		content = menuObj.transform.Find("Scroll View/Viewport/Content");
		rect = menuObj.transform.Find("Scroll View").GetComponent<ScrollRect>();
		menuCanvas = menuObj.GetComponent<Canvas>();

		// Register events.
		manager = GameObject.Find("CVRREventSystem").GetComponent<NotificationManager>();
		manager.onAdd.AddListener(AddNotification);
		manager.onAdd.AddListener(PopupNotification);

		// Get notification popup instance.
		notificationPopup = transform.Find("Notification Popup");

		GameObject.Find("LeftController").GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.AXIS0, ChangeNotification);
		GameObject.Find("LeftController").GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, SelectNotification);
	}

	private void Update() {
		float upAngle = Vector3.Dot(Camera.main.transform.forward.normalized, transform.up.normalized);
		float rightAngle = Vector3.Dot(Camera.main.transform.forward.normalized, transform.right.normalized);
		if(Mathf.Abs(rightAngle) > 0.7 && Mathf.Abs(upAngle) < 0.2){
			if(!menuCanvas.enabled){
				menuCanvas.enabled = true;
				currentNotification = 0;
				if(content.childCount > 0) content.GetChild(0).GetComponent<Button>().Select();
				rect.verticalNormalizedPosition = 1;
			}
		}
		else{
			menuCanvas.enabled = false;
		}
	}
	
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

		newNot.transform.SetParent(content);
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
	/// 	Changes the current highlighted notification.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void ChangeNotification(RaycastHit hit){
		if(menuCanvas.enabled && content.childCount > 1){
			Vector3 axis = GameObject.Find("LeftController").GetComponent<ControllerFunctions>().GetAxis();

			if(axis.y <= 0.2){
				if(axis.x >= 0.3)			NextNotification();
				else if(axis.x <= -0.3)		PreviousNotification();
			}
		}
	}

	/// <summary>
	/// 	Highlights the next notification.
	/// </summary>
	void NextNotification(){
		currentNotification = currentNotification + 1 >= content.childCount ? 0 : currentNotification + 1;

		EventSystem.current.SetSelectedGameObject(null);
		content.GetChild(currentNotification).GetComponent<Button>().Select();
	}

	/// <summary>
	/// 	Highlights the previous notification.
	/// </summary>
	void PreviousNotification(){
		currentNotification = currentNotification - 1 < 0 ? content.childCount - 1 : currentNotification - 1;

		EventSystem.current.SetSelectedGameObject(null);
		content.GetChild(currentNotification).GetComponent<Button>().Select();
	}

	/// <summary>
	/// 	Selects current notification.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void SelectNotification(RaycastHit hit){
		if(menuCanvas.enabled){
			content.GetChild(currentNotification).GetComponent<Button>().onClick.Invoke();

			// Make another notification to be highlighted.
			if(currentNotification != 0)	--currentNotification;
			content.GetComponent<Button>().Select();
		}
	}

	void OnDestroy(){
		manager.onAdd.RemoveListener(AddNotification);
		manager.onAdd.RemoveListener(PopupNotification);
		GameObject.Find("LeftController").GetComponent<VRKeyHandler>().RemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.AXIS0, ChangeNotification);
		GameObject.Find("LeftController").GetComponent<VRKeyHandler>().RemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, SelectNotification);
	}
}
}
