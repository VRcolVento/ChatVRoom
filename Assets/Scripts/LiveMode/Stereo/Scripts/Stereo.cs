using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

using DemoAV.Common;
using DemoAV.Live.Stereo.System;

namespace DemoAV.Live.Stereo{

/// <summary>
/// 	The class that manage all main stereo functionalities.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Stereo : MonoBehaviour {
	AudioSource source;
	// Stereo components.
	public StereoSlider slider;
	public SystemManager system;
	// Duration variable.
	int lastTime = 0;

	/// <summary>
	/// 	Start is called on the frame when a script is enabled just before
	/// 	any of the Update methods is called the first time.
	/// </summary>
	void Start() {
		source = GetComponent<AudioSource>();
	}

	void Update() {
		if(source.isPlaying && (int)source.time != lastTime){
			lastTime = (int)source.time;
			slider.ChangeTime(lastTime);
		}
	}

	/// <summary>
	/// 	Plays a song sited at given path.
	/// </summary>
	/// <param name="path"> The url (or path) in which find the song </param>
	public void PlayNewSong(string url){
		if(source.isPlaying){
			source.Stop();
			slider.ChangeTime(0);
			lastTime = 0;
		}

		StartCoroutine(LoadFile(url));
	}

	/// <summary>
	/// 	
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	IEnumerator LoadFile(string url){
		WWW www = new WWW("file://" + url);
		print("loading " + url);

		AudioClip clip = www.GetAudioClip(false);
		while(!clip.isReadyToPlay)
			yield return null;

		print("done loading");
		clip.name = Path.GetFileName(url);

		// Load the clips and update stereo components.
		source.clip = clip;
		if(!source.isPlaying)	source.Play();
		slider.SetNewSong((int)clip.length);
	}

	/// <summary>
	/// 	If the song is playing, pauses it, otherwise plays it.
	/// </summary>
	public void PauseSong(){
		if(source.isPlaying)	source.Pause();
		else					source.Play();
	}

	/// <summary>
	/// 	Switches between 3D and 2D audio.
	/// </summary>
	public void ChangeSpatialBlend(){
		source.spatialBlend = 1 - source.spatialBlend;
	}

	/// <summary>
	/// 	Changes the audio according to a slider.
	/// </summary>
	/// <param name="slider"> The slider from which the value is caught. </param>
	public void ChangeVolume(Slider slider){
		source.volume = slider.value;
	}

	/// <summary>
	/// 	Changes the current playing time of the song.
	/// </summary>
	/// <param name="time"> The new time for the song. </param>
	public void ChangeCurrentTime(float time){
		source.time = time;
	}

	/// <summary>
	/// 	Opens the stereo interface.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	public void Open(RaycastHit hit){
		transform.Find("Stereo Interface").gameObject.SetActive(true);
	}

	/// <summary>
	/// 	Closes the stereo interface.
	/// </summary>
	public void Close(){
		system.gameObject.SetActive(false);
		transform.Find("Stereo Interface").gameObject.SetActive(false);
	}

	/// <summary>
	/// 	OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other"> The other Collider involved in this collision. </param>
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Controller")
			other.gameObject.GetComponent<VRKeyHandler>().AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, Open);
	}

	/// <summary>
	/// 	OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Controller")
			other.gameObject.GetComponent<VRKeyHandler>().RemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, Open);
	}
}
}