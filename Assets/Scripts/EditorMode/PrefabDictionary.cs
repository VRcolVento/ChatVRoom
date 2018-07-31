using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using DemoAV.Editor.ObjectUtil;

namespace DemoAV.Editor.StorageUtility {

	/// <summary>
	///     Class to handle object's storage
	/// </summary>     
    public class PrefabDictionary : MonoBehaviour {

        /// <summary>
        /// Class describing an object entity. It stores position and rotation of the object
        /// </summary>
        [System.Serializable]
        private class Entity{

            // Name of the object            
            public string prefabName;
            [SerializeField]
            // (sub)-Path of the object            
            private string path;
            [SerializeField]
            // Position of the object            
            private float[] _position = new float[3];
            [SerializeField]
            // Rotation of the object            
            private float[] _rotation = new float[4];
            [SerializeField]

            public Entity(string path, string name, Vector3 position, Quaternion rotation){
                this.path = path;
                this.prefabName = name;
                this.position = position;
                this.rotation = rotation;
            }

            public Vector3 position{
                get{
                    return new Vector3(this._position[0], this._position[1], this._position[2]);
                }
                set{
                    this._position[0] = value.x;
                    this._position[1] = value.y;
                    this._position[2] = value.z;
                }
            }

            public Quaternion rotation{
                get{
                    return new Quaternion(this._rotation[0], this._rotation[1], this._rotation[2], this._rotation[3]);
                }
                set{
                    this._rotation[0] = value.x;
                    this._rotation[1] = value.y;
                    this._rotation[2] = value.z;
                    this._rotation[3] = value.w;

                }
            }

            public string Path {
                get {return path;}
                set {this.path = value;}
            }
        }
        ///

        private string _Name = "SignoraStanza";
        // Incremental ID
        private int currId;
        private Dictionary<int, Entity> dictionary;
        
        void Awake(){
//            DontDestroyOnLoad(this);
            currId = 0;
            dictionary = new Dictionary<int, Entity>();
        }

        public string Name{
            get{ return _Name; }
            set{ if(_Name == null) this._Name = value; }
        }

		/// <summary>
		/// Add an entity to the dictionary. Overload in case that entity already exists
		/// <para name="id">The object's id</para>
		/// <para name="path">The object's (sub)path</para>
		/// <para name="name">The object's name</para>
		/// <para name="position">The object's position</para>
		/// <para name="rotation">The object's rotation</para>
		/// </summary>
        public void AddEntity(int id, string path, string name, Vector3 position, Quaternion rotation) {
            dictionary[id] = new Entity(path, name, position, rotation);
        }

		/// <summary>
		/// Add an entity to the dictionary.
		/// <para name="path">The object's (sub)path</para>
		/// <para name="name">The object's name</para>
		/// <para name="position">The object's position</para>
		/// <para name="rotation">The object's rotation</para>
		/// </summary>
        public int AddEntity(string path, string name, Vector3 position, Quaternion rotation){
            dictionary.Add(currId, new Entity(path, name, position, rotation));
            return currId++;
        }

		/// <summary>
		/// Remove an object from the dictionary
		/// <para name="id">The object's id</para>
		/// </summary>
        public void RemoveEntity(int id) {
            dictionary.Remove(id);
        }

		/// <summary>
		/// Update object position
		/// <para name="id">The object's id</para>
		/// <para name="position">The new object's position</para>
		/// </summary>
        public void UpdatePosition(int id, Vector3 position){
            dictionary[id].position = position;
        }

		/// <summary>
		/// Update object rotation
		/// <para name="id">The object's id</para>
		/// <para name="rotation">The new object's rotation</para>
		/// </summary>
        public void UpdateRotation (int id, Quaternion rotation) {
            dictionary[id].rotation = rotation;
        }

		/// <summary>
		/// Save the scene in the dictionary
		/// </summary>
        public void Save(){
            BinaryFormatter binary  = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/Room_" + _Name + ".dat");
            Entity[] entities = new Entity[dictionary.Count];
            int i = 0;

            foreach(KeyValuePair<int, Entity> en in dictionary){
                entities[i++] = en.Value;
            }

            binary.Serialize(file, entities);
            file.Close();

            Debug.Log("/Room_" + _Name + ".dat");
        }

		/// <summary>
		/// Load the scene from the dictionary
		/// </summary>
        public void Load(){
            if(File.Exists(Application.persistentDataPath + "/Room_" + _Name + ".dat")){
                BinaryFormatter binary = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/Room_" + _Name + ".dat", FileMode.Open);

                Entity[] entities = (Entity[])binary.Deserialize(file);
                file.Close();

                Debug.Log(entities.Length);

                foreach(Entity en in entities){

                    GameObject currObj = Object.Instantiate(Resources.Load("EditorPrefabs/Furnitures/" + en.Path + "/" + en.prefabName), en.position, en.rotation) as GameObject;
                    currObj.GetComponent<DictionaryEntity>().AddEntity(en.Path, en.prefabName, en.position, en.rotation);
                    freezeObject(currObj);
//                    currObj.GetComponent<MeshRenderer>().material = currObj.GetComponent<Interactible>().DefaultMaterial;
                }
            }
            Debug.Log("Loaded!!");        
        }


        // Helpers
        private void freezeObject(GameObject obj){
            Rigidbody objRb = obj.GetComponent<Rigidbody>();
            objRb.constraints = RigidbodyConstraints.FreezePositionX | 
                                RigidbodyConstraints.FreezePositionY |
                                RigidbodyConstraints.FreezePositionZ |
                                RigidbodyConstraints.FreezeRotationX | 
                                RigidbodyConstraints.FreezeRotationY |
                                RigidbodyConstraints.FreezeRotationZ;
        }
    }
}
