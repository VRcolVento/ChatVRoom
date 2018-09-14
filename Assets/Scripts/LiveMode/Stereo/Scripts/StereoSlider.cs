using UnityEngine;
using UnityEngine.UI;

namespace DemoAV.Live.Stereo{

public class StereoSlider : MonoBehaviour {
	public Text maxDuration;
	int currentDuration;

	/// <summary>
	/// 	Sets a new song for the slider.
	/// </summary>
	/// <param name="duration"> The duration of the new song. </param>
	public void SetNewSong(int duration){
		currentDuration = duration;
		maxDuration.text = FormatDuration(duration);
	}

	/// <summary>
	/// 	Formats the duration of the song as m:ss.
	/// </summary>
	/// <param name="duration"> The duration of the new song. </param>
	/// <returns> The duration formatted as m:ss. </returns>
	string FormatDuration(int duration){
		string format = "";

		int minutes = duration / 100;
		format += minutes + ":";
		format += duration - minutes * 100;

		return format;
	}
	
}
}