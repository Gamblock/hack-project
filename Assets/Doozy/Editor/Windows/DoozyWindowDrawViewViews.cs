// Copyright (c) 2015 - 2020 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using Doozy.Engine.UI.Base;
using Doozy.Engine.UI.Settings;
using UnityEngine;

namespace Doozy.Editor.Windows
{
    public partial class DoozyWindow
    {
        private static NamesDatabase UIViewDatabase { get { return UIViewSettings.Database; } }

        private void InitViewViews() { }

        private void DrawViewViews()
        {
            if (CurrentView != View.Views) return;
            DrawItemsDatabase(UIViewDatabase, false, View.Views);
            
            DrawDynamicViewVerticalSpace(2);
        }
    }
}