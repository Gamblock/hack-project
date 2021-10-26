// Copyright (c) 2015 - 2020 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using Doozy.Editor.Internal;
using Doozy.Engine.Themes;
using Doozy.Engine.Utils;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Doozy.Editor.Themes
{
    [CustomEditor(typeof(ColorTargetParticleSystem))]
    [CanEditMultipleObjects]
    public class ColorTargetParticleSystemEditor : BaseEditor
    {
        protected override ColorName ComponentColorName { get { return DGUI.Colors.ThemesColorName; } }

        private ColorTargetParticleSystem m_target;

        private ColorTargetParticleSystem Target
        {
            get
            {
                if (m_target != null) return m_target;
                m_target = (ColorTargetParticleSystem) target;
                return m_target;
            }
        }

        private static ThemesDatabase Database { get { return ThemesSettings.Database; } }
        private string[] ThemesNames;
        private string[] VariantsNames;
//        private string[] PropertyNames;
//        private Color[] Colors;

        private SerializedProperty
            m_particleSystem,
            m_overrideAlpha,
            m_alpha;

        private ThemeData m_theme;
        private LabelId m_Property;
        private int m_selectedThemeIndex;
        private int m_selectedVariantIndex;
        private int m_selectedPropertyIndex;

        private bool HasReference { get { return m_particleSystem.objectReferenceValue != null; } }

        protected override void LoadSerializedProperty()
        {
            base.LoadSerializedProperty();

            m_particleSystem = GetProperty(PropertyName.ParticleSystem);
            m_overrideAlpha = GetProperty(PropertyName.OverrideAlpha);
            m_alpha = GetProperty(PropertyName.Alpha);
        }

        public override void OnInspectorGUI()
        {
            UpdateIds();
            UpdateLists();
            base.OnInspectorGUI();
            serializedObject.Update();
            DrawHeader(Styles.GetStyle(Styles.StyleName.ComponentHeaderColorTargetParticleSystem), MenuUtils.ColorTargetParticleSystem_Manual, MenuUtils.ColorTargetParticleSystem_YouTube);
            GUILayout.Space(DGUI.Properties.Space(2));
            DGUI.Property.Draw(m_particleSystem, UILabels.ParticleSystem, HasReference ? ComponentColorName : ColorName.Red);
            GUILayout.Space(DGUI.Properties.Space());
            ThemeTargetEditorUtils.DrawOverrideAlpha(m_overrideAlpha, m_alpha, Target.ParticleSystem == null ? 1 : Target.ParticleSystem.main.startColor.color.a, ComponentColorName, InitialGUIColor);
            GUILayout.Space(DGUI.Properties.Space(4));
            int themeIndex = Database.GetThemeIndex(Target.ThemeId);
            if (themeIndex != -1)
            {
                ThemeTargetEditorUtils.DrawThemePopup(Database, m_theme, ThemesNames, themeIndex, ComponentColorName, serializedObject, targets, Target, InitialGUIColor, UpdateIds, UpdateLists);
                GUILayout.Space(DGUI.Properties.Space());
                ThemeTargetEditorUtils.DrawActiveVariant(m_theme, ComponentColorName);
            }

            GUILayout.Space(DGUI.Properties.Space(2));
            int propertyIndex = m_theme.GetColorPropertyIndex(Target.PropertyId);
            if (Target.PropertyId == Guid.Empty || propertyIndex == -1) ThemeTargetEditorUtils.DrawLabelNoPropertyFound();
            else ThemeTargetEditorUtils.DrawColorProperties(m_theme, propertyIndex, serializedObject, targets, Target, InitialGUIColor);
            GUILayout.Space(DGUI.Properties.Space(4));
            serializedObject.ApplyModifiedProperties();
        }


        private void UpdateIds()
        {
            if (!Database.Contains(Target.ThemeId))
                Target.ThemeId = Database.Themes[0].Id;
            m_theme = Database.GetThemeData(Target.ThemeId);

            if (!m_theme.ContainsColorProperty(Target.PropertyId))
                Target.PropertyId = m_theme.ColorLabels.Count > 0
                                        ? m_theme.ColorLabels[0].Id
                                        : Guid.Empty;
        }


        private void UpdateLists()
        {
            ThemesNames = ThemesDatabase.GetThemesNames(Database);
//            PropertyNames = ThemesDatabase.GetPropertyNames(m_theme);
//            Colors = ThemesDatabase.GetColors(m_theme);
        }
    }
}