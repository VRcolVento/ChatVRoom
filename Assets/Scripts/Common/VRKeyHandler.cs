using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  DemoAV.Common {
/// It handles the controller input event.
public class VRKeyHandler : MonoBehaviour {
	// Steam controller references.
	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}
	// The callback. The arguments is the object hit by a raycast from controller.
	public delegate void KeyCallback(RaycastHit hit);
	// Enumeration and structs.
	public enum Map{ KEY_DOWN = 0, KEY_UP = 1, KEY_PRESSED = 2 };
	public enum Key : ulong { 
		TRIGGER = SteamVR_Controller.ButtonMask.Trigger, GRIP = SteamVR_Controller.ButtonMask.Grip,
		TOUCH = SteamVR_Controller.ButtonMask.Touchpad, SYSTEM = SteamVR_Controller.ButtonMask.System,
		MENU = SteamVR_Controller.ButtonMask.ApplicationMenu, AXIS0 = SteamVR_Controller.ButtonMask.Axis0,
		AXIS1 = SteamVR_Controller.ButtonMask.Axis1, AXIS2 = SteamVR_Controller.ButtonMask.Axis2,
		AXIS3 = SteamVR_Controller.ButtonMask.Axis3, AXIS4 = SteamVR_Controller.ButtonMask.Axis4 };
	struct Container{
		public readonly Key key;
		public readonly Map map;
		public readonly KeyCallback callback;
		public readonly bool toAdd;
		public Container(Map map, Key key, KeyCallback callback, bool isNew){
			this.key = key; this.map = map; this.callback = callback; this.toAdd = isNew;
		}
	}
	// Dictonary of callbacks.
	Dictionary<Key, HashSet<KeyCallback>>[] keyMap = new Dictionary<Key, HashSet<KeyCallback>>[3]{
		new Dictionary<Key, HashSet<KeyCallback>>(), 
		new Dictionary<Key, HashSet<KeyCallback>>(), 
		new Dictionary<Key, HashSet<KeyCallback>>()
	};
	Dictionary<Key, HashSet<KeyCallback>>[] mapBackup = new Dictionary<Key, HashSet<KeyCallback>>[3]{
		new Dictionary<Key, HashSet<KeyCallback>>(), 
		new Dictionary<Key, HashSet<KeyCallback>>(), 
		new Dictionary<Key, HashSet<KeyCallback>>()
	};	
	List<Container> deferredCallback = new List<Container>();

	void Awake(){
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Ray raycast = new Ray(transform.position + transform.forward * 2, transform.forward);
        RaycastHit hit;
		bool bHit = Physics.Raycast(raycast, out hit);

		// Key down.
		foreach(KeyValuePair<Key, HashSet<KeyCallback>> keyPair in keyMap[(int)Map.KEY_DOWN]){
			if(Controller.GetPressDown((ulong)keyPair.Key))
				foreach(KeyCallback callback in keyPair.Value)
					callback(hit);
		}

		// Key up.
		foreach(KeyValuePair<Key, HashSet<KeyCallback>> keyPair in keyMap[(int)Map.KEY_UP]){
			if(Controller.GetPressUp((ulong)keyPair.Key))
				foreach(KeyCallback callback in keyPair.Value)
					callback(hit);
		}

		// Key pressed.
		foreach(KeyValuePair<Key, HashSet<KeyCallback>> keyPair in keyMap[(int)Map.KEY_PRESSED]){
			if(Controller.GetPress((ulong)keyPair.Key))
				foreach(KeyCallback callback in keyPair.Value)
					callback(hit);
		}

		// Deferred removal.
		if(deferredCallback.Count > 0)		DeferredOperations();
	}

	/// <summary>
	/// 	Add a callback to the list of the callbacks for a given key event.
	/// </summary>
	/// <param name="type">The type of the event.</param>
	/// <param name="key">The key focused by the event.</param>
	/// <param name="callback">The callback to add.</param>
	public void AddCallback(Map type, Key key, KeyCallback callback){
		HashSet<KeyCallback> hs;

		Dictionary<Key, HashSet<KeyCallback>> currDic = keyMap[(int)type];

		// If set does not exist, create it.
		if(!currDic.TryGetValue(key, out hs)) {
			hs = new HashSet<KeyCallback>();
			currDic.Add(key, hs);
		}

		// Add callback.
		hs.Add(callback);
	}

	/// <summary>
	/// 	Remove a given callback from the set of callback of a given key event.
	/// </summary>
	/// <param name="type">The type of the event.</param>
	/// <param name="key">The key focused by the event.</param>
	/// <param name="callback">The callback to remove.</param>
	public void RemoveCallback(Map type, Key key, KeyCallback callback){
		HashSet<KeyCallback> hs;

		Dictionary<Key, HashSet<KeyCallback>> currDic = keyMap[(int)type];

		// If set does not exist, delete it.
		if (currDic.TryGetValue(key, out hs)) {
			hs.Remove(callback);

			if (hs.Count == 0)
				currDic.Remove(key);
		}
	}

	/// <summary>
	/// 	Removes a given callback from the set of callback of a given key event within another 
	/// 	key callback. This method must be called only if the removal of a callback happens in
	/// 	another function that is already binds to a key event. 
	/// 	To call the default remove method will otherwise generates an error at runtime.
	/// </summary>
	/// <param name="type"> The type of the event. </param>
	/// <param name="key"> The key focused by the event. </param>
	/// <param name="callback"> The callback to remove. </param>
	public void DeferredRemoveCallback(Map type, Key key, KeyCallback callback){
		deferredCallback.Add(new Container(type, key, callback, false));
	}

	/// <summary>
	/// 	Add a given callback from the set of callback of a given key event within another 
	/// 	key callback. This method must be called only if the addition of a callback happens in
	/// 	another function that is already binds to a key event. 
	/// 	To call the default add method will otherwise generates an error at runtime.
	/// </summary>
	/// <param name="type"> The type of the event. </param>
	/// <param name="key"> The key focused by the event. </param>
	/// <param name="callback"> The callback to add. </param>
	public void DeferredAddCallback(Map type, Key key, KeyCallback callback){
		deferredCallback.Add(new Container(type, key, callback, true));
	}

	/// <summary>
	/// 	The deferred removal and addition of the callbacks.
	/// </summary>
	void DeferredOperations(){
		foreach (Container container in deferredCallback){
			if (container.toAdd)		AddCallback(container.map, container.key, container.callback);
			else						RemoveCallback(container.map, container.key, container.callback);
		}

		deferredCallback.Clear();
	}

	/// <summary>
	/// 	Substitute the whole key event set with a single callback. To restore
	/// 	the old set of callback use the method RestoreCallback(). This is 
	/// 	useful when you want to radically change the keyboard handler for few 
	/// 	time.
	/// </summary>
	/// <example>
	/// 	KeyboardHandler.SetCallback(Map.KEY_DOWN, Key.Space, MyCallback);
	/// 	....
	/// 	....
	/// 	....
	/// 	KeyboardHandler.RestoreCallbacks(Map.KEY_DOWN, Key.Space);
	/// </example>
	/// <param name="type">The type of the event.</param>
	/// <param name="key">The key focused by the event.</param>
	/// <param name="callback">The callback for that event.</param>
	public void SetCallback(Map type, Key key, KeyCallback callback){
		Dictionary<Key, HashSet<KeyCallback>> currDic = keyMap[(int)type];

		// Save the set in the backup array.
		if(currDic.ContainsKey(key))
			mapBackup[(int)type][key] = currDic[key];

		// Set the passed callback as the only one.
		HashSet<KeyCallback> hs = new HashSet<KeyCallback>();
		hs.Add(callback);
		currDic[key] = hs;
	}

	/// <summary>
	/// 	Restore the last saved set of callbacks for a given keyboard event. The
	/// 	current set is overwritten.
	/// </summary>
	/// <param name="type">The type of the event.</param>
	/// <param name="key">The key focused by the event.</param>
	public void RestoreCallbacks(Map type, Key key){
		Dictionary<Key, HashSet<KeyCallback>> currDic = keyMap[(int)type];

		// Restore last saved callbacks set.
		if(currDic.ContainsKey(key))
			currDic[key] = mapBackup[(int)type][key];
	}
}
}