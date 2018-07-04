using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoAV.Editor.StorageUtility;

namespace DemoAV.Editor.SceneControl {

	public class SceneController : MonoBehaviour {

		private static PrefabDictonary dictionaryInstance;


		public static PrefabDictonary Dictionary {
			get { return dictionaryInstance; }
		}
		
		void Start () {
			dictionaryInstance = ScriptableObject.CreateInstance<PrefabDictonary>();
			Debug.Log(dictionaryInstance);
		}
	}

}
