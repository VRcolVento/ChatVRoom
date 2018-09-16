using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace DemoAV.Live.SmarTv.SmartMenu{

/// <summary>
///     A menu with a list of string.
/// </summary>
public class TextMenu : Menu {
    struct TextMenuInfo{
		public float width, height;
        public int itemsPerTab, count;
		public Vector3 start;
        
	};
	TextMenuInfo menuInfo;        // Some infos about the panel structure.
    GameObject textBox;

    public TextMenu(GameObject father, GameObject textBox, string name, int itemsPerTab) : base(father, name) {
        this.textBox = textBox;

        // Init infos.
        menuInfo.itemsPerTab = itemsPerTab;
        menuInfo.width = 1;
        menuInfo.height = 1.0f / itemsPerTab;
        menuInfo.start = new Vector3( 0, 0.5f - menuInfo.height, - 0.02f);
    }

    public override void SetSelected(string item){
        Transform newItem = item == null ? null : GetTab(activeTab).transform.Find(item);
        GameObject itemObj = newItem == null ? null : newItem.gameObject;

        if(_selectedItem != itemObj){
            // If there is a selected item.
            if(_selectedItem){
                _selectedItem.GetComponent<TextMeshPro>().color = Color.white;
            }

            // If there is a new selected item.
            if(itemObj){
                itemObj.GetComponent<TextMeshPro>().color = Color.blue;
            }

            _selectedItem = itemObj;
        }
    }

    public override bool AddMenuItem(MenuItem item, ItemCallback callback){
        if(root.transform.Find(item.name) == null){
            int i = menuInfo.count++;
		    int tabIndex = i / menuInfo.itemsPerTab;
            
		    Transform tab = GetTab(tabIndex).transform;
            
            Vector3 pos = menuInfo.start - new Vector3(0, (i % menuInfo.itemsPerTab) * menuInfo.height, 0);
            GameObject currText = GameObject.Instantiate(textBox);

            // Set attributes.
            currText.transform.SetParent(tab);
            currText.transform.localPosition = pos;
            currText.transform.localRotation = Quaternion.identity;
            currText.transform.localScale = new Vector3(1, 1, 1);
            currText.name = item.name;
            currText.layer = Menu.menuLayer;
            currText.GetComponent<RectTransform>().sizeDelta = new Vector2(menuInfo.width, menuInfo.height);
            currText.GetComponent<BoxCollider>().size = new Vector3(menuInfo.width, menuInfo.height, 0);

            // Add callback.
            SetCallback(item.name, callback);

            // Set text.
            TextMeshPro tmp = currText.GetComponent<TextMeshPro>();
            tmp.text = item.name;
            return true;
        }
        return false;
    }
}
}