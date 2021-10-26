using System;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Doozy.Examples
{
    public class E11PopupScriptThree : MonoBehaviour
    {
        //a custom enum used to show the same UIPopup, but with different settings
        public enum PopupType
        {
            Error,
            Info,
            Warning,
            Whatever
        }

        [Header("Popup Settings")]
        public string PopupName = "Popup3";

        [Header("Error Popup Settings")]
        public Sprite ErrorSprite;
        public string ErrorTitle = "Error";
        public string ErrorMessage = "This is an ERROR message";
        public Color ErrorTextColor = Color.red;

        [Header("Info Popup Settings")]
        public Sprite InfoSprite;
        public string InfoTitle = "Info";
        public string InfoMessage = "This is an INFO message";
        public Color InfoTextColor = Color.cyan;

        [Header("Warning Popup Settings")]
        public Sprite WarningSprite;
        public string WarningTitle = "Warning";
        public string WarningMessage = "This is a WARNING message";
        public Color WarningTextColor = Color.yellow;

        [Header("Whatever Popup Settings")]
        public Sprite WhateverSprite;
        public string WhateverTitle = "Whatever";
        public string WhateverMessage = "Hello world! WHATEVER!!! :)";
        public Color WhateverTextColor = Color.green;

        /// <summary> Reference to the UIPopup clone used by this script</summary>
        private UIPopup m_popup;

        public void ShowPopup(PopupType popupType)
        {
            //get a clone of the UIPopup, with the given PopupName, from the UIPopup Database
            m_popup = UIPopup.GetPopup(PopupName);

            //make sure that a popup clone was actually created
            if (m_popup == null)
                return;

            var icon = m_popup.Data.Images[0];
            var title = m_popup.Data.Labels[0].GetComponent<Text>(); //we know this has a Text component because we created it
            var message = m_popup.Data.Labels[1].GetComponent<Text>();

            switch (popupType)
            {
                case PopupType.Error:
                    m_popup.Data.SetImagesSprites(ErrorSprite);            //set icon (we know we have one because we created this UIPopup with it)
                    m_popup.Data.SetLabelsTexts(ErrorTitle, ErrorMessage); //set the title and the message (we know they exist and their order because we created them)
                    icon.color = ErrorTextColor;                           //set the icon color (we use white icons, so we can set any tint color)
                    title.color = ErrorTextColor;                          //set title text color
                    message.color = ErrorTextColor;                        //set message text color
                    break;
                case PopupType.Info:
                    m_popup.Data.SetImagesSprites(InfoSprite);           //set icon (we know we have one because we created this UIPopup with it)
                    m_popup.Data.SetLabelsTexts(InfoTitle, InfoMessage); //set the title and the message (we know they exist and their order because we created them)
                    icon.color = InfoTextColor;                          //set the icon color (we use white icons, so we can set any tint color)
                    title.color = InfoTextColor;                         //set title text color
                    message.color = InfoTextColor;                       //set message text color
                    break;
                case PopupType.Warning:
                    m_popup.Data.SetImagesSprites(WarningSprite);              //set icon (we know we have one because we created this UIPopup with it)
                    m_popup.Data.SetLabelsTexts(WarningTitle, WarningMessage); //set the title and the message (we know they exist and their order because we created them)
                    icon.color = WarningTextColor;                             //set the icon color (we use white icons, so we can set any tint color)
                    title.color = WarningTextColor;                            //set title text color
                    message.color = WarningTextColor;                          //set message text color
                    break;
                case PopupType.Whatever:
                    m_popup.Data.SetImagesSprites(WhateverSprite);               //set icon (we know we have one because we created this UIPopup with it)
                    m_popup.Data.SetLabelsTexts(WhateverTitle, WhateverMessage); //set the title and the message (we know they exist and their order because we created them)
                    icon.color = WhateverTextColor;                              //set the icon color (we use white icons, so we can set any tint color)
                    title.color = WhateverTextColor;                             //set title text color
                    message.color = WhateverTextColor;                           //set message text color
                    break;
                default: throw new ArgumentOutOfRangeException("popupType", popupType, null);
            }

            m_popup.Show(); //show the popup
        }

        public void ShowInfoPopup() { ShowPopup(PopupType.Info); }
        public void ShowWarningPopup() { ShowPopup(PopupType.Warning); }
        public void ShowErrorPopup() { ShowPopup(PopupType.Error); }
        public void ShowWhateverPopup() { ShowPopup(PopupType.Whatever); }
    }
}