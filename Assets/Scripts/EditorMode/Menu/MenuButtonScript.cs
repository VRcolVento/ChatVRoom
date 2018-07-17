using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DemoAV.Editor.User;
using DemoAV.Editor.SceneControl;
using TMPro;

namespace DemoAV.Editor.MenuUtil {

	/// <summary>
	/// Class to handle the the callbacks for the user menu.
	/// </summary>
	public class MenuButtonScript : MonoBehaviour {

		private Color32 defaultColor;
		
		void Start () {
			defaultColor = GetComponent<Image>().color;
			VRChooseObject.menuSelect += OnPointerEnter;
			VRChooseObject.menuDeselect += OnPointerExit;
			VRChooseObject.menuPress += Press;
		}

		
		/// <summary>
		/// Button pressed
		/// <para name="btn">The pressed button</para>
		/// </summary>
		public void Press(GameObject btn) {
			if(btn != null && GameObject.ReferenceEquals(btn, this.gameObject))
				GetComponent<Image>().color = new Color32(224, 0, 0, 77);
		}

		/// <summary>
		/// Button enter
		/// <para name="btn">The entered button</para>
		/// </summary>
		public void OnPointerEnter(GameObject obj) {			
			if(obj != null && GameObject.ReferenceEquals(obj, this.gameObject)) {
				GetComponent<Image>().color = new Color32(224, 243, 74, 77); // giallino	
			}
		}

		/// <summary>
		/// Button exit
		/// </summary>
		public void OnPointerExit() {
			GetComponent<Image>().color = defaultColor;
		}

		public void OnDestroy() {

			VRChooseObject.menuSelect -= OnPointerEnter;
			VRChooseObject.menuDeselect -= OnPointerExit;
			VRChooseObject.menuPress -= Press;
		}
	}
}
