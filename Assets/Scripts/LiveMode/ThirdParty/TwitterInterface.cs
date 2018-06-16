using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

using Twity.DataModels.Responses;

namespace DemoAV.Live.ThirdParty{
using TwitterCallback = Twity.TwitterCallback;

public class TwitterInterface {

	static TwitterInterface () {
		Twity.Oauth.consumerKey = TwitterKeys.ConsumerKey;
		Twity.Oauth.consumerSecret = TwitterKeys.ConsumerSecret;
		Twity.Oauth.accessToken = TwitterKeys.AccessToken;
		Twity.Oauth.accessTokenSecret= TwitterKeys.AccessTokenSecret;
	}
		
	/// <summary>
	/// 	Retrieves the last tweets.
	/// </summary>
	/// <param name="num"> The number of tweets to retrieve. </param>
	public static void GetTwitters(int num){
		Dictionary<string, string> parameters = new Dictionary<string, string>();
		parameters ["count"] = num.ToString();
		IEnumerator enumerator = Twity.Client.Get ("statuses/home_timeline", parameters, Callback);

		while(enumerator.MoveNext()){
			while(((IEnumerator)enumerator.Current).MoveNext()){
			}
		}
	}

	public static void GetTwitters(MonoBehaviour script, int num, TwitterCallback callback){
		Dictionary<string, string> parameters = new Dictionary<string, string>();
		parameters ["count"] = num.ToString();

		script.StartCoroutine(Twity.Client.Get ("statuses/home_timeline", parameters, callback));
	}

	static void Callback(bool success, string response) {
		
		if (success) {
			StatusesHomeTimelineResponse Response = JsonUtility.FromJson<StatusesHomeTimelineResponse> (response);

			// Print the tweets and their author.
			Debug.Log("Success");
			Debug.Log( Response.items[0].user.name);
			Debug.Log( Response.items[0].text);
		} else {
			Debug.Log (response);
		}

	}
}
}


