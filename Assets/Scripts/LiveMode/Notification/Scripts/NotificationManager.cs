using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DemoAV.Live.Notification{

/// <summary>
/// 	The event-system manager for notifications.
/// 	It uses Unity event system for handling add notification and delete notification event.
/// </summary>
public class NotificationManager : MonoBehaviour {
	/// <summary>
	/// 	Struct for notification.
	/// </summary>
	public struct notification {
		static uint totalNotification;
		uint _id;
		string _title, _text;
		Color _titleColor, _textColor;

		public notification(string title, Color titleColor, string text, Color textColor){
			_id = totalNotification++;
			_title = title;	_titleColor = titleColor;
			_text = text; _textColor = textColor;
		}

		public uint id{
			get{ return _id; }
		}

		public string title{
			get { return _title; }
		}

		public string text{
			get { return _text; }
		}

		public Color titleColor{
			get { return _titleColor; }
		}

		public Color textColor{
			get { return _textColor; }
		}
	}
	public delegate void eventCallback (notification notification);

	public class NotificationEvent : UnityEvent<notification>{}

	List<notification> notifications = new List<notification>();
	// The events list for the add and remove events.
	public NotificationEvent onAdd = new NotificationEvent();
	public NotificationEvent onRemove = new NotificationEvent();

	/// <summary>
	/// 	Adds a new notification. It calls all the OnAdd delegates.
	/// </summary>
	/// <param name="notification"></param>
	public void AddNotification(notification notification){
		// Add notification to the list.
		notifications.Add(notification);

		// Call add listeners.
		onAdd.Invoke(notification);
	}

	/// <summary>
	/// 	Deletes a notification. It calls all the OnAdd delegates.
	/// </summary>
	/// <param name="notification"> The notification to delete. </param>
	public void Delete(notification notification){
		notifications.Remove(notification);

		// Call remove listeners.
		onRemove.Invoke(notification);
	}

	/// <summary>
	/// 	Clears all the actual notifications.
	/// </summary>
	public void Clear(){
		notifications.Clear();
	}

	/// <summary>
	/// 	Returns all notifications.
	/// </summary>
	/// <returns> An array with all the notifications. </returns>
	public notification[] GetNotifications(){
		return notifications.ToArray();
	}

	void OnDestroy(){
		onAdd.RemoveAllListeners();
		onRemove.RemoveAllListeners();
	}	
}
}
