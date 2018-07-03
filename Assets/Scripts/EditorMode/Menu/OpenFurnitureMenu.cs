using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

namespace DemoAV.Editor.MenuUtil {

	/// <summary>
	/// Open the user's menu 
	/// </summary>
	public class OpenFurnitureMenu : MonoBehaviour {

		public Canvas canvas;
		public Transform lookTarget;
		private Transform hand; // The left controller
		
		private List<Vector3> buttons;
		private List<string> names;

		// Helpers
		public float height = 0.2f;
		private int size = 30;
		private int offset = 5;
		private int childOffset = 2;
		private Vector3 initPosition;

		void Start () {
			
			Transform saveBtn = transform.GetChild(0).GetChild(0);
			Transform exitBtn = transform.GetChild(0).GetChild(1);
			RectTransform canvasRect = canvas.GetComponent<RectTransform>();
			string[] files = Directory.GetFiles("./Assets/Resources/EditorPrefabs/Furnitures/");

			names = new List<string>();
			buttons = new List<Vector3>();

			foreach (var f in files) {
				// Get the list of names
				string model = ExtractName(f);

				if(!model.Contains(".meta")) {
					names.Add(model.Substring(0, model.IndexOf('.')));
				}		
			}

			// Setup canvas and starting animation position
			canvasRect.sizeDelta = new Vector2((names.Count+1) * size, 60);
			initPosition = new Vector3(-canvasRect.sizeDelta.x / 2 + size/2, 0, 0);

			// Get reference to hand and setup position
			hand = GameObject.Find("Controller (left)").transform;
			transform.position = hand.position + hand.forward*7;

							
			// Create the furniture buttons			
			for(int i=0; i<names.Count; i++){

				GameObject btn = Instantiate(Resources.Load("EditorPrefabs/3DBtn", typeof(GameObject)),
									this.transform.GetChild(0)) as GameObject;

				btn.transform.GetChild(0).GetComponent<TextMeshPro>().text = names[i];

				btn.transform.localPosition = initPosition;
				buttons.Add(initPosition);
			}		
			
			// Set save/exit buttons position on the thirds of the canvas
			float btnOffset = (canvasRect.sizeDelta.x / 2) - ((1.0f / 3.0f) * canvasRect.sizeDelta.x);
//			saveBtn.localPosition = new Vector3((1.0f / 3.0f) * canvasRect.sizeDelta.x, saveBtn.localPosition.y, 0);
//			saveBtn.localPosition = new Vector3(canvasRect.sizeDelta.x / 2, saveBtn.localPosition.y, saveBtn.localPosition.z);
			saveBtn.localPosition = new Vector3(-btnOffset, saveBtn.localPosition.y, saveBtn.localPosition.z);
			exitBtn.localPosition = new Vector3(btnOffset, saveBtn.localPosition.y, saveBtn.localPosition.z);
		}
		
		void Update () {
			
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

				for(int i=0; i<buttons.Count; i++)
					child.GetChild(i+childOffset).localPosition = Vector3.Lerp(child.GetChild(i+childOffset).localPosition, buttons[i] + new Vector3((size + offset)*i, 0, 0), Time.deltaTime);

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
			Transform child = transform.GetChild(0);				
			for(int i=0; i<buttons.Count; i++)
				child.GetChild(i+childOffset).localPosition = initPosition;
			StopCoroutine("SfogliaCoroutine");	
		}


		private string ExtractName(string path) {

			while(true) {

				path = path.Substring(path.IndexOf('/') + 1);
				if(path.IndexOf('/') == -1)
					return path;
			}
		}
	}
}