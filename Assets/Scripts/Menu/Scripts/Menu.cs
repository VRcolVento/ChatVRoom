using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoAV.StartMenu{
public class Menu : MonoBehaviour {
	GameObject currentMenu;

	void Start(){
		currentMenu = GameObject.Find("Default Menu Tab");
	}

	/// <summary>
	/// 	Opens a new menu.
	/// </summary>
	/// <param name="menuName"> The name of the menu to open.</param>
	public void OpenNewMenu(string menuName){
		currentMenu.SetActive(false);
		currentMenu = transform.Find(menuName + " Tab").gameObject;
		currentMenu.SetActive(true);
	}
}
}

