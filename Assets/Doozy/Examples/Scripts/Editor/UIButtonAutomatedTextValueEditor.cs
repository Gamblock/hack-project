using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Editor;
using Doozy.Editor.Internal;
using Doozy.Engine.UI;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Doozy.Examples
{
    [CustomEditor(typeof(UIButtonAutomatedTextValue))]
    [CanEditMultipleObjects]
    public class UIButtonAutomatedTextValueEditor : BaseEditor
    {
        private UIButtonAutomatedTextValue Target { get { return (UIButtonAutomatedTextValue) target; } }

        protected override void OnEnable()
        {
            base.OnEnable();

            UpdatePresetCategoryAndName();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.BeginHorizontal();
            {
                DGUI.Property.Draw(serializedObject.FindProperty("PresetCategory"));
                GUILayout.Space(DGUI.Properties.Space());
                DGUI.Property.Draw(serializedObject.FindProperty("PresetName"));
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(DGUI.Properties.Space(2));
            DGUI.Property.Draw(serializedObject.FindProperty("BehaviorType"));

            serializedObject.ApplyModifiedProperties();

//            UpdatePresetCategoryAndName();

            GUILayout.Space(DGUI.Properties.Space(8));

            if (DGUI.Button.Draw("Set Preset Text Values", Size.S, TextAlign.Center, ComponentColorName, ComponentColorName, true, DGUI.Properties.SingleLineHeight * 2))
            {
                UpdatePresetCategoryAndName();
            }
        }

        private void UpdatePresetCategoryAndName()
        {
            foreach (Object o in targets)
            {
                var targetObject = (UIButtonAutomatedTextValue) o;
                var button = targetObject.GetComponent<UIButton>();
                switch (targetObject.BehaviorType)
                {
                    case UIButtonBehaviorType.OnClick:
                        targetObject.PresetCategory.text = button.OnClick.PresetCategory.ToUpper();
                        targetObject.PresetName.text = button.OnClick.PresetName;
                        break;
                    case UIButtonBehaviorType.OnDoubleClick:
                        targetObject.PresetCategory.text = button.OnDoubleClick.PresetCategory.ToUpper();
                        targetObject.PresetName.text = button.OnDoubleClick.PresetName;
                        break;
                    case UIButtonBehaviorType.OnLongClick:
                        targetObject.PresetCategory.text = button.OnLongClick.PresetCategory.ToUpper();
                        targetObject.PresetName.text = button.OnLongClick.PresetName;
                        break;
                    case UIButtonBehaviorType.OnRightClick:
                        targetObject.PresetCategory.text = button.OnRightClick.PresetCategory.ToUpper();
                        targetObject.PresetName.text = button.OnRightClick.PresetName;
                        break;
                    case UIButtonBehaviorType.OnPointerEnter:
                        targetObject.PresetCategory.text = button.OnPointerEnter.PresetCategory.ToUpper();
                        targetObject.PresetName.text = button.OnPointerEnter.PresetName;
                        break;
                    case UIButtonBehaviorType.OnPointerExit:
                        targetObject.PresetCategory.text = button.OnPointerExit.PresetCategory.ToUpper();
                        targetObject.PresetName.text = button.OnPointerExit.PresetName;
                        break;
                    case UIButtonBehaviorType.OnPointerDown:
                        targetObject.PresetCategory.text = button.OnPointerDown.PresetCategory.ToUpper();
                        targetObject.PresetName.text = button.OnPointerDown.PresetName;
                        break;
                    case UIButtonBehaviorType.OnPointerUp:
                        targetObject.PresetCategory.text = button.OnPointerUp.PresetCategory.ToUpper();
                        targetObject.PresetName.text = button.OnPointerUp.PresetName;
                        break;
                    case UIButtonBehaviorType.OnSelected:
                        targetObject.PresetCategory.text = button.OnSelected.PresetCategory.ToUpper();
                        targetObject.PresetName.text = button.OnSelected.PresetName;
                        break;
                    case UIButtonBehaviorType.OnDeselected:
                        targetObject.PresetCategory.text = button.OnDeselected.PresetCategory.ToUpper();
                        targetObject.PresetName.text = button.OnDeselected.PresetName;
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }

                targetObject.name = "Button - " + targetObject.PresetCategory.text + " " + targetObject.PresetName.text;
            }
        }
    }
}