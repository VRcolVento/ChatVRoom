using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

using DemoAV.Common;
using DemoAV.SmartMenu;

namespace DemoAV.Live.SmarTv{
public class SmartTv : MonoBehaviour {
	// Settings.
	[Serializable]
	struct Settings{
		public string videoFolder, musicFolder; 
	};
	Settings settings;

	// Remote controller.
	public GameObject remoteController;

	// List of apps.
	List<ITvApp> apps;

	// Shortcut to objects.
	public GameObject panel;
	TvMenuFactory menuFactory;
	GameObject display;

	// Video player variables.
	VideoPlayer player;
	AudioSource audioSource;
	const int speedIncreaseTime = 2, maxSpeed = 32;
	bool backward, forward, closeVideo;
	float speed, speedTime;
	short updates;


	void Awake(){
		if(Directory.Exists(Application.persistentDataPath + "/tvSettings.json")){
			settings = new Settings();
			settings.videoFolder = settings.musicFolder = "";
		}
		else{
			settings = JsonUtility.FromJson<Settings>(File.ReadAllText(Application.persistentDataPath + "/tvSettings.json"));
		}
	}

	// Use this for initialization
	void Start () {
		// Creates component to render video.
		display = transform.Find("Display").gameObject;
		
		closeVideo = false;
		RenderTexture texture = new RenderTexture(1024, 720, 24);
		display.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
		audioSource = display.GetComponent<AudioSource>();
		player = display.AddComponent<VideoPlayer>();

		player.playOnAwake = false;
		audioSource.playOnAwake = false;

		//Set Audio Output to AudioSource
		player.audioOutputMode = VideoAudioOutputMode.AudioSource;

		// Cretes panel menu.
		menuFactory = GetComponent<TvMenuFactory>();
		apps = new List<ITvApp>{ new TvLocalStreaming(menuFactory, PlayVideo, settings.videoFolder), 
								 new TvTwitter(display, menuFactory) };

		Menu currMenu = menuFactory.CreateMenu(TvMenuFactory.Type.PANEL_MENU, "main");

		foreach(ITvApp app in apps)
			currMenu.AddMenuItem(new Menu.MenuItem(app.GetName(), app.GetTexture(), null), app.ItemCallback);

		menuFactory.SetActiveMenu("main");

		// Go back in Menu view.
		KeyboardHandler.AddCallback(KeyboardHandler.Map.KEY_DOWN, KeyCode.Escape, menuFactory.GoBack);
		KeyboardHandler.AddCallback(KeyboardHandler.Map.KEY_DOWN, KeyCode.D, NextTab);
		KeyboardHandler.AddCallback(KeyboardHandler.Map.KEY_DOWN, KeyCode.A, PreviousTab);
	}
	
	// Update is called once per frame
	void Update () {
		// Go forward.
		if(forward){
			player.playbackSpeed = speed;

			// Speed up.
			speedTime += Time.deltaTime;
			if(speedTime > speedIncreaseTime && speed < maxSpeed){
				speed *= 2;
				speedTime = 0;
			}
		}
		// Go backward.
		else if(backward){
			updates++;
			speedTime += Time.deltaTime;
			
			if(updates >= 30){
				updates = 0;
				player.time = player.time - 0.25 * speed;
				player.Play();
				player.Pause();
			}
			if(speedTime > speedIncreaseTime && speed < maxSpeed){
				speed *= 2;
				speedTime = 0;
			}
		}

		// Stop player.
		if(closeVideo){
			player.Stop();
			menuFactory.GoBack();
			closeVideo = false;
			
			// Change the video callback with menu one.
			KeyboardHandler.RemoveCallback(KeyboardHandler.Map.KEY_DOWN, KeyCode.Escape, CloseVideo);
			KeyboardHandler.AddCallback(KeyboardHandler.Map.KEY_DOWN, KeyCode.Escape, menuFactory.GoBack);
		}
	}

	/// <summary>
	/// 	Goes to the previous tab in the active menu.
	/// </summary>
	void PreviousTab(){
		menuFactory.ChangeTab(-1);
	}

	/// <summary>
	/// 	Goes to the next tab in the active menu.
	/// </summary>
	void NextTab(){
		menuFactory.ChangeTab(+1);
	}

	/// <summary>
	/// 	Plays the video.
	/// </summary>
	/// <param name="url"> The url of the video to play. </param>
	void PlayVideo(string url){
		VRKeyHandler handler = remoteController.GetComponent<VRKeyHandler>();

		// Change the menu callback with video one.
		// KeyboardHandler.RemoveCallback(KeyboardHandler.Map.KEY_DOWN, KeyCode.Escape, menuFactory.GoBack);
		// KeyboardHandler.AddCallback(KeyboardHandler.Map.KEY_DOWN, KeyCode.Escape, CloseVideo);

		// Pause video.
		handler.AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, PauseVideo);
		// Forward.
		handler.AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.AXIS0, StartForward);
		handler.AddCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.AXIS0, EndForward);
		// Backward.
		handler.AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.AXIS0, StartBackward);
		handler.AddCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.AXIS0, EndBackward);

		player.url = url;
		StartCoroutine(StartVideo());
	}

	/// <summary>
	/// 	The callback to load the video.
	/// </summary>
	/// <returns></returns>
	IEnumerator StartVideo(){
		//Assign the Audio from Video to AudioSource to be played
		player.EnableAudioTrack(0, true);
		player.SetTargetAudioSource(0, audioSource);

		audioSource.volume = 1.0f;
		player.controlledAudioTrackCount = 1;
		player.Prepare();

		//Wait until video is prepared
		while (!player.isPrepared)
		{
			Debug.Log("Preparing Video");
			yield return null;
		}

		Debug.Log("Done Preparing Video");

		// Disable Menu.
		menuFactory.SetActiveMenu(null);

		//Play Sound
		audioSource.Play();

		player.Play();
	}

	void PauseVideo(RaycastHit hit){
		// If the remote controller is pointing the tv.
		if(hit.transform.gameObject == gameObject){
			if(player.isPlaying) 	player.Pause();
			else					player.Play();
		}
	}

	void StartForward(RaycastHit hit){
		Vector2 axis = remoteController.GetComponent<ControllerFunctions>().GetAxis();

		// Right touchpad.
		if(axis.x >= 0.4 && axis.y < 0.8 && axis.y > -0.8){
			speed = 2;
			speedTime = 0;
			audioSource.volume = 0;
			forward = true;
		}
	}

	void EndForward(RaycastHit hit){
		Vector2 axis = remoteController.GetComponent<ControllerFunctions>().GetAxis();

		// Right touchpad.
		if(axis.x >= 0.4 && axis.y < 0.8 && axis.y > -0.8){
			player.playbackSpeed = 1;
			audioSource.volume = 1;
			forward = false;
		}
	}

	void StartBackward(RaycastHit hit){
		Vector2 axis = remoteController.GetComponent<ControllerFunctions>().GetAxis();

		// Left touchpad.
		if(axis.x <= -0.4 && axis.y < 0.8 && axis.y > -0.8){
			player.Pause();
			speed = 2;
			speedTime = 0;
			updates = 0;
			backward = true;
		}
	}

	void EndBackward(RaycastHit hit){
		Vector2 axis = remoteController.GetComponent<ControllerFunctions>().GetAxis();

		// Left touchpad.
		if(axis.x <= -0.4 && axis.y < 0.8 && axis.y > -0.8){
			player.Play();
			backward = false;
		}
	}
	

	void CloseVideo(){
		closeVideo = true;
	}
}
}

