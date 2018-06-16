using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

using DemoAV.SmartMenu;
using DemoAV.Common;
using DemoAV.Live.ThirdParty;

using Twity.DataModels.Responses;
using TMPro;

namespace DemoAV.Live.SmarTv{
    using TwitterResponse = StatusesHomeTimelineResponse;
    class TvTwitter : ITvApp{
        Texture2D texture;
        GameObject display;
        TvMenuFactory menuFact;
        

        public TvTwitter( GameObject display, TvMenuFactory fact){
            this.display = display;
            this.menuFact = fact;
            this.texture = Resources.Load("Images/SmartTv/Twitter") as Texture2D;
        }


		/// <summary>
        ///     Gets the name of the current application.
        /// </summary>
        /// <returns> The name of the current application. </returns>
        string ITvApp.GetName(){
            return "Twitter";
        }


		/// <summary>
        ///     Gets a description of the current application.
        /// </summary>
        /// <returns> The description of the current application. </returns>
        string ITvApp.GetDescription(){
            return "Twitter app";
        }

		/// <summary>
        ///     Gets the icon representing the current application.
        /// </summary>
        /// <returns> The texture for the icon. </returns>
        Texture2D ITvApp.GetTexture(){
            return texture;
        }

        /// <summary>
        ///     The function to call when the twitter application is selected.
        /// </summary>
        /// <param name="name"> "Twitter app" </param>
        void ITvApp.ItemCallback(string name){
            // Deactivate menu.
            menuFact.SetActiveMenu(null);

            Menu tweets = menuFact.CreateMenu(TvMenuFactory.Type.SOCIAL_MENU, "TwitterMenu");

            if(tweets != null){
                // Show twitters
                TwitterInterface.GetTwitters(display.transform.parent.GetComponent<SmartTv>(), 20, (bool success, string response)=>{
                    if (success) {
                        TwitterResponse tresponse = JsonUtility.FromJson<TwitterResponse> (response);

                        // Print the tweets and their author.
                        for(int i = 0; i < tresponse.items.Length; ++i){
                            // display.transform.parent.GetComponent<SmartTv>().StartCoroutine(DownloadImage(tresponse.items[i].user.profile_background_image_url, image));

                            string []fields = {tresponse.items[i].user.name, tresponse.items[i].text};
                            Menu.MenuItem item = new Menu.MenuItem();
                            item.name = "tweet" + i;
                            item.fields = fields;
                            tweets.AddMenuItem(item, (string nm) => {});
                        }

                        menuFact.SetActiveMenu("TwitterMenu");
                    } else {
                        Debug.Log (response);
                    }
                });
            }
            else
                menuFact.SetActiveMenu("TwitterMenu");
        }

        IEnumerator DownloadImage(string url, Image img){
            Texture2D tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
            using (WWW www = new WWW(url))
            {
                yield return www;
                www.LoadImageIntoTexture(tex);
                img.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
            }
        }

    }
}
