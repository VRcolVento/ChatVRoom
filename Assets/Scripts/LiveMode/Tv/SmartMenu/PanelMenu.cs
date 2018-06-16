using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoAV.SmartMenu{
public class PanelMenu : Menu {
    struct PanelMenuInfo{
		public int itemsPerRow, itemsPerTab, count;
		public Vector3 start, scale;
	};
	PanelMenuInfo panelInfo;        // Some infos about the panel structure.
    GameObject panel;               // The panel to use as prefab.
    GameObject selectedItem;        // Current selected item.

    public PanelMenu(GameObject father, GameObject panel, string name, int itemsPerRow, int itemsPerCol) : base(father, name) {
        // Init panel object.
        this.panel = panel;

        // Init panel info.
        panelInfo = new PanelMenuInfo();
		panelInfo.itemsPerRow = itemsPerRow;
		panelInfo.itemsPerTab = panelInfo.itemsPerRow * itemsPerCol;
        panelInfo.scale = new Vector3(1.0f/itemsPerCol, 1.0f/itemsPerRow, 1);
        // panelInfo.scale.Scale(father.transform.localScale);
		panelInfo.count = 0;
        Vector3 upperLeftCorner = new Vector3(-0.5f, 0.5f, 0);
		panelInfo.start = upperLeftCorner + 
							new Vector3(  panelInfo.scale.x / 2,
										- panelInfo.scale.y / 2,
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
		int tabIndex = i / panelInfo.itemsPerTab;
		Transform tab = GetTab(tabIndex).transform;

		// Create and append panel.
		Vector3 pos = new Vector3((i % panelInfo.itemsPerRow), - i / panelInfo.itemsPerRow, 0);
        pos.Scale(panelInfo.scale);
		GameObject currPanel = GameObject.Instantiate(panel);
		currPanel.transform.SetParent(tab);
        currPanel.transform.localPosition = panelInfo.start + pos;
        currPanel.transform.localRotation = Quaternion.identity;
        currPanel.transform.localScale = panelInfo.scale;
        currPanel.name = item.name;
        currPanel.layer = Menu.menuLayer;

        // Add callback.
        SetCallback(item.name, callback);

		// Set Texture.
		currPanel.GetComponent<Renderer>().material.SetTexture("_MainTex", item.texture);
        return true;
    }
}
}