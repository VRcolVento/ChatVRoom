using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DemoAV.Live.Stereo{
	
public class SongsList : MonoBehaviour {
	// Supported extensions.
	static readonly IEnumerable<string> supportedExtensions = new ReadOnlyCollection<string> (new List<string>{".ogg", ".wav"});
	// External references.
	public Stereo stereo;
	public GameObject songButton;
	public GameObject songPanel;
	// Shortcut to child.
	Transform content;

	/// <summary>
	/// 	Start is called on the frame when a script is enabled just before
	/// 	any of the Update methods is called the first time.
	/// </summary>
	void Start() {
		content = transform.Find("Scroll View/Viewport/Content/List");
		ChangeCurrentDir("D:/ROBA FIGA, NON PER FRANCESCO/Music/Caparezza/Le Dimensioni del Mio Caos/");
	}

	/// <summary>
	/// 	Changes the current directory in which search for the songs.
	/// </summary>
	/// <param name="newDir"> The new directory. </param>
	public void ChangeCurrentDir(string newDir){
		if (Directory.Exists(newDir)) {
			// Deletes all children.
			foreach (Transform child in content)
				if (child.gameObject.name != "Navigator")
					Destroy(child.gameObject);

			// Search for supported files.
			IEnumerable<string> files = GetSupportedFiles(newDir);

			int filesPerTab = (int)Math.Floor(songPanel.GetComponent<RectTransform>().sizeDelta.y / songButton.GetComponent<RectTransform>().sizeDelta.y);
			int i = 0, numOfTab = 0;
			Transform currentTab = null;
			print(filesPerTab);
			foreach (string file in files){
				if (i++ / filesPerTab >= numOfTab){
					currentTab = AddTab();
					numOfTab++;
				}

				AddSong(file, currentTab);
			}
		}
	}

	/// <summary>
	/// 	Returns a list of supported music files in a directory.
	/// </summary>
	/// <param name="dir"> The directory in which search. </param>
	/// <returns> The list of supported files. </returns>
	public static IEnumerable<string> GetSupportedFiles(string dir){
		return Directory.GetFiles(dir)
						.Where(s => supportedExtensions.Contains(Path.GetExtension(s)));
	}

	/// <summary>
	/// 	Adds a new tab to the song list.
	/// </summary>
	/// <returns> The transform of the created tab. </returns>
	Transform AddTab(){
		Transform newTab = Instantiate(songPanel).transform;
		newTab.SetParent(content);
		newTab.transform.localPosition = Vector3.zero;
		newTab.transform.localRotation = Quaternion.identity;
		newTab.transform.localScale = new Vector3(1, 1, 1);

		return newTab;
	}

	/// <summary>
	/// 	Adds a new song to the list of songs.
	/// </summary>
	/// <param name="songName"> The name of the new song. </param>
	/// <param name="father"> The panel to which attach the song. </param>
	void AddSong(string songPath, Transform father){
		GameObject newSong = Instantiate(songButton);
		string songName = Path.GetFileNameWithoutExtension(songPath);
		
		newSong.transform.SetParent(father);
		newSong.transform.localPosition = Vector3.zero;
		newSong.transform.localRotation = Quaternion.identity;
		newSong.transform.localScale = new Vector3(1, 1, 1);

		newSong.transform.Find("Title").GetComponent<Text>().text = songName;
		newSong.name = songName;
		newSong.GetComponent<Button>().onClick.AddListener(() => { PlaySong(songPath); });
	}

	/// <summary>
	/// 	Changes the current playing song.
	/// </summary>
	/// <param name="songPath"> The path of the new song. </param>
	public void PlaySong(string songPath){
		stereo.PlayNewSong(songPath);
	}
}

}