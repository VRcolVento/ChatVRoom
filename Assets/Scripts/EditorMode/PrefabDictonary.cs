using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu] 
public class PrefabDictonary : ScriptableObject {

    [System.Serializable]
    private class Entity{
        public string prefabName;
        [SerializeField]
        private float[] _position = new float[3];
        [SerializeField]
        private float[] _rotation = new float[4];

        public Entity(string name, Vector3 position, Quaternion rotation){
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
                this._rotation[0] = value.w;
                this._rotation[1] = value.x;
                this._rotation[2] = value.y;
                this._rotation[3] = value.z;
            }
        }
    }



    private string _Name = "SignoraStanza";
    
    private int currId;
    private Dictionary<int, Entity> dictionary;
    
	private PrefabDictonary(){
        currId = 0;
        dictionary = new Dictionary<int, Entity>();
    }

    public string Name{
        get{
            return _Name;
        }
        set{
            if(_Name == null)
                this._Name = value;
        }
    }

    // Modify an already existing element
    public void AddEntity(int id, string name, Vector3 position, Quaternion rotation) {
        dictionary[id] = new Entity(name, position, rotation);
    }

    // Add a new element to the dictionary
    public int AddEntity(string name, Vector3 position, Quaternion rotation){
        dictionary.Add(currId, new Entity(name, position, rotation));
        return currId++;
    }

    public void RemoveEntity(int id) {
        dictionary.Remove(id);
    }

    public void UpdatePosition(int id, Vector3 position){
        dictionary[id].position = position;
    }

	public void UpdateRotation (int id, Quaternion rotation) {
		dictionary[id].rotation = rotation;
	}

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

        Debug.Log("Saved!!");
    }

    public void Load(){
        if(File.Exists(Application.persistentDataPath + "/Room_" + _Name + ".dat")){
            BinaryFormatter binary = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Room_" + _Name + ".dat", FileMode.Open);

            Entity[] entities = (Entity[])binary.Deserialize(file);
            file.Close();

            foreach(Entity en in entities){
                GameObject currObj = Object.Instantiate(Resources.Load("EditorPrefabs/" + en.prefabName), en.position, en.rotation) as GameObject;
                currObj.GetComponent<DictonaryEntity>().AddEntity(en.prefabName, en.position, en.rotation);
                freezeObject(currObj);
                currObj.GetComponent<MeshRenderer>().material = currObj.GetComponent<Interactible>().DefaultMaterial;
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
