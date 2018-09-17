using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DemoAV.Live.SmarTv.SmartMenu;
using DemoAV.Common;

namespace DemoAV.Live.SmarTv{

/// <summary>
/// 	Class that realizes the factory pattern for the tv menu. 
/// 	It also allows to interact with the current active menu without
/// 	exposing it.
/// </summary>
public class TvMenuFactory : MonoBehaviour {

	static LayerMask menuMask = 1 << 9;
	public enum Type { PANEL_MENU, TEXT_MENU, SOCIAL_MENU };
	Dictionary<string, Menu> menus = new Dictionary<string, Menu>();		// The already existents menu.
	GameObject activeMenuObj = null;										// The menu game object currently active.
	Menu activeMenu;														// The menu currently active.
	Stack<string> menuStack = new Stack<string>();							// The stack of menus.
	public GameObject remoteController;
	LineRenderer liner = null;

	/************************** PANEL MENU ******************************/
	public GameObject panel;

	/************************** TEXT MENU ******************************/
	public GameObject text;


	/// <summary>
	/// 	Start is called on the frame when a script is enabled just before
	/// 	any of the Update methods is called the first time.
	/// </summary>
	void Start () {
		remoteController.GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, (RaycastHit hit) => {
			if(hit.transform != null &&  hit.transform.gameObject != null && hit.transform.gameObject.layer == Menu.menuLayer) {
				activeMenu.SetSelected(hit.transform.gameObject.name);
				activeMenu.Active(hit.transform.gameObject.name);
			}
		});  
	}
	
	/// <summary>
	/// 	Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update () {
		// Select the current hit item.
		GameObject hit = remoteController.GetComponent<InteractionLaser>().lastHitObject;

		if (hit && hit.layer == Menu.menuLayer)
			activeMenu.SetSelected(hit.name);
		else if (activeMenu.selectedItem != null)
			activeMenu.SetSelected(null);
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
	/// 	Change the current tab of the active menu if any.
	/// </summary>
	/// <param name="direction"> 0 > : go backward by the number.
	/// 						 0 < : go backward by the module of the number.
	/// 						 0 : go to the desidered tab.
	/// </param>
	/// <param name="index"> The desidered tab. </param>
	public void ChangeTab(int direction, int index = 0){
		if(activeMenu != null){
			if(direction == 0 )
				activeMenu.activeTab = index;
			else
				activeMenu.activeTab += direction;
		}
	}
}
}