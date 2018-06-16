using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  DemoAV.Common {
public class KeyboardHandler : MonoBehaviour {
	public delegate void KeyCallback();
	public enum Map{
		KEY_DOWN = 0,
		KEY_UP = 1,
		KEY_PRESSED = 2
	};
	static Dictionary<KeyCode, HashSet<KeyCallback>>[] keyMap = new Dictionary<KeyCode, HashSet<KeyCallback>>[3]{
		new Dictionary<KeyCode, HashSet<KeyCallback>>(), 
		new Dictionary<KeyCode, HashSet<KeyCallback>>(), 
		new Dictionary<KeyCode, HashSet<KeyCallback>>()
	};
	static Dictionary<KeyCode, HashSet<KeyCallback>>[] mapBackup = new Dictionary<KeyCode, HashSet<KeyCallback>>[3]{
		new Dictionary<KeyCode, HashSet<KeyCallback>>(), 
		new Dictionary<KeyCode, HashSet<KeyCallback>>(), 
		new Dictionary<KeyCode, HashSet<KeyCallback>>()
	};	

	void Awake(){
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// Key down.
		foreach(KeyValuePair<KeyCode, HashSet<KeyCallback>> keyPair in keyMap[(int)Map.KEY_DOWN]){
			if(Input.GetKeyDown(keyPair.Key))
				foreach(KeyCallback callback in keyPair.Value)
					callback();
		}

		// Key up.
		foreach(KeyValuePair<KeyCode, HashSet<KeyCallback>> keyPair in keyMap[(int)Map.KEY_UP]){
			if(Input.GetKeyUp(keyPair.Key))
				foreach(KeyCallback callback in keyPair.Value)
					callback();
		}

		// Key pressed.
		foreach(KeyValuePair<KeyCode, HashSet<KeyCallback>> keyPair in keyMap[(int)Map.KEY_PRESSED]){
			if(Input.GetKey(keyPair.Key))
				foreach(KeyCallback callback in keyPair.Value)
					callback();
		}
	}

	/// <summary>
	/// 	Add a callback to the list of the callbacks for a given key event.
	/// </summary>
	/// <param name="type">The type of the event.</param>
	/// <param name="key">The key focused by the event.</param>
	/// <param name="callback">The callback to add.</param>
	static public void AddCallback(Map type, KeyCode key, KeyCallback callback){
		HashSet<KeyCallback> hs;

		Dictionary<KeyCode, HashSet<KeyCallback>> currDic = keyMap[(int)type];

		// If set does not exist, create it.
		if(!currDic.TryGetValue(key, out hs)) {
			hs = new HashSet<KeyCallback>();
			currDic.Add(key, hs);
		}

		// Add callback.
		hs.Add(callback);
	}

	/// <summary>
	/// 	Substitute the whole key event set with a single callback. To restore
	/// 	the old set of callback use the method RestoreCallback(). This is 
	/// 	useful when you want to radically change the keyboard handler for few 
	/// 	time.
	/// </summary>
	/// <example>
	/// 	KeyboardHandler.SetCallback(Map.KEY_DOWN, KeyCode.Space, MyCallback);
	/// 	....
	/// 	....
	/// 	....
	/// 	KeyboardHandler.RestoreCallbacks(Map.KEY_DOWN, KeyCode.Space);
	/// </example>
	/// <param name="type">The type of the event.</param>
	/// <param name="key">The key focused by the event.</param>
	/// <param name="callback">The callback for that event.</param>
	static public void SetCallback(Map type, KeyCode key, KeyCallback callback){
		Dictionary<KeyCode, HashSet<KeyCallback>> currDic = keyMap[(int)type];

		// Save the set in the backup array.
		if(currDic.ContainsKey(key))
			mapBackup[(int)type][key] = currDic[key];

		// Set the passed callback as the only one.
		HashSet<KeyCallback> hs = new HashSet<KeyCallback>();
		hs.Add(callback);
		currDic[key] = hs;
	}

	/// <summary>
	/// 	Remove a given callback from the set of callback of a given key event.
	/// </summary>
	/// <param name="type">The type of the event.</param>
	/// <param name="key">The key focused by the event.</param>
	/// <param name="callback">The callback to remove.</param>
	static public void RemoveCallback(Map type, KeyCode key, KeyCallback callback){
		HashSet<KeyCallback> hs;

		Dictionary<KeyCode, HashSet<KeyCallback>> currDic = keyMap[(int)type];

		// If set does not exist, delete it.
		if(currDic.TryGetValue(key, out hs)) {
			hs.Remove(callback);

			if(hs.Count == 0)
				currDic.Remove(key);
		}
	}

	/// <summary>
	/// 	Restore the last saved set of callbacks for a given keyboard event. The
	/// 	current set is overwritten.
	/// </summary>
	/// <param name="type">The type of the event.</param>
	/// <param name="key">The key focused by the event.</param>
	static public void RestoreCallbacks(Map type, KeyCode key){
		Dictionary<KeyCode, HashSet<KeyCallback>> currDic = keyMap[(int)type];

		// Restore last saved callbacks set.
		if(currDic.ContainsKey(key))
			currDic[key] = mapBackup[(int)type][key];
	}
}
}

