// Copyright (c) 2015 - 2020 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System.Text;
using Doozy.Engine.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Doozy.Engine.Progress
{
    /// <inheritdoc />
    /// <summary>
    /// Used by a Progressor to update the text value of a Text component
    /// </summary>
    [AddComponentMenu(MenuUtils.ProgressTargetText_AddComponentMenu_MenuName, MenuUtils.ProgressTargetText_AddComponentMenu_Order)]
    [DefaultExecutionOrder(DoozyExecutionOrder.PROGRESS_TARGET_TEXT)]
    public class ProgressTargetText : ProgressTarget
    {
        #region UNITY_EDITOR

#if UNITY_EDITOR
        [MenuItem(MenuUtils.ProgressTargetText_MenuItem_ItemName, false, MenuUtils.ProgressTargetText_MenuItem_Priority)]
        private static void CreateComponent(MenuCommand menuCommand) { DoozyUtils.AddToScene<ProgressTargetText>(MenuUtils.ProgressTargetText_GameObject_Name, false, true); }
#endif

        #endregion

        #region Public Variables

        /// <summary> Target Text component </summary>
        public Text Text;

        /// <summary> The variable that will get converted to string and set as the text value of the Text target </summary>
        public TargetVariable TargetVariable = TargetVariable.Progress;

        /// <summary> Should the target variable value get rounded to the nearest integer? </summary>
        public bool WholeNumbers = true;

        /// <summary> Should the target variable value have a multiplier applied? </summary>
        public bool UseMultiplier;

        /// <summary>
        /// If UseMultiplier is TRUE, the target variable value will be multiplied by this value.
        /// Useful if you want to convert the Progress from 0.5 to 50% for example. 
        /// </summary>
        public float Multiplier = 100;

        /// <summary> Text added before the converted string value </summary>
        public string Prefix;

        /// <summary> Text added after the converted string value </summary>
        public string Suffix = "%";

        #endregion

        #region Private Variables

        /// <summary> Internal variable used to keep track if this progress target's variables have been initialized </summary>
        private bool m_initialized;

        /// <summary> Internal variable used to get the updated target value </summary>
        private float m_targetValue;

        /// <summary> Internal variable used to lower the string operations memory allocations </summary>
        private StringBuilder m_stringBuilder = new StringBuilder();

        #endregion

        #region Public Methods

        /// <inheritdoc />
        /// <summary> Method used by a Progressor to when the current Value has changed </summary>
        /// <param name="progressor"> The Progressor that triggered this update </param>
        public override void UpdateTarget(Progressor progressor)
        {
            base.UpdateTarget(progressor);

            if (!m_initialized) Init();

            if (Text == null) return;

            m_targetValue = 0;
            switch (TargetVariable)
            {
                case TargetVariable.Value:
                    m_targetValue = progressor.Value;
                    if (UseMultiplier) m_targetValue *= Multiplier;
                    if (WholeNumbers) m_targetValue = Mathf.Round(m_targetValue);
                    break;
                case TargetVariable.MinValue:
                    m_targetValue = progressor.MinValue;
                    if (UseMultiplier) m_targetValue *= Multiplier;
                    break;
                case TargetVariable.MaxValue:
                    m_targetValue = progressor.MaxValue;
                    if (UseMultiplier) m_targetValue *= Multiplier;
                    break;
                case TargetVariable.Progress:
                    m_targetValue = progressor.Progress;
                    if (UseMultiplier) m_targetValue *= Multiplier;
                    if (WholeNumbers) m_targetValue = Mathf.Round(m_targetValue);
                    break;
                case TargetVariable.InverseProgress:
                    m_targetValue = progressor.InverseProgress;
                    if (UseMultiplier) m_targetValue *= Multiplier;
                    if (WholeNumbers) m_targetValue = Mathf.Round(m_targetValue);
                    break;
            }


            Text.text = m_stringBuilder.Remove(0, m_stringBuilder.Length)
                                       .Append(Prefix)
                                       .Append(m_targetValue)
                                       .Append(Suffix)
                                       .ToString();
        }

        #endregion

        #region Private Methods

        private void Reset() { UpdateReference(); }

        private void Init()
        {
            UpdateReference();
            if (m_stringBuilder == null) m_stringBuilder = new StringBuilder();
            m_initialized = true;
        }

        private void UpdateReference()
        {
            if (Text == null)
                Text = GetComponent<Text>();
        }

        #endregion
    }
}