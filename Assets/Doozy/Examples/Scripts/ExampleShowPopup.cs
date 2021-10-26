using Doozy.Engine.UI;
using UnityEngine;

namespace Doozy.Examples
{
    public class ExampleShowPopup : MonoBehaviour
    {
        public void ShowPopup(string popupName) { UIPopupManager.ShowPopup(popupName, false, false); }
        public void ShowQueuedPopup(string popupName) { UIPopupManager.ShowPopup(popupName, true, false); }
        public void HidePopup(string popupName) { UIPopup.HidePopup(popupName); }
    }
}