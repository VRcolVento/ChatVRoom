using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoAV.Editor.SceneControl;

namespace DemoAV.Editor.StorageUtility {

	/// <summary>
	/// Class describing a dictionary's entity. 
	/// </summary>
	public class DictionaryEntity : MonoBehaviour {

		int id = -1; 
		public int ID{
			get{ return id; }
			set{ this.id = value; }
		}

		/// <summary>
		/// Update object's enitity in the dictionary
		/// <para name="id">The object's id</para>
		/// <para name="position">The object's position</para>
		/// <para name="rotation">The object's rotation</para>
		/// </summary>
		public void AddEntity(int id, Vector3 position, Quaternion rotation) {
			UpdatePosition(position);
			UpdateRotation(rotation);
		}

		/// <summary>
		/// Add an entity
		/// <para name="path">The object's (sub)path</para>
		/// <para name="name">The object's name</para>
		/// <para name="position">The object's position</para>
		/// <para name="rotation">The object's rotation</para>
		/// </summary>
		public void AddEntity (string path, string prefabName, Vector3 position, Quaternion rotation) {			
			id = SceneController.Dictionary.AddEntity(path, prefabName, position, rotation);
		}

		/// <summary>
		/// Remove an entity
		/// <para name="id">The object's id</para>
		/// </summary>
		public void RemoveEntity (int id) {
			SceneController.Dictionary.RemoveEntity(id);
		}

		// Helpers
		private void UpdatePosition (Vector3 position) {
			SceneController.Dictionary.UpdatePosition(id, position);
		}

		private void UpdateRotation (Quaternion rotation) {
			SceneController.Dictionary.UpdateRotation(id, rotation);
		}
	}
}
