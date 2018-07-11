using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DemoAV.Common;

public class NotificationMenuMiniCanvas : MonoBehaviour {
	static Vector3 bigSize = new Vector3(0.004f, 0.001f, 1), smallSize;
	Rigidbody body;
	FixedJoint fx;
	bool isGrabbed = false;
	GameObject toucher = null;

	// Use this for initialization
	void Awake () {
		body = GetComponent<Rigidbody>();
		GetComponent<BoxCollider>().size = GetComponent<RectTransform>().sizeDelta;
		smallSize = transform.localScale;
	}

	void Start() {
		GameObject.Find("RightController").GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, GrabCanvas);
		GameObject.Find("RightController").GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.TRIGGER, ReleaseCanvas);
	}

	/// <summary>
	/// 	Binds the object to a grabber.
	/// </summary>
	/// <param name="grabber"> The object that has grabbed the text canvas. </param>
	public void Grab(GameObject grabber){
		// Add a Fixed joint and start popup effect.
		body.isKinematic = false;
		fx = gameObject.AddComponent<FixedJoint>();
		fx.breakForce = 20000;
    	fx.breakTorque = 20000;
		fx.connectedBody = grabber.GetComponent<Rigidbody>();
		isGrabbed = true;
		
		StartCoroutine(PopUpEffect(transform.localScale, smallSize));
	}

	/// <summary>
	/// 	Release the object and set the current one as its new location.
	/// </summary>
	public void Release(){
		// Remove fixed joint and start popup effect.
		body.isKinematic = true;
		isGrabbed = false;

		Destroy(fx);
		StartCoroutine(PopUpEffect(transform.localScale, bigSize));
	}

	/// <summary>
	/// 	The popup effect.
	/// </summary>
	/// <returns></returns>
	IEnumerator PopUpEffect(Vector3 initSize, Vector3 finalSize){
		float weight = 0;
		RectTransform rect = GetComponent<RectTransform>();
		Transform textTr = transform.Find("Layout/Notification title");
		Vector3 initScale = textTr.localScale;
		Vector3 finalScale = new Vector3(1, finalSize == bigSize ? 2 : 10, 1);

		// Increase canvas size until it is at maximum.
		while(weight < 1){
			weight += 0.1f;
			transform.localScale = Vector3.Lerp(initSize, finalSize, weight);
			textTr.localScale = Vector3.Lerp(initScale, finalScale, weight);
			yield return new WaitForSeconds(0.05f);
		}

		transform.Find("Layout/Notification description").gameObject.SetActive(finalSize == bigSize);
	}

	
	/// <summary>
	/// 	The wrapper to grab function for key handler.
	/// </summary>
	/// <param name="hit"></param>
	void GrabCanvas (RaycastHit hit) {
		if(toucher)
			Grab(toucher);
	}

	/// <summary>
	/// 	The wrapper to release function for key handler.
	/// </summary>
	/// <param name="hit"></param>
	void ReleaseCanvas (RaycastHit hit) {
		if(isGrabbed)
			Release();
	}

	void OnTriggerEnter(Collider collider){
		toucher = collider.gameObject;
	}

	void OnTriggerExit(){
		toucher = null;
	}

	void OnDisable() {
		GameObject.Find("RightController").GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, GrabCanvas);
		GameObject.Find("RightController").GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.TRIGGER, ReleaseCanvas);
	}
}
