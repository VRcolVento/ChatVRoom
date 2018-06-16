using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DemoAV.Live.ThirdParty;
using Twity.DataModels.Responses;
using TwitterCallback = Twity.TwitterCallback;

namespace DemoAV.Live.Controller {

public class TwitterWritstDisplay : MonoBehaviour {

	public TextMeshPro text;
	public Canvas canvas;
	public Image canvasBg;
	public Transform lookTarget;

	public Transform flooor;

	// Use this for initialization
	void Start () {
		
		Twity.Oauth.consumerKey = TwitterKeys.ConsumerKey;
		Twity.Oauth.consumerSecret = TwitterKeys.ConsumerSecret;
		Twity.Oauth.accessToken = TwitterKeys.AccessToken;
		Twity.Oauth.accessTokenSecret= TwitterKeys.AccessTokenSecret;
		
		
		Dictionary<string, string> parameters = new Dictionary<string, string>();
		parameters ["count"] = "1";
		StartCoroutine (Twity.Client.Get ("statuses/home_timeline", parameters, Callback));
	}
	
	// Update is called once per frame
	void Update () {
		
		text.transform.LookAt(lookTarget);
		text.transform.forward = -text.transform.forward;
	}

	void Callback(bool success, string response) {

		if (success) {
			StatusesHomeTimelineResponse Response = JsonUtility.FromJson<StatusesHomeTimelineResponse> (response);
			text.text = "New tweet from: " + Response.items[0].user.name + "\n";
		} else {
			Debug.Log (response);
		}
}
}

}
