using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DemoAV.Common;

public class SliderChanger : MonoBehaviour {

	public Transform controller;

	public Slider slider;
	private float sliderValue;
	private Vector3 position;

	private bool pressed;
	private bool collided;

	
	// Update is called once per frame
	void Update () {


		// if(Controller.GetHairTriggerDown() && collided) {
		// 	sliderValue = slider.value;
		// 	direction = transform.forward;
		// 	pressed = true;
		// }

		MoveSlider();

		// if(Controller.GetHairTriggerUp()){
		// 	pressed = false;
		// }
	}

	void GrabSlider(RaycastHit hit) {

		if(collided) {
			sliderValue = slider.value;
			position = controller.position;
			pressed = true;			
		}
	}

	void MoveSlider() {

		if(pressed) {
			// Cambiare le direzioni, dipende da dove è orientato la tua UI nel mondo
			float v = (controller.position.x - position.x) * slider.maxValue;
			updateValue(v);
		}
	}

	void SgrabSlider(RaycastHit hit) {
		pressed = false;
	} 

	void OnTriggerEnter(Collider other) {

		if(other.transform.tag == "Controller") { // Cambiare metodo di identificazione slider
			collided = true;
			other.gameObject.GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, GrabSlider);
			other.gameObject.GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.TRIGGER, SgrabSlider);
		}
	}

	void OnTriggerExit(Collider other) {
		
		if(other.transform.tag == "Controller") {
			collided = false;
			other.gameObject.GetComponent<VRKeyHandler>().RemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, GrabSlider);
			other.gameObject.GetComponent<VRKeyHandler>().RemoveCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.TRIGGER, SgrabSlider);
		}
	}

	public void updateValue(float v) {
		slider.value = Mathf.Clamp(sliderValue - v, slider.minValue, slider.maxValue);
	}
}
