using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System;

namespace DemoAV.Editor.MenuUtil {

	/// <summary>
	/// Open the user's furniture menu 
	/// </summary>
	public class OpenFurnitureMenuList : MonoBehaviour {

		public GameObject menuHook;

		public GameObject btnPrefab;

		public Canvas canvas;
		public Transform menuTransform;
		private RectTransform canvasRect;

		private Transform hand; // The left controller
		
		private string[] directories;
		private Dictionary<string, string> dirMap;

		// Helpers
		public float height = 0.2f;
		private int size = 30;

		void Awake() {
			hand = GameObject.Find("Controller (left)").transform;
			canvasRect = canvas.GetComponent<RectTransform>();
			directories = Directory.GetDirectories(".\\Assets\\Resources\\EditorPrefabs\\Furnitures\\");

		}

		void Start () {

			// Get reference to hand and setup position
			transform.position = hand.position + hand.forward*7;
			dirMap = new Dictionary<string, string>();

			foreach(string dir in directories) {
				dirMap.Add(ExtractName(dir), dir);
			}

			// Load main menu
			mainMenu();
			
			// Set save/exit buttons position on the thirds of the canvas
			Transform saveBtn = GameObject.Find("SaveCanvas/SaveBtn").transform;
//			Transform exitBtn = GameObject.Find("SaveCanvas/ExitBtn").transform;
			float btnOffset = (canvasRect.sizeDelta.x / 2) - ((1.0f / 2.0f) * canvasRect.sizeDelta.x);
			saveBtn.localPosition = new Vector3(-btnOffset, -canvasRect.sizeDelta.y / 2 + saveBtn.gameObject.GetComponent<RectTransform>().sizeDelta.y/2, 0);
//			exitBtn.localPosition = new Vector3(btnOffset, -canvasRect.sizeDelta.y / 2 + exitBtn.gameObject.GetComponent<RectTransform>().sizeDelta.y/2, 0);
		}
		
		void Update () {
			
			// Update menu position / rotation
			transform.position = hand.position + Vector3.up*height;
		}

		/// <summary>
		/// Open the sub menu with a certain category of furnitures
		/// <param="dir">The name of the directory to open</param>
		/// </summary>		
		public void OpenSubMenu(string dir) {

			// Destroy current menu
			foreach(Transform child in menuTransform)
				Destroy(child.gameObject);

			string[] names = Directory.GetFiles(dirMap[dir]);

			// Setup canvas size
			canvasRect.sizeDelta = new Vector2((names.Length / 2 + 1) * size, size); // discard meta, plus back btn

			// Populate menu
			foreach(string s in names) {
				
				string model = ExtractName(s);

				if(!model.Contains(".meta")) {

					GameObject btn = Instantiate(btnPrefab,	menuTransform) as GameObject;
					model = model.Substring(0, model.IndexOf('.'));			
					addInfo(btn, "MenuFurniture", model, dir);
				}
			}

			GameObject backBtn = Instantiate(btnPrefab,	menuTransform) as GameObject;

			backBtn.tag = "MenuBack";
			backBtn.name = "Back";
			backBtn.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Back";
		}

		/// <summary>
		/// Open the menu
		/// </summary>
		public void OpenMenu() {

			menuHook.SetActive(true);
			// Load main menu
			mainMenu();
		}

		/// <summary>
		/// Close the menu
		/// </summary>
		public void CloseMenu() {

			menuHook.SetActive(false);
		}

		/// <summary>
		/// Helper function to display the main furniture menu, i.e. the buttons with the categories
		/// </summary>
		private void mainMenu() {

			// Destroy current menu
			foreach(Transform child in menuTransform)
				Destroy(child.gameObject);

			// Setup canvas size
			canvasRect.sizeDelta = new Vector2(directories.Length * size, size);

			foreach(string directory in directories) {
				// Iterate row == furniture object

				GameObject btn = Instantiate(btnPrefab,	menuTransform) as GameObject;
				string dir = ExtractName(directory);
				addInfo(btn, "MenuObject", dir, directory);			
			}
		}


		/// <summary>
		/// Add information to a menu button
		/// <param="btn">The Gameobject button</param>
		/// <param="tag">The tag of the button</param>
		/// <param="name">The name of the button</param>
		/// <param="path">The path (previous directory) of the furniture</param>
		/// </summary>
		private void addInfo(GameObject btn, string tag, string name, string path) {

			btn.tag = tag;
			btn.name = name;
			btn.transform.GetChild(0).GetComponent<TextMeshPro>().text = name;

			// Add to the buttons the folders contaings the models (e.g. Floor, Wall..)					
			btn.AddComponent(Type.GetType("DemoAV.Editor.MenuUtil.ButtonPathInfo"));
			btn.GetComponent<ButtonPathInfo>().MyPath = path;
		}

		/// <summary>
		/// Extract the last position in path
		/// <param="path">The path</param>
		/// </summary>
		private string ExtractName(string path) {

			while(true) {

				path = path.Substring(path.IndexOf('\\') + 1);
				if(path.IndexOf('\\') == -1)
					return path;
			}
		}
	}
}