using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DemoAV.SmartMenu;

public class TvMenuFactory : MonoBehaviour {

	static LayerMask menuMask = 1 << 9;
	public enum Type { PANEL_MENU, TEXT_MENU, SOCIAL_MENU };
	Dictionary<string, Menu> menus = new Dictionary<string, Menu>();		// The already existents menu.
	GameObject activeMenuObj = null;										// The menu game object currently active.
	Menu activeMenu;														// The menu currently active.
	Stack<string> menuStack = new Stack<string>();							// The stack of menus.
	public GameObject remoteController;
	LineRenderer liner;

	/************************** PANEL MENU ******************************/
	public GameObject panel;

	/************************** TEXT MENU ******************************/
	public GameObject text;


	// Use this for initialization
	void Start () {
		liner = GameObject.Find("SignorTelecomando").GetComponent<LineRenderer>();
		menuMask = LayerMask.GetMask(new string[]{"MenuLayer"});
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = new Ray(remoteController.transform.position, Vector3.forward);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit)){
			liner.SetPosition(1, hit.point);

			GameObject hitted = hit.transform.gameObject;
			if(hitted.layer == 10){
				activeMenu.SetSelected(hitted.name);

				if(Input.GetMouseButtonDown(1))
					activeMenu.Active(hitted.name);
			}
		}
	}

	/// <summary>
	/// 	Creates a new menu of the given type.
	/// </summary>
	/// <param name="type"> The type of the menu to create. </param>
	/// <param name="name"> The name of the menu. </param>
	/// <returns></returns>
	public Menu CreateMenu(Type type, string name){
		// Check if a menu with the same method exists.
		if(menus.ContainsKey(name))		return null;

		// Create a menu based on the type.
		Menu newMenu;
		switch(type){
			case Type.PANEL_MENU:
				newMenu =  new PanelMenu(transform.Find("Display").gameObject, panel, name, 3, 5); break;
			case Type.TEXT_MENU:
				newMenu = new TextMenu(transform.Find("Display").gameObject, text, name, 15); break;
			case Type.SOCIAL_MENU:
				newMenu = new SocialMenu(transform.Find("Display").gameObject, text, name, 4); break;
			default:
				return null;
		}
		
		menus.Add(name, newMenu);
		return newMenu;
	}

	/// <summary>
	/// 	Sets a new menu as the active one, deactivating the last one.
	/// </summary>
	/// <param name="name"> The name of the menu to activate. </param>
	public void SetActiveMenu(string name){
		// If null is passed, just deactivate all the menus.
		if(name == null){
			if(activeMenuObj != null){
				// Add the old menu to the stack of active menu.
				menuStack.Push(activeMenuObj.name);

				// Null the active menu.
				activeMenuObj.SetActive(false);
				activeMenuObj = null;
			}
		}
		else{
			Transform searchedMenu = transform.Find("Display/MenuRoot/" + name);

			// If the menu exists, show it.
			if(searchedMenu != null){
				if(activeMenuObj != null){
					activeMenuObj.SetActive(false);
				
					// Add the old menu to the stack of active menu.
					menuStack.Push(activeMenuObj.name);
				} 		
				
				activeMenuObj = searchedMenu.gameObject;
				activeMenuObj.SetActive(true);
				menus.TryGetValue(name, out activeMenu);
			}
		}		
	}

	/// <summary>
	/// 	Cames back to the menu shown just before the current one, if any.
	/// </summary>
	public void GoBack(){
		if(menuStack.Count != 0){
			string name = menuStack.Pop();
			Transform searchedMenu = transform.Find("Display/MenuRoot/" + name);

			// If the menu exists, show it.
			if(searchedMenu != null){
				if(activeMenuObj != null){
					activeMenuObj.SetActive(false);
				} 		
				
				activeMenuObj = searchedMenu.gameObject;
				activeMenuObj.SetActive(true);
				menus.TryGetValue(name, out activeMenu);
			}
		}
	}

	/// <summary>
	/// 	Clears the stack of the menu, make calls to GoBack() pointless.
	/// </summary>
	public void ClearMenuStack(){
		menuStack.Clear();
	}

	/// <summary>
	/// 	Change the current tab of the active menu.
	/// </summary>
	/// <param name="direction"> 0 > : go backward by the number.
	/// 						 0 < : go backward by the module of the number.
	/// 						 0 : go to the desidered tab.
	/// </param>
	/// <param name="index"> The desidered tab. </param>
	public void ChangeTab(int direction, int index = 0){
		if(direction == 0 )
			activeMenu.activeTab = index;
		else
			activeMenu.activeTab += direction;
	}
}
