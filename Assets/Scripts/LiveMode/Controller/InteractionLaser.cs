using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionLaser : MonoBehaviour {

	static LayerMask layerMask;
	// The color of the ray.
	public Color color = Color.red;
	// The thickness of the ray.
    public float thickness = 0.002f;
	// The laser and its father.
	GameObject laser, holder;
	Material laserMaterial;

	// Use this for initialization
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
		layerMask = LayerMask.GetMask("Menu Layer", "Interactable Layer");
	}
	
	// Update is called once per frame
	void Update () {
		Ray raycast = new Ray(transform.position, transform.forward);
        RaycastHit hit;
		bool bHit = Physics.Raycast(raycast, out hit, Mathf.Infinity, layerMask);

		// Show the laser if it is colliding with interactable object.
		if(bHit) {
			laser.SetActive(true);
            laser.transform.localScale = new Vector3(thickness, thickness, hit.distance);
        	laser.transform.localPosition = new Vector3(0f, 0f, hit.distance / 2f);

			// Chnage color based on hit object.
			if(hit.transform.gameObject.layer == 11)
        		laserMaterial.SetColor("_Color", Color.blue);
			else
       			laserMaterial.SetColor("_Color", color);
        }
		else
			laser.SetActive(false);
	}
}
