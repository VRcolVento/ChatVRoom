using UnityEngine;

namespace DemoAV.StartMenu.Keyboard{
public class Keyboard : MonoBehaviour {
	public Transform user;
	public float distance = 1;

	/// <summary>
	/// 	This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()	{
		transform.position = user.position + user.forward * distance - user.up * 0.2f - user.right * 0.5f;
	}
}

}