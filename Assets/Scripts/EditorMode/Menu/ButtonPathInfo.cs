using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoAV.Editor.MenuUtil {

	/// <summary> Store the subfolder path of each object </summary>
	public class ButtonPathInfo : MonoBehaviour {

		private string folder;

		public string MyPath {
			get { return folder; }
			set { folder = value; }
		}
	}
}