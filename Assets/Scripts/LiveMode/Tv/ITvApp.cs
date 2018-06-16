using UnityEngine;

namespace DemoAV.Live.SmarTv{

    interface ITvApp{

        /// <summary>
        ///     Gets the name of the current application.
        /// </summary>
        /// <returns> The name of the current application. </returns>
        string GetName();

        /// <summary>
        ///     Gets a description of the current application.
        /// </summary>
        /// <returns> The description of the current application. </returns>
        string GetDescription();

        /// <summary>
        ///     Gets the icon representing the current application.
        /// </summary>
        /// <returns> The texture for the icon. </returns>
        Texture2D GetTexture();

        /// <summary>
        ///     The function to call when the streaming function is selected.
        /// </summary>
        /// <param name="name"> The name of the selected item. </param>
        void ItemCallback(string name);
    }
}