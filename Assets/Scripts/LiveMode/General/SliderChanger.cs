using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DemoAV.Common;

public class SliderChanger : MonoBehaviour {

	Transform controller;
	public Slider slider;
	private float sliderValue;
	private Vector3 position;

	private bool pressed;
	private bool collided;


	void GrabSlider(RaycastHit hit) {
		if(collided) {
			sliderValue = slider.value;
			position = controller.localPosition;
			pressed = true;			
		}
	}

	void SgrabSlider(RaycastHit hit) {
		pressed = false;
	} 

	/// <summary>
	/// 	OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerEnter(Collider other) {
		if(other.transform.tag == "Controller") { // Cambiare metodo di identificazione slider
			collided = true;
			controller = other.transform;
			other.gameObject.GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, GrabSlider);
			other.gameObject.GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.TRIGGER, SgrabSlider);
		}
	}

	/// <summary>
	/// 	OnTriggerStay is called once per frame for every Collider other
	/// 	that is touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerStay(Collider other) {
		if(pressed) {
			// Cambiare le direzioni, dipende da dove è orientato la tua UI nel mondo
			float v = (controller.localPosition.x - position.x) * slider.maxValue;
			updateValue(v);
		}
	}

	/// <summary>
	/// 	OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerExit(Collider other) {
		if(other.transform.tag == "Controller") {
			collided = pressed = false;
			other.gameObject.GetComponent<VRKeyHandler>().RemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, GrabSlider);
			other.gameObject.GetComponent<VRKeyHandler>().RemoveCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.TRIGGER, SgrabSlider);
		}
	}

	public void updateValue(float v) {
		slider.value = Mathf.Clamp(sliderValue - v, slider.minValue, slider.maxValue);
	}
}
