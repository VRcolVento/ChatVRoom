using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System;

namespace DemoAV.Editor.MenuUtil {

	/// <summary>
	/// Open the user's menu 
	/// </summary>
	public class OpenFurnitureMenu : MonoBehaviour {

		public Canvas canvas;
		public Transform lookTarget;
		private Transform hand; // The left controller
		
		private List<Vector3> initPositions;	// vertical init positions
		private List<List<string>> names;		// names of the objects to display on the buttons
		private List<List<GameObject>> buttonObjects;

		// Helpers
		public float height = 0.2f;
		private int size = 30;
		private int offset = 5;
		private Vector3 initPosition;		// First hook position

		void Start () {
			
			Transform saveBtn = transform.GetChild(0).GetChild(0);
			Transform exitBtn = transform.GetChild(0).GetChild(1);
			RectTransform canvasRect = canvas.GetComponent<RectTransform>();
			string[] directories = Directory.GetDirectories(".\\Assets\\Resources\\EditorPrefabs\\Furnitures\\");			

			names = new List<List<string>>();
			initPositions = new List<Vector3>();
			buttonObjects = new List<List<GameObject>>();

			for(int i=0; i<directories.Length; i++) {
				// Get the directories containing our model's categories (floor, wall..)
				string[] fileInDir = Directory.GetFiles(directories[i]);
				List<string> models = new List<string>();

				foreach(var f in fileInDir) {
					// Get the models in each sub directory
					string model = ExtractName(f);

					if(!model.Contains(".meta")) 
						models.Add(model.Substring(0, model.IndexOf('.')));
				}
				names.Add(models);
			}

			// Setup canvas size
			canvasRect.sizeDelta = new Vector2((MaxElementLength(names)+1) * size, (names.Count+1) * size); // offset for save/exit btns

			// starting animation position
			initPosition = new Vector3(-canvasRect.sizeDelta.x / 2 + size/2, -canvasRect.sizeDelta.y / 2 + size/2 + 30, 0);

			// Get reference to hand and setup position
			hand = GameObject.Find("Controller (left)").transform;
			transform.position = hand.position + hand.forward*7;
							
			// Create the furniture buttons			
			for(int i=0; i<names.Count; i++){
				// Iterate column == furniture category (subfolders)
				Vector3 initColumnPosition = initPosition + new Vector3(0, (size + offset)*i, 0);
				initPositions.Add(initColumnPosition);
				buttonObjects.Add(new List<GameObject>());
				
				for(int j=0; j<names[i].Count; j++){
					// Iterate row == furniture object

					GameObject btn = Instantiate(Resources.Load("EditorPrefabs/3DBtn", typeof(GameObject)),
										this.transform.GetChild(0)) as GameObject;

					btn.name = names[i][j] + "Btn";
					btn.transform.GetChild(0).GetComponent<TextMeshPro>().text = names[i][j];
					btn.transform.localPosition = initColumnPosition;

					// Add to the buttons the folders contaings the models (e.g. Floor, Wall..)					
					btn.AddComponent(Type.GetType("ButtonPathInfo"));
					btn.GetComponent<ButtonPathInfo>().MyPath = ExtractName(directories[i]);

					buttonObjects[i].Add(btn);
				}
			}		
			
			// Set save/exit buttons position on the thirds of the canvas
			float btnOffset = (canvasRect.sizeDelta.x / 2) - ((1.0f / 3.0f) * canvasRect.sizeDelta.x);
			saveBtn.localPosition = new Vector3(-btnOffset, -canvasRect.sizeDelta.y / 2 + saveBtn.gameObject.GetComponent<RectTransform>().sizeDelta.y/2, 0);
			exitBtn.localPosition = new Vector3(btnOffset, -canvasRect.sizeDelta.y / 2 + exitBtn.gameObject.GetComponent<RectTransform>().sizeDelta.y/2, 0);
		}
		
		void Update () {
			
			// Update menu position / rotation
			canvas.gameObject.transform.LookAt(lookTarget);
			canvas.gameObject.transform.position = hand.position + Vector3.up*height;
		}

		/// <summary>
		/// Menu animation
		/// </summary>
		IEnumerator SfogliaCoroutine() {

			Transform child = transform.GetChild(0);
			// BRutta condizione di uscita TODO
			while(true) {

				for(int i=0; i<buttonObjects.Count; i++)
					for(int j=0; j<buttonObjects[i].Count; j++)
						buttonObjects[i][j].transform.localPosition = Vector3.Lerp(	buttonObjects[i][j].transform.localPosition, 
																					initPositions[i] + new Vector3((size + offset)*j, 0, 0), 
																					Time.deltaTime);

				yield return new WaitForEndOfFrame();
			}

//			yield return null;
		}

		/// <summary>
		/// Open the menu
		/// </summary>
		public void OpenMenu() {

			canvas.gameObject.SetActive(true);
			StartCoroutine("SfogliaCoroutine");
		}

		/// <summary>
		/// Close the menu
		/// </summary>
		public void CloseMenu() {

			canvas.gameObject.SetActive(false);

			for(int i=0; i<buttonObjects.Count; i++)
				for(int j=0; j<buttonObjects[i].Count; j++){
					buttonObjects[i][j].transform.localPosition = initPositions[i];
				}
	
			StopCoroutine("SfogliaCoroutine");	
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

		/// <summary>
		/// Get the element of max length in a 2 level nested list
		/// <param="list">The list</param>
		/// </summary>
		private int MaxElementLength(List<List<string>> list) {

			int max = 0;
			foreach(var sublist in list)
				if(sublist.Count > max) max = sublist.Count;

			return max;
		}
	}
}