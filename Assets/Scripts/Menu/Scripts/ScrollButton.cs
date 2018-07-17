using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollButton : MonoBehaviour, ISelectHandler, IDeselectHandler {
	GameObject image, highlightedImage;

	void Start(){
		image = transform.Find("Image").gameObject;
		highlightedImage = transform.Find("Highlighted Image").gameObject;

		// GetComponent<Button>().Select();
		// StartCoroutine(Deselect());
	}

	// IEnumerator Deselect(){
	// 	yield return new WaitForSeconds(2);
	// 	Debug.Log("Deselect");
	// 	EventSystem.current.SetSelectedGameObject(null);
	// }

	/// <summary>
	/// 	Shows white arrow on blue background.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnSelect(BaseEventData eventData){
		image.SetActive(false); highlightedImage.SetActive(true);
	}

	/// <summary>
	/// 	Shows blue arrow on transparent background.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnDeselect(BaseEventData data){
		image.SetActive(true); highlightedImage.SetActive(false);
	}
}
