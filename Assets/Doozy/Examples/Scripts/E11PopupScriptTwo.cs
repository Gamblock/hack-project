using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Doozy.Examples
{
    public class E11PopupScriptTwo : MonoBehaviour
    {
        [Header("Popup Settings")]
        public string PopupName = "Popup2";

        public string Title = "Example Title";
        public string Message = "This is an example message for this UIPopup";

        [Space(10)]
        public string LabelButtonOne = "Yes";

        public string LabelButtonTwo = "No";
        public bool HideOnButtonOne = true;
        public bool HideOnButtonTwo = true;

        [Header("Settings Controls")]
        public InputField TitleInput;

        public InputField MessageInput;

        [Space(10)]
        public InputField LabelButtonOneInput;

        public InputField LabelButtonTwoInput;
        public Toggle ButtonOneToggle;
        public Toggle ButtonTwoToggle;

        /// <summary> Reference to the UIPopup clone used by this script</summary>
        private UIPopup m_popup;

        private void OnEnable()
        {
            TitleInput.text = Title;
            MessageInput.text = Message;

            TitleInput.onEndEdit.AddListener((value) => { Title = value; });
            MessageInput.onEndEdit.AddListener((value) => { Message = value; });

            LabelButtonOneInput.text = LabelButtonOne;
            LabelButtonTwoInput.text = LabelButtonTwo;

            HideOnButtonOne = ButtonOneToggle.isOn;
            HideOnButtonTwo = ButtonTwoToggle.isOn;

            ButtonOneToggle.onValueChanged.AddListener(value => { HideOnButtonOne = value; });
            ButtonTwoToggle.onValueChanged.AddListener(value => { HideOnButtonTwo = value; });
        }

        private void OnDisable()
        {
            TitleInput.onEndEdit.RemoveAllListeners();
            MessageInput.onEndEdit.RemoveAllListeners();
            LabelButtonOneInput.onEndEdit.RemoveAllListeners();
            LabelButtonTwoInput.onEndEdit.RemoveAllListeners();
            ButtonOneToggle.onValueChanged.RemoveAllListeners();
            ButtonTwoToggle.onValueChanged.RemoveAllListeners();
        }

        public void ShowPopup()
        {
            //get a clone of the UIPopup, with the given PopupName, from the UIPopup Database
            m_popup = UIPopup.GetPopup(PopupName);

            //make sure that a popup clone was actually created
            if (m_popup == null)
                return;

            //we assume (because we know) this UIPopup has a Title and a Message text objects referenced, thus we set their values
            m_popup.Data.SetLabelsTexts(Title, Message);

            //get the values from the label input fields
            LabelButtonOne = LabelButtonOneInput.text;
            LabelButtonTwo = LabelButtonTwoInput.text;

            //set the button labels
            m_popup.Data.SetButtonsLabels(LabelButtonOne, LabelButtonTwo);

            //set the buttons callbacks as methods
            m_popup.Data.SetButtonsCallbacks(ClickButtonOne, ClickButtonTwo);

            //OR set the buttons callbacks as lambda expressions
            //m_popup.Data.SetButtonsCallbacks(() => { ClickButtonOne(); }, () => { ClickButtonTwo(); });

            //if the developer did not enable at least one button to hide it, make the UIPopup hide when its Overlay is clicked
            if (!HideOnButtonOne && !HideOnButtonTwo)
            {
                m_popup.HideOnClickOverlay = true;
                DDebug.Log("Popup '" + PopupName + "' is set to close when clicking its Overlay because you did not enable any hide option");
            }

            m_popup.Show(); //show the popup
        }

        private void ClickButtonOne()
        {
            DDebug.Log("Clicked button ONE: " + LabelButtonOne);
            if (HideOnButtonOne) ClosePopup();
        }

        private void ClickButtonTwo()
        {
            DDebug.Log("Clicked button TWO: " + LabelButtonTwo);
            if (HideOnButtonTwo) ClosePopup();
        }

        private void ClosePopup()
        {
            if (m_popup != null) m_popup.Hide();
        }
    }
}