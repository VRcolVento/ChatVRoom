using UnityEngine;
using UnityEngine.UI;

namespace DemoAV.Live{

/// <summary>
/// 	This class allows the UI ScrollView to be scrolled by the VRController.
/// 	Remember to edit the BoxCollider to well fit the button.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class VRScrollView : MonoBehaviour {
	ScrollRect scroll;
	Vector3 lastPosition;
	// How much the scroll is mitigated.
	public float divisor = 100;

	void Awake() {
		scroll = GetComponent<ScrollRect>();
	}

	/// <summary>
	/// 	OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other"> The other Collider involved in this collision. </param>
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Controller"){
			lastPosition = other.transform.position;
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Controller"){
			Vector3 difference = (other.transform.position - lastPosition) / 100;

			scroll.verticalNormalizedPosition += difference.y;
			scroll.horizontalNormalizedPosition += difference.x;

			lastPosition = other.transform.position;
		}
	}
	
}

}