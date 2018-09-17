using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DemoAV.Live{

/// <summary>
/// 	This class allows the UI ScrollView to be scrolled by the VRController.
/// 	Remember to edit the BoxCollider to well fit the button.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class VRScrollView : MonoBehaviour {
	Vector3 lastPosition;
	public Transform contentToSwipe;
	public bool swipeHorizontal, swipeVertical;
	byte currentHorizontalTab = 0, currentVerticalTab = 0;
	// The difference to activate sweep.
	public float margin = 0.05f;

	/// <summary>
	/// 	Changes the current active tab.
	/// </summary>
	/// <param name="index"> The index of the new tab. </param>
	/// <param name="horizontalSwipe"> True if the tab should be changed horizontally, false otherwise. </param>
	public void GoToTab(short index, bool horizontalSwipe){
		if (index >= 0 && index < contentToSwipe.childCount){

			// Horizontal swipe.
			if (horizontalSwipe && swipeHorizontal){
				float diff = index - currentHorizontalTab;
				diff *= contentToSwipe.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
				StartCoroutine(Transition(contentToSwipe.localPosition - new Vector3(1, 0, 0) * diff));
				currentHorizontalTab = (byte)index;
			}
			// Vertical swipe.
			else if (!horizontalSwipe && swipeVertical){
				float diff = index - currentVerticalTab;
				diff *= contentToSwipe.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
				StartCoroutine(Transition(contentToSwipe.position + new Vector3(0, 1, 0) * diff));
				currentVerticalTab = (byte)index;
			}
		}
	} 

	/// <summary>
	/// 	Starts a transition for sliding.
	/// </summary>
	/// <param name="newPos"> The position in which the transition must end. </param>
	/// <returns></returns>
	IEnumerator Transition(Vector3 newPos){
		float weight = 0;
		Vector3 start = contentToSwipe.localPosition;

		while(weight < 1){
			contentToSwipe.localPosition = Vector3.Lerp(start, newPos, weight);
			weight += 0.1f;
			yield return new WaitForSeconds(0.02f);
		}

		contentToSwipe.localPosition = newPos;
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

	/// <summary>
	/// OnTriggerStay is called once per frame for every Collider other
	/// that is touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Controller"){
			Vector3 difference = other.transform.position - lastPosition;
			Vector2 size = contentToSwipe.GetChild(0).GetComponent<RectTransform>().sizeDelta;
			print("diff: " + Mathf.Abs(difference.x) + " > " + margin);
			if (swipeHorizontal && Mathf.Abs(difference.x) > margin){
				GoToTab((short)(difference.x < 0 ? currentHorizontalTab + 1 : currentHorizontalTab - 1), true);
				lastPosition = other.transform.position;
			}
			else if(swipeVertical && Mathf.Abs(difference.y) > margin){
				GoToTab((short)(currentVerticalTab + 1), false);
				lastPosition = other.transform.position;
			}
		}
	}
	
}

}