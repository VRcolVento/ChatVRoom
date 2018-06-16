using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
	Class to handle whenever the user wish to modify an object location/rotation
	It should be enabed during the first placement or when the user wants to make some modifications

	Handles:
		Collisions with walls and other objects
*/
public class ModifyObject : MonoBehaviour {

	public Material feasibleMat;
	public Material unfeasibleMat;

	/* Handle Collision with other Interactible Objects */
	private bool isColliding = false;
	private HashSet<GameObject> interactibleCollisionList = new HashSet<GameObject>();

	/* User modifications */
	private float rotY;
	private Quaternion final;


	/* Getters & Setters */
	public bool IsColliding{

		get { return isColliding; }
	}

	void Awake () {

		this.GetComponent<Renderer>().material = feasibleMat;
		rotY = transform.rotation.y;
		final = transform.rotation;
	}
	
	void Update() {

		// Rotate the object
		if(Input.GetButtonDown("Fire2")) {
			rotY = (rotY + 90) % 360;
			final = Quaternion.Euler(0, rotY, 0);
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, final, Time.deltaTime * 20);
	}

	void OnEnable() {
		GetComponent<Renderer>().material = feasibleMat;
		Debug.Log(rotY);
	}

	/* 
		Fare il controllo esplicito se lo script è disabilitato perché script disabilitato previene la chiamata di
		Start()
		Update()
		FixedUpdate()
		LateUpdate()
		OnGUI()
		OnDisable()
		OnEnable()
	*/
	void OnCollisionEnter(Collision collision){

		if(this.enabled){
			if(collision.gameObject.GetComponent<Interactible>() != null){
	//			Gli oggetti non hanno più InteractibleObject component
	//			Devo riscrivere quella classe in modo che sia più "statica"
	//			questa si occupa delle collisioni in posizionamento

				interactibleCollisionList.Add(collision.gameObject);

				isColliding = true;
				this.GetComponent<Renderer>().material = unfeasibleMat;
			}
		}
	}

	void OnCollisionExit(Collision collision){
		
		if(this.enabled) {
			if(collision.gameObject.GetComponent<Interactible>() != null){

				interactibleCollisionList.Remove(collision.gameObject);

				if(interactibleCollisionList.Count == 0) {
					// Check if I am not colliding with anyone anymore
					isColliding = false;
					this.GetComponent<Renderer>().material = feasibleMat;
				}
			}
		}
	}
}
