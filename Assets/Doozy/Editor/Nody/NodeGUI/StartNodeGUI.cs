// Copyright (c) 2015 - 2020 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using Doozy.Engine.Nody.Nodes;
using UnityEngine;

namespace Doozy.Editor.Nody.NodeGUI
{
    [CustomNodeGUI(typeof(StartNode))]
    public class StartNodeGUI : BaseNodeGUI
    {
        private static GUIStyle s_iconStyle;
        private static GUIStyle IconStyle { get { return s_iconStyle ?? (s_iconStyle = Styles.GetStyle(Styles.StyleName.NodeIconStartNode)); } }
        protected override GUIStyle GetIconStyle() { return IconStyle; }
    }
}