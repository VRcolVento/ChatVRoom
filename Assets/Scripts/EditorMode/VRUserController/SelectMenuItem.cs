﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectMenuItem : MonoBehaviour {

	public delegate void SelectAction(GameObject obj);
	public delegate void DeselectAction();
	public static event SelectAction menuSelect;
	public static event DeselectAction menuDeselect;
	public static event SelectAction menuPress;

	private SteamVR_TrackedObject trackedObj;


	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

//	private UpdateLineRenderer lineRenderer;
	private int menuMask;

	void Start () {
		menuMask = LayerMask.GetMask("Menu Layer");
		trackedObj = GetComponent<SteamVR_TrackedObject>();
//		lineRenderer = GetComponent<UpdateLineRenderer>();
	}
	

	void Update () {

//		Ray ray = new Ray(lineRenderer.GetPosition(), lineRenderer.GetForward());
		Ray ray = new Ray(transform.position, transform.forward);

		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, Mathf.Infinity, menuMask)) {

			GameObject obj = hit.transform.gameObject;

			if(menuDeselect != null) menuDeselect(); // Call Deselect event: otherwise if objects overlap they all stay blue
			if(menuSelect != null) menuSelect(obj); // Call Select event

			if(Controller.GetHairTriggerDown()) {
				if(menuPress != null) menuPress(obj);
			}
		}
		else {
			if(menuDeselect != null) menuDeselect(); // Call Deselect event: otherwise if objects overlap they all stay blue
		}
	}
}
