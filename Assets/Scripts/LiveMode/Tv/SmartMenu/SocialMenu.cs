using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace DemoAV.Live.SmarTv.SmartMenu{

/// <summary>
///     A menu to show the twitter social network.
/// </summary>
public class SocialMenu : Menu {
    struct SocialMenuInfo{
		public int itemsPerCol, count;
		public Vector3 start, imageScale;
        public Vector2 nameDim, statusDim;
	};
	SocialMenuInfo panelInfo;        // Some infos about the panel structure.
    GameObject textobj;
    GameObject selectedItem;        // Current selected item.

    public SocialMenu(GameObject father, GameObject text, string name, int itemsPerPage) : base(father, name) {
        textobj = text;
        // Init panel info.
        panelInfo = new SocialMenuInfo();
		panelInfo.itemsPerCol = itemsPerPage;
        panelInfo.imageScale = new Vector3(1.0f / (panelInfo.itemsPerCol * 3), 1.0f / (panelInfo.itemsPerCol * 3), 1);
        panelInfo.nameDim = new Vector2(1 - panelInfo.imageScale.x, panelInfo.imageScale.y);
        panelInfo.statusDim = new Vector2(1, 1.0f / panelInfo.itemsPerCol - panelInfo.imageScale.y);
		panelInfo.count = 0;
        Vector3 upperLeftCorner = new Vector3(-0.5f, 0.5f, 0);
		panelInfo.start = upperLeftCorner + 
							new Vector3(  panelInfo.imageScale.x / 2,
										- panelInfo.imageScale.y / 2,
				  						- 0.02f); 
    }

    public override void SetSelected(string item){
        Transform itemTrs = root.transform.Find(item);
        GameObject itemObj = itemTrs == null ? null : itemTrs.gameObject;

        if(selectedItem != itemObj){
            // If there is a selected item.
            if(selectedItem){
                itemObj.GetComponent<Material>().color = Color.white;
            }

            // If there is a new selected item.
            if(itemObj){

            }

            selectedItem = itemObj;
        }
    }

    public override bool AddMenuItem(MenuItem item, ItemCallback callback){
        int i = panelInfo.count++;
		int tabIndex = i / panelInfo.itemsPerCol;
        int rowIndex = i % panelInfo.itemsPerCol;
		Transform tab = GetTab(tabIndex).transform;

        Vector3 pos = new Vector3(0, - rowIndex * (1.0f / panelInfo.itemsPerCol), 0);
        pos += panelInfo.start;
        
        // Profile image.
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        PositionItem(quad.transform, tab.gameObject, pos, Quaternion.identity, panelInfo.imageScale);
        quad.GetComponent<Renderer>().material.SetTexture("_MainTex", item.texture);

        // Name
        GameObject name = GameObject.Instantiate(textobj);
        PositionItem(name.transform, tab.gameObject, pos + new Vector3(panelInfo.imageScale.x / 2.0f + panelInfo.nameDim.x / 2.0f, 0, 0), Quaternion.identity, new Vector3(1, 1, 1));
        name.GetComponent<RectTransform>().sizeDelta = panelInfo.nameDim;
        name.GetComponent<BoxCollider>().size = new Vector3(panelInfo.nameDim.x, panelInfo.nameDim.y, 0);
        name.GetComponent<TextMeshPro>().text = (string)item.fields[0];
        name.GetComponent<TextMeshPro>().fontSize = 0.5f; 

        // Status
        GameObject status = GameObject.Instantiate(textobj);
        PositionItem(status.transform, tab.gameObject, new Vector3(0, pos.y - (panelInfo.imageScale.y / 2.0f + panelInfo.statusDim.y / 2.0f), -0.02f), Quaternion.identity, new Vector3(1, 1, 1));
        status.GetComponent<RectTransform>().sizeDelta = panelInfo.statusDim;
        status.GetComponent<BoxCollider>().size = new Vector3(panelInfo.statusDim.x, panelInfo.statusDim.y, 0);
        status.GetComponent<TextMeshPro>().text = WrapString((string)item.fields[1]);
        status.GetComponent<TextMeshPro>().fontSize = 0.35f; 

        // Add callback.
        SetCallback(item.name, callback);

        return true;
    }

    /// <summary>
    ///     Shortcut to set a new father for an object and puts it in a new position.
    /// </summary>
    /// <param name="obj"> The object. </param>
    /// <param name="father"> The father. </param>
    /// <param name="pos"> The position wrt the father. </param>
    /// <param name="rot"> The rotation wrt the father. </param>
    /// <param name="scale"> The scale wrt the father. </param>
    private void PositionItem(Transform obj, GameObject father, Vector3 pos, Quaternion rot, Vector3 scale){
        obj.SetParent(father.transform);
        obj.localPosition = pos;
        obj.localRotation = rot;
        obj.localScale = scale;
    }

    /// <summary>
    ///     Adds newline to the string.
    /// </summary>
    /// <param name="str"> The string to wrap. </param>
    /// <returns> The string with newline. </returns>
    string WrapString(string str){
        if(!str.Contains("\n")){
            int step = 50;
            int pos = 0, newPos;
            string newstr = "";

            while( pos + step < str.Length - 1 && (newPos = str.IndexOf(" ", pos + step)) >= 0){            
                newstr += str.Substring(pos, newPos - pos + 1) + '\n';
                pos = newPos + 1;
            }
            newstr += str.Substring(pos, str.Length - pos);

            return newstr;
        }
        return str;
    }
}
}