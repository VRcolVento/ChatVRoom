using System.Collections;
using UnityEngine;

using Twity.DataModels.Responses;
using DemoAV.Live.ThirdParty;

namespace DemoAV.Live.Notification{
	
public class TwitterNotification : MonoBehaviour {
	public NotificationManager notificationManager;
	// Last tweet.
	string lastTweet = null;

	/// <summary>
	/// 	Start is called on the frame when a script is enabled just before
	/// 	any of the Update methods is called the first time.
	/// </summary>
	void Start(){
		// StartCoroutine(AddNotifications());
		TwitterInterface.GetTwitters(this, 10, AddTwitterNotifications);
	}

	/// <summary>
	/// 	Adds a starting number of tweets to the menu.
	/// </summary>
	/// <param name="success"> If the callback has been successful. </param>
	/// <param name="response"> The text json of the response object. </param>
	void AddTwitterNotifications(bool success, string response){
		if (success) {
			StatusesHomeTimelineResponse responseObj = JsonUtility.FromJson<StatusesHomeTimelineResponse> (response);

			// Print the tweets and their author.
			for(short i = (short)(responseObj.items.Length - 1); i > 0; --i)
				notificationManager.AddNotification(new NotificationManager.notification("New tweet from " + responseObj.items[i].user.name, Color.black, responseObj.items[i].text, Color.black));
		
			lastTweet = responseObj.items[0].text;
		}
	}

	/// <summary>
	/// 	Periodically checks if a new tweet has been tweeted.
	/// </summary>
	/// <returns></returns>
	IEnumerator CheckNewTweets(){
		while(true){
			yield return new WaitForSeconds(30);
			TwitterInterface.GetTwitters(this, 1, AddNewTweet);
		}
	}

	/// <summary>
	/// 	Adds a new tweet if it is different from the last one.
	/// </summary>
	/// <param name="success"> True in case of success. </param>
	/// <param name="response"> The JSON object as response text. </param>
	void AddNewTweet(bool success, string response){
		if (success) {
			StatusesHomeTimelineResponse responseObj = JsonUtility.FromJson<StatusesHomeTimelineResponse> (response);

			if(responseObj.items[0].text != lastTweet){
				notificationManager.AddNotification(new NotificationManager.notification("New tweet from " + responseObj.items[0].user.name, Color.black, responseObj.items[0].text, Color.black));
				lastTweet = responseObj.items[0].text;
			}
		}
	}

	// IEnumerator AddNotifications(){
	// 	yield return new WaitForSeconds(2);
	// 	for(int i = 0; i < 15; ++i){
	// 		AddNotification(new notification("Notification " + i, Color.black, "Some random text", Color.black));
	// 		// yield return new WaitForSeconds(1);
	// 	}
	// }
}
}
