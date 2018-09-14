using System.Collections;
using System.IO;
using UnityEngine;

namespace DemoAV.Live.Stereo{

[RequireComponent(typeof(AudioSource))]
public class Stereo : MonoBehaviour {
	AudioSource source;
	// Stereo components.
	public StereoSlider slider;

	private void Start() {
		source = GetComponent<AudioSource>();
		PlaySong("D:/ROBA FIGA, NON PER FRANCESCO/Music/test.ogg");
	}

	/// <summary>
	/// 	Plays a song sited at given path.
	/// </summary>
	/// <param name="path"> The url (or path) in which find the song </param>
	void PlaySong(string url){
		if(source.isPlaying)	source.Stop();
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
			yield return www;

		print("done loading");
		clip.name = Path.GetFileName(url);

		// Load the clips and update stereo components.
		source.clip = clip;
		if(!source.isPlaying)	source.Play();
		slider.SetNewSong((int)clip.length);
	}

	private void OnAudioFilterRead(float[] data, int channels) {
	}
}
}