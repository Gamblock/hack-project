using System;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;

namespace Doozy.Examples
{
    public class E12PopupManagerScript : MonoBehaviour
    {
        [Header("Popup Settings")]
        public string PopupName = "AchievementPopup";

        [Header("Achievements")]
        public List<AchievementData> Achievements = new List<AchievementData>();

        private UIPopup m_popup;

        public void ShowAchievement(int achievementId)
        {
            //make sure the achievement we want to show has been defined in the Achievements list
            //the achievementId is actually the index of the achievement as it has been defined in the list
            if (Achievements == null || achievementId < 0 || achievementId > Achievements.Count - 1) return;

            //get the achievement from the list
            AchievementData achievement = Achievements[achievementId];

            //make sure we got an achievement and that the entry was not null
            if (achievement == null) return;

            //get a clone of the UIPopup, with the given PopupName, from the UIPopup Database 
            m_popup = UIPopupManager.GetPopup(PopupName);

            //make sure that a popup clone was actually created
            if (m_popup == null)
                return;

            //set the achievement icon
            m_popup.Data.SetImagesSprites(achievement.Icon);
            //set the achievement title and message
            m_popup.Data.SetLabelsTexts(achievement.Achievement, achievement.Description);

            //show the popup
            UIPopupManager.ShowPopup(m_popup, m_popup.AddToPopupQueue, false);
        }

        public void ClearPopupQueue()
        {
            UIPopupManager.ClearQueue();
        }
    }

    [Serializable]
    public class AchievementData
    {
        public string Achievement;
        public string Description;
        public Sprite Icon;
    }
}