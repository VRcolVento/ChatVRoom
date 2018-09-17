using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	Grab an UI slider and change value
 */
public class MoveHandle : MonoBehaviour {

	// The steam VR controller
	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	public Slider mySlider;

	private float sliderValue;
	private Vector3 direction;

	private bool pressed;
	private bool collided;

	void Start() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	void Update () {
		if(Controller.GetHairTriggerDown() && collided) {
			sliderValue = mySlider.value;
			direction = transform.forward;
			pressed = true;
		}

		if(pressed) {
			// Cambiare le direzioni, dipende da dove è orientato la tua UI nel mondo
			float v = (transform.forward.x - direction.x) * mySlider.maxValue;
			updateValue(v);
		}

		if(Controller.GetHairTriggerUp()){
			pressed = false;
		}
	}

	public void updateValue(float v) {
		mySlider.value = Mathf.Clamp(sliderValue - v, mySlider.minValue, mySlider.maxValue);
	}

	void OnTriggerEnter(Collider other) {
		if(other.transform.name == "Handle") // Cambiare metodo di identificazione slider
			collided = true;
	}

	void OnTriggerExit(Collider other) {
		
		if(collided)
			collided = false;
	}
}
