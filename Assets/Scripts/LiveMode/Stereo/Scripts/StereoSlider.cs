using UnityEngine;
using UnityEngine.UI;

namespace DemoAV.Live.Stereo{

public class StereoSlider : MonoBehaviour {
	Slider slider;
	// Stereo.
	public Stereo stereo;
	// Information about duration.
	public Text maxDurationTxt, currentDurationTxt;
	int maxDuration;

	void Awake() {
		slider = GetComponent<Slider>();
	}

	/// <summary>
	/// 	Sets a new song for the slider.
	/// </summary>
	/// <param name="duration"> The duration of the new song. </param>
	public void SetNewSong(int duration){
		maxDuration = duration;
		slider.maxValue = maxDuration;
		maxDurationTxt.text = FormatDuration(duration);
	}

	/// <summary>
	/// 	Formats the duration of the song as m:ss.
	/// </summary>
	/// <param name="duration"> The duration of the new song. </param>
	/// <returns> The duration formatted as m:ss. </returns>
	string FormatDuration(int duration){
		string format = "";

		int minutes = duration / 60;
		format += minutes + ":";

		int seconds = duration - minutes * 60;
		if(seconds < 10)	format += "0";
		format += seconds;

		return format;
	}

	/// <summary>
	/// 	Changes the slider position and the displayed current time.
	/// </summary>
	/// <param name="currTime"> The current time of the song. </param>
	public void ChangeTime(int currTime){
		if(currTime < maxDuration){
			currentDurationTxt.text = FormatDuration(currTime);
			slider.value = currTime;
		}
	}

	/// <summary>
	/// 	Updates the time of the playing song according to the slider position.
	/// </summary>
	public void UpdateTime(){
		stereo.ChangeCurrentTime(slider.value);
	}
	
}
}