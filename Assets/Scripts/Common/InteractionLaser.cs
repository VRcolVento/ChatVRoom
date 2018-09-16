using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DemoAV.Common{

/// <summary>
/// 	Class that spawn a laser from the controller with which is 
/// 	possible to interact with UI or other UI-like object.
/// </summary>
public class InteractionLaser : MonoBehaviour {

	static LayerMask layerMask;
	// The color of the ray.
	public Color color = Color.red;
	// The thickness of the ray.
    public float thickness = 0.002f;
	// The laser and its father.
	GameObject laser, holder;
	// The last hit object.
	GameObject lastHit;
	Material laserMaterial;

	public GameObject lastHitObject{
		get{ return lastHit; }
	}

	/// <summary>
	/// 	Start is called on the frame when a script is enabled just before
	/// 	any of the Update methods is called the first time.
	/// </summary>
	void Start () {
		// Create laser as thick cube and hide it.
		holder = new GameObject();
        holder.transform.SetParent(transform);
        holder.transform.localPosition = Vector3.zero;
		holder.transform.localRotation = Quaternion.identity;

		laser = GameObject.CreatePrimitive(PrimitiveType.Cube);
        laser.transform.SetParent(holder.transform);
        laser.transform.localScale = new Vector3(thickness, thickness, 100f);
        laser.transform.localPosition = new Vector3(0f, 0f, 50f);
		laser.transform.localRotation = Quaternion.identity;

		laserMaterial = new Material(Shader.Find("Unlit/Color"));
        laserMaterial.SetColor("_Color", color);
        laser.GetComponent<MeshRenderer>().material = laserMaterial;
		laser.SetActive(false); // Hide the laser.

		// Set the hitable layers.
		layerMask = LayerMask.GetMask("Menu Layer", "Interactable Layer", "Selectable Button Menu Layer");

		// Set VR click.
		GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, PressButton);
	}
	
	/// <summary>
	/// 	Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update () {
		Ray raycast = new Ray(transform.position + transform.forward * 0.2f, transform.forward);
        RaycastHit hit;
		bool bHit = Physics.Raycast(raycast, out hit, Mathf.Infinity, layerMask);

		// Show the laser if it is colliding with interactable object.
		if(bHit) {
			laser.SetActive(true);
            laser.transform.localScale = new Vector3(thickness, thickness, hit.distance);
        	laser.transform.localPosition = new Vector3(0f, 0f, hit.distance / 2f);

			HandleInteraction(hit);
        }
		else{
			laser.SetActive(false);
			if(lastHit != null){
				EventSystem.current.SetSelectedGameObject(null);
				lastHit = null;
			}
		}
	}

	/// <summary>
	/// 	Handles the interaction with the current pointed element.
	/// </summary>
	/// <param name="hit"> The element hit by raycast. </param>
	void HandleInteraction(RaycastHit hit){
		GameObject hitObject = hit.transform.gameObject;

		// Selectable UI element.
		if(hitObject.layer == 13){
			if(hitObject != lastHit){
				// Deselect old and select new one.
				EventSystem.current.SetSelectedGameObject(null);
				hitObject.GetComponent<Button>().Select();
			}
		}
		else if(lastHit != null && lastHit.layer == 13){
			EventSystem.current.SetSelectedGameObject(null);
		}

		// Change color based on hit object.
		if(hitObject.layer == 11 || hitObject.layer == 13)
			laserMaterial.SetColor("_Color", Color.blue);
		else
			laserMaterial.SetColor("_Color", color);

		// Update lasthit.
		if (lastHit != hitObject)	lastHit = hitObject;
	}

	/// <summary>
	/// 	Acivate the button hit by raycast.
	/// </summary>
	/// <param name="hit"> The button hit by raycast. </param>
	void PressButton(RaycastHit hit){
		if(hit.transform.gameObject.layer == 13 && EventSystem.current.currentSelectedGameObject)
			EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
	}

	/// <summary>
	/// 	This function is called when the behaviour becomes disabled or inactive.
	/// </summary>
	void OnDisable(){
		GetComponent<VRKeyHandler>().RemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, PressButton);
	}
}

}