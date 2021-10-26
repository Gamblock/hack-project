using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Doozy.Examples
{
    public class E11PopupScriptOne : MonoBehaviour
    {
        [Header("Popup Settings")]
        public string PopupName = "Popup1";
        public string Title = "Example Title";
        public string Message = "This is an example message for this UIPopup";

        [Space(10)]
        public bool HideOnBackButton = true;
        public bool HideOnClickAnywhere = true;
        public bool HideOnClickOverlay = true;
        public bool HideOnClickContainer = true;

        [Header("Settings Controls")]
        public InputField TitleInput;
        public InputField MessageInput;
        [Space(10)]
        public Toggle BackButtonToggle;
        public Toggle ClickAnywhereToggle;
        public Toggle ClickOverlayToggle;
        public Toggle ClickContainerToggle;

        private void OnEnable()
        {
            TitleInput.text = Title;
            MessageInput.text = Message;

            TitleInput.onEndEdit.AddListener((value) => { Title = value; });
            MessageInput.onEndEdit.AddListener((value) => { Message = value; });

            HideOnBackButton = BackButtonToggle.isOn;
            HideOnClickAnywhere = ClickAnywhereToggle.isOn;
            HideOnClickOverlay = ClickOverlayToggle.isOn;
            HideOnClickContainer = ClickContainerToggle.isOn;

            BackButtonToggle.onValueChanged.AddListener(value => { HideOnBackButton = value; });
            ClickAnywhereToggle.onValueChanged.AddListener(value => { HideOnClickAnywhere = value; });
            ClickOverlayToggle.onValueChanged.AddListener(value => { HideOnClickOverlay = value; });
            ClickContainerToggle.onValueChanged.AddListener(value => { HideOnClickContainer = value; });
        }

        private void OnDisable()
        {
            TitleInput.onEndEdit.RemoveAllListeners();
            MessageInput.onEndEdit.RemoveAllListeners();
            BackButtonToggle.onValueChanged.RemoveAllListeners();
            ClickAnywhereToggle.onValueChanged.RemoveAllListeners();
            ClickOverlayToggle.onValueChanged.RemoveAllListeners();
            ClickContainerToggle.onValueChanged.RemoveAllListeners();
        }

        public void ShowPopup()
        {
            //get a clone of the UIPopup, with the given PopupName, from the UIPopup Database 
            UIPopup popup = UIPopup.GetPopup(PopupName);

            //make sure that a popup clone was actually created
            if (popup == null)
                return;

            //we assume (because we know) this UIPopup has a Title and a Message text objects referenced, thus we set their values
            popup.Data.SetLabelsTexts(Title, Message);

            //update the hide settings
            popup.HideOnBackButton = HideOnBackButton;
            popup.HideOnClickAnywhere = HideOnClickAnywhere;
            popup.HideOnClickOverlay = HideOnClickOverlay;
            popup.HideOnClickContainer = HideOnClickContainer;

            //if the developer did not enable at least one hide option, make the UIPopup automatically hide itself (to avoid blocking the UI)
            if (!HideOnBackButton && !HideOnClickAnywhere && !HideOnClickOverlay && !HideOnClickContainer)
            {
                popup.AutoHideAfterShow = true;
                popup.AutoHideAfterShowDelay = 3f;
                DDebug.Log("Popup '" + PopupName + "' is set to auto-hide in " + popup.AutoHideAfterShowDelay + " seconds because you did not enable any hide option");
            }

            popup.Show(); //show the popup
        }
    }
}