using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DemoAV.Live.Stereo.System{

public class SystemManager : MonoBehaviour {
	// Folder prefab.
	public GameObject folder;
	// Shortcut to children.
	Transform content;
	GameObject parentFolder;
	// Stack of previous directory.
	Stack<string> history = new Stack<string>();
	string currentDir = null;

	/// <summary>
	/// 	Start is called on the frame when a script is enabled just before
	/// 	any of the Update methods is called the first time.
	/// </summary>
	void Start() {
		content = transform.Find("Scroll View/Viewport/Content");
		parentFolder = content.Find("Parent Folder").gameObject;
		ChangeDirectory("C:/Users/giuli/Music/");
	}

	/// <summary>
	/// 	Switch between opened and closed state.
	/// </summary>
	public void Switch(){
		if (gameObject.activeSelf)	Close();
		else						Open();
	}

	/// <summary>
	/// 	Opens the directory interface.
	/// </summary>
	public void Open(){
		gameObject.SetActive(true);
	}

	/// <summary>
	/// 	Closes the directory interface.
	/// </summary>
	public void Close(){
		gameObject.SetActive(false);
	}

	/// <summary>
	/// 	Changes the current directory.
	/// </summary>
	/// <param name="directory"> The new directory. </param>
	public void ChangeDirectory(string directory) {
		// Go backward.
		if (directory == "" && history.Count > 0){
			currentDir = history.Pop();
		}
		// Go forward.
		else if (directory != "" && directory != currentDir){
			if (currentDir != null)		history.Push(currentDir);
			currentDir = directory;
		}

		// Update visualized directories.
		if (currentDir != null){
			IEnumerable<string> dirs = Directory.GetDirectories(currentDir)
												.Where(d => SongsList.GetSupportedFiles(d).Count() > 0 || Directory.GetDirectories(d).Count() > 0);

			// Hide / Show parent directory option.
			if (history.Count > 0)	parentFolder.SetActive(true);
			else					parentFolder.SetActive(false);

			// Clear previous dir and add new ones.
			foreach (Transform child in content)
				if (child.gameObject.name != "Parent Folder")
					Destroy(child.gameObject);
			
			foreach (string dir in dirs)
				AddNewDirectory(dir);
		}
	}

	/// <summary>
	/// 	Adds a new directory to the interface.
	/// </summary>
	/// <param name="dir"> The path of the new directory. </param>
	void AddNewDirectory(string dir){
		string dirName = Path.GetFileName(dir);
		GameObject newDir = Instantiate(folder);

		newDir.transform.SetParent(content);
		newDir.transform.localPosition = Vector3.zero;
		newDir.transform.localRotation = Quaternion.identity;
		newDir.transform.localScale = new Vector3(1, 1, 1);

		newDir.name = dirName;
		newDir.transform.Find("Name").GetComponent<Text>().text = dirName;
		newDir.GetComponent<Button>().onClick.AddListener(() => { ChangeDirectory(dir); });
	}
}

}