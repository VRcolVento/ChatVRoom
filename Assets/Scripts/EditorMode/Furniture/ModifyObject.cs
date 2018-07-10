using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
	Class to handle whenever the user wish to modify an object location/rotation
	It should be enabed during the first placement or when the user wants to make some modifications

	Handles:
		Collisions with walls and other objects
*/

namespace DemoAV.Editor.ObjectUtil {

	/// <summary>
	/// Class to handle the object modification by the user.
	/// This class checks if the object is colliding with other objects.
	/// </summary>
	public class ModifyObject : MonoBehaviour {

		// Object materials during placement		
		public Material feasibleMat;
		public Material unfeasibleMat;

		// Handle Collision with other Interactible Objects
		private bool isColliding = false;
		private HashSet<GameObject> interactibleCollisionList = new HashSet<GameObject>();

		// User modifications
		private Quaternion final;


		// Getters & Setters
		public bool IsColliding{

			get { return isColliding; }
		}

		void Start () {

//			GetComponent<Renderer>().material = feasibleMat;
			final = transform.rotation;
		}
		
		void Update() {

			transform.rotation = Quaternion.Slerp(transform.rotation, final, Time.deltaTime * 20);
		}

		void OnEnable() {
			GetComponent<Renderer>().material = feasibleMat;
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

		/// <summary>
		/// Rotate the oject.
		/// <para name="clockwise">1 if clockwise, -1 otherwise</para>
		/// </summary>
		public void RotateObject(int clockwise) {

			Vector3 rot = transform.rotation.eulerAngles;
			final = Quaternion.Euler(new Vector3(rot.x, rot.y, (rot.z + 90*clockwise) % 360)); // Because the downloaded models are z-up
		}
	}
}