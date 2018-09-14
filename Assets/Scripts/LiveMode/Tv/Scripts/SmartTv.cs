using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

using DemoAV.Common;
using DemoAV.Live.SmarTv.SmartMenu;

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
	public GameObject loadingScreen;
	VideoPlayer player;
	AudioSource audioSource;
	const int speedIncreaseTime = 2, maxSpeed = 32;
	bool backward, forward, closeVideo;
	float speed, speedTime;
	short updates;
	// Teleport script.
	public GameObject teleport;

	/// <summary>
	/// 	Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake(){
		if(Directory.Exists(Application.persistentDataPath + "/tvSettings.json")){
			settings = new Settings();
			settings.videoFolder = settings.musicFolder = "";
		}
		else{
			settings = JsonUtility.FromJson<Settings>(File.ReadAllText(Application.persistentDataPath + "/tvSettings.json"));
		}
	}

	/// <summary>
	/// 	Start is called on the frame when a script is enabled just before
	/// 	any of the Update methods is called the first time.
	/// </summary>
	void Start () {
		// Creates component to render video.
		display = transform.Find("Display").gameObject;
		
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
		// KeyboardHandler.AddCallback(KeyboardHandler.Map.KEY_DOWN, KeyCode.Escape, menuFactory.GoBack);
		// KeyboardHandler.AddCallback(KeyboardHandler.Map.KEY_DOWN, KeyCode.D, NextTab);
		// KeyboardHandler.AddCallback(KeyboardHandler.Map.KEY_DOWN, KeyCode.A, PreviousTab);
		VRKeyHandler handler = remoteController.GetComponent<VRKeyHandler>();
		handler.AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.GRIP, PreviousMenu);
		handler.AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.AXIS0, PreviousTab);
		handler.AddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.AXIS0, NextTab);
		// handler.AddCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.AXIS0, ActivateTeleport);
	}
	
	/// <summary>
	/// 	Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
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
	}

	/// <summary>
	/// 	Returns to the previous menu.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void PreviousMenu(RaycastHit hit){
		if(hit.transform.gameObject == display || hit.transform.gameObject.layer == Menu.menuLayer)
			menuFactory.GoBack();
	}

	/// <summary>
	/// 	Goes to the previous tab in the active menu.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void PreviousTab(RaycastHit hit){
		if(hit.transform.gameObject == display || hit.transform.gameObject.layer == Menu.menuLayer){
			Vector2 axis = remoteController.GetComponent<ControllerFunctions>().GetAxis();
			// teleport.SetActive(false);

			// Left touchpad.
			if(axis.x <= -0.4 && axis.y < 0.8 && axis.y > -0.8)
				menuFactory.ChangeTab(-1);
		}
	}

	/// <summary>
	/// 	Goes to the next tab in the active menu.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void NextTab(RaycastHit hit){
		if(hit.transform.gameObject == display || hit.transform.gameObject.layer == Menu.menuLayer){
			Vector2 axis = remoteController.GetComponent<ControllerFunctions>().GetAxis();
			// teleport.SetActive(false);
		
			// Right touchpad.
			if(axis.x >= 0.4 && axis.y < 0.8 && axis.y > -0.8)
				menuFactory.ChangeTab(+1);
		}
	}

	/// <summary>
	/// 	
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void ActivateTeleport(RaycastHit hit){
		if(!player.isActiveAndEnabled &&  (hit.transform.gameObject == display || hit.transform.gameObject.layer == Menu.menuLayer))
			teleport.SetActive(true);
	}

	/// <summary>
	/// 	Starts the video.
	/// </summary>
	/// <param name="url"> The url of the video to play. </param>
	void PlayVideo(string url){
		VRKeyHandler handler = remoteController.GetComponent<VRKeyHandler>();

		// Change the menu callback with video one.
		// Pause video.
		handler.DeferredAddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, PauseVideo);
		// Forward.
		handler.DeferredAddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.AXIS0, StartForward);
		handler.DeferredAddCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.AXIS0, EndForward);
		// Backward.
		handler.DeferredAddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.AXIS0, StartBackward);
		handler.DeferredAddCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.AXIS0, EndBackward);
		// End video.
		handler.DeferredRemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.GRIP, PreviousMenu);
		handler.DeferredAddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.GRIP, EndVideo);

		player.url = url;
		StartCoroutine(StartVideo());
	}

	/// <summary>
	/// 	The callback to load the video.
	/// </summary>
	/// <returns></returns>
	IEnumerator StartVideo(){
		// Show loading screen.
		loadingScreen.SetActive(true);

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

		// Show menu.
		loadingScreen.SetActive(false);

		// Disable Menu.
		menuFactory.SetActiveMenu(null);

		//Play Sound
		audioSource.Play();

		player.Play();
	}

	/// <summary>
	/// 	Pauses and plays the video.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void PauseVideo(RaycastHit hit){
		// If the remote controller is pointing the tv.
		if(hit.transform.gameObject == display){
			if(player.isPlaying) 	player.Pause();
			else					player.Play();
		}
	}

	/// <summary>
	/// 	Starts the forwarding of the video.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void StartForward(RaycastHit hit){
		if(hit.transform.gameObject == display){
			Vector2 axis = remoteController.GetComponent<ControllerFunctions>().GetAxis();

			// Right touchpad.
			if(axis.x >= 0.4 && axis.y < 0.8 && axis.y > -0.8){
				teleport.SetActive(false);
				speed = 2;
				speedTime = 0;
				audioSource.volume = 0;
				forward = true;
			}
		}
	}

	/// <summary>
	/// 	Stops the forwarding of the video.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void EndForward(RaycastHit hit){
		if (forward)	teleport.SetActive(true);

		if (hit.transform.gameObject == display){
			Vector2 axis = remoteController.GetComponent<ControllerFunctions>().GetAxis();

			print(axis.x >= 0.4 && axis.y < 0.8 && axis.y > -0.8);
			// Right touchpad.
			if(axis.x >= 0.4 && axis.y < 0.8 && axis.y > -0.8){
				player.playbackSpeed = 1;
				audioSource.volume = 1;
				forward = false;
			}
		}
	}

	/// <summary>
	/// 	Starts the backwarding of the video.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void StartBackward(RaycastHit hit){
		if(hit.transform.gameObject == display){
			Vector2 axis = remoteController.GetComponent<ControllerFunctions>().GetAxis();

			// Left touchpad.
			if(axis.x <= -0.4 && axis.y < 0.8 && axis.y > -0.8){
				teleport.SetActive(false);
				player.Pause();
				speed = 2;
				speedTime = 0;
				updates = 0;
				backward = true;
			}
		}
	}

	/// <summary>
	/// 	Stops the backwarding of the video.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void EndBackward(RaycastHit hit){
		if (backward)	teleport.SetActive(true);	

		if (hit.transform.gameObject == display){
			Vector2 axis = remoteController.GetComponent<ControllerFunctions>().GetAxis();

			// Left touchpad.
			if(axis.x <= -0.4 && axis.y < 0.8 && axis.y > -0.8){
				player.Play();
				backward = false;
			}
		}
	}
	
	/// <summary>
	/// 	Ends the video.
	/// </summary>
	/// <param name="hit"> The object hit by raycast. </param>
	void EndVideo(RaycastHit hit){
		if(hit.transform.gameObject == display){
			VRKeyHandler handler = remoteController.GetComponent<VRKeyHandler>();

			player.Stop();
			menuFactory.GoBack();
			
			// Change the video callback with menu one.
			handler.DeferredRemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.GRIP, EndVideo);
			handler.DeferredAddCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.GRIP, PreviousMenu);
			// Delete other handlers.
			// Pause video.
			handler.DeferredRemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.TRIGGER, PauseVideo);
			// Forward.
			handler.DeferredRemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.AXIS0, StartForward);
			handler.DeferredRemoveCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.AXIS0, EndForward);
			// Backward.
			handler.DeferredRemoveCallback(VRKeyHandler.Map.KEY_DOWN, VRKeyHandler.Key.AXIS0, StartBackward);
			handler.DeferredRemoveCallback(VRKeyHandler.Map.KEY_UP, VRKeyHandler.Key.AXIS0, EndBackward);
		}
	}
}
}

