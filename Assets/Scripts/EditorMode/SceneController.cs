using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoAV.Editor.StorageUtility;

namespace DemoAV.Editor.SceneControl {

	/// <summary>
	/// Hook for the Dictionary to handle storage/load of the scene
	/// </summary>
	public class SceneController : MonoBehaviour {

		private static PrefabDictionary dictionaryInstance;

		public static PrefabDictionary Dictionary {
			get { return dictionaryInstance; }
		}
		
		void Awake () {
			Debug.Log( GameObject.Find("GlobalDictionary").GetComponent<PrefabDictionary>());
			dictionaryInstance = GameObject.Find("GlobalDictionary").GetComponent<PrefabDictionary>();
			dictionaryInstance.Load();
		}
	}
}
