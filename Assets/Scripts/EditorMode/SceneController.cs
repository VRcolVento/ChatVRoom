using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

	private static PrefabDictonary dictionaryInstance;


	public static PrefabDictonary Dictionary {
		get { return dictionaryInstance; }
	}
	
	void Start () {
		dictionaryInstance = ScriptableObject.CreateInstance<PrefabDictonary>();
	}
}
