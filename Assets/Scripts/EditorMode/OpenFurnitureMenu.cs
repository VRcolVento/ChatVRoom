using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenFurnitureMenu : MonoBehaviour {

	public Canvas canvas;
	public Transform lookTarget;
	private Transform hand;
	
	private List<Vector3> buttons;
	private bool start = false;


	public float height = 0.3f;
	void Start () {
		
		hand = GameObject.Find("Mano").transform;
		transform.position = hand.position + hand.forward*7;

		buttons = new List<Vector3>();

		Transform child = transform.GetChild(0); // The child is the canvas
		for(int i=0; i<child.childCount; i++){
			buttons.Add(child.GetChild(i).localPosition);
		}		
	}
	
	void Update () {
		
		transform.LookAt(lookTarget);
		transform.position = hand.position + Vector3.up*height;

		if(Input.GetKeyDown(KeyCode.Tab)){

			if(!canvas.gameObject.activeInHierarchy) {

				canvas.gameObject.SetActive(true);
				StartCoroutine("SfogliaCoroutine");
			}
			else {				

				canvas.gameObject.SetActive(false);
				Transform child = transform.GetChild(0);				
				for(int i=0; i<buttons.Count; i++)
					child.GetChild(i).localPosition = new Vector3(-100, 0, 0);
				StopCoroutine("SfogliaCoroutine");
			}
		}
	}

	IEnumerator SfogliaCoroutine() {

		Transform child = transform.GetChild(0);
		// BRutta condizione di uscita
		while(child.GetChild(buttons.Count-1).position != buttons[buttons.Count-1] + new Vector3(50*buttons.Count-1, 0, 0)) {

			for(int i=0; i<buttons.Count; i++)
				child.GetChild(i).localPosition = Vector3.Lerp(child.GetChild(i).localPosition, buttons[i] + new Vector3(50*i, 0, 0), Time.deltaTime);

			yield return new WaitForEndOfFrame();
		}
		Debug.Log("UDIGFKUDSYGVFDSKYUGVFDSKGVFDSGVYDFVBDSJYVBSJF");
		yield return null;
	}
}
