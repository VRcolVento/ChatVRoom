using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserChooseObject : MonoBehaviour {

	public delegate void SelectAction(GameObject obj);
	public delegate void DeselectAction();
	public static event SelectAction select;
	public static event DeselectAction deselect;
	public static event SelectAction menuSelect;
	public static event DeselectAction menuDeselect;
	public static event SelectAction menuPress;

	// UI
	public Text stateText;

	// Object
	string prefabName = "";
	GameObject objToPlace = null;


	UpdateLineRenderer lineRenderer;

	// Mask
	int furnitureMask;
	int UImask;

	// Placing script reference
	UserPlaceObject placingScript;

	void Awake() {
		placingScript = GetComponent<UserPlaceObject>();
	}

	void Start() {
		furnitureMask = LayerMask.GetMask("FurnitureLayer");
		UImask = LayerMask.GetMask("UI");
		lineRenderer = GameObject.Find("Mano").GetComponent<UpdateLineRenderer>();	
	}

	void Update () {
		
		// Check if the user wants to place a new furniture
		if (chooseFurniture(out objToPlace)) {

			placingScript.setObject(objToPlace, prefabName);
			objToPlace.GetComponent<Interactible>().RemoveSelectionEvent();

			// Update status: switch working scripts
			this.enabled = false;
			placingScript.enabled = true;
		}

		// Check if the user wants to modify an already placed furniture
		Ray ray = new Ray(lineRenderer.GetPosition(), lineRenderer.GetForward());

		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit, 1000f, furnitureMask)) {

			GameObject obj = hit.transform.gameObject;

			if(deselect != null) deselect(); // Call Deselect event: otherwise if objects overlap they all stay blue
			if(select != null) select(obj); // Call Select event

			if(Input.GetKeyDown("q")) {

				placingScript.setObject(obj, obj.name);

				// Update status: switch working scripts
				this.enabled = false;
				placingScript.enabled = true;
			}
		}
		else {
			if(deselect != null) deselect(); // Call Deselect event
		}
 
	
		if(Physics.Raycast(ray, out hit, 1000f, UImask)) {
			// Check if the user hit the menu
			GameObject obj = hit.transform.gameObject;
			
			if(menuSelect != null) menuSelect(obj);

			// check user "fire1" input
			if(Input.GetButtonDown("Fire1")) {
				if(menuPress != null) menuPress(obj);
			}
		}
		else {
			if(menuDeselect != null) menuDeselect();
		}

	}


	void OnEnable(){
		stateText.text = "Stato: Scegli";
		objToPlace = null;
	}

	void OnDisable(){
		stateText.text = "Stato: Posiziona";
	}

	/* Choose a furniture with numbers as input key */
	bool chooseFurniture(out GameObject newObject){
	
		if(Input.GetKeyDown ("1")){
			newObject = loadResource("Tavolo");
			return true;
		}

		else if(Input.GetKeyDown ("2")){
			newObject = loadResource("Lampada");
			return true;
		}

		else if(Input.GetKeyDown ("3")){
			newObject = loadResource("Comodino");
			return true;
		}

		else if(Input.GetKeyDown ("4")){
			newObject = loadResource("Quadro");
			return true;
		}

		newObject = null;
		return false;
	}

	private GameObject loadResource(string res) {

		prefabName = res;
		return Instantiate(Resources.Load("EditorPrefabs/" + res, typeof(GameObject)),
				new Vector3(0, 5, 0), Quaternion.identity) as GameObject;
	}

}
