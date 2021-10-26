// Copyright (c) 2015 - 2020 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using Doozy.Editor.Internal;
using Doozy.Engine.Nody.Nodes;
using Doozy.Engine.Utils;
using UnityEditor;
using UnityEngine;

namespace Doozy.Editor.Nody.Editors
{
    [CustomEditor(typeof(EnterNode))]
    public class EnterNodeEditor : BaseNodeEditor
    {
        private EnterNode m_enterNode;
        private InfoMessage m_infoMessageNotConnected;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_enterNode = (EnterNode) target;
            m_infoMessageNotConnected = new InfoMessage(InfoMessage.MessageType.Error, UILabels.NotConnectedTitle, UILabels.NotConnectedMessage);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            DrawHeader(Styles.GetStyle(Styles.StyleName.ComponentHeaderEnterNode), MenuUtils.EnterNode_Manual, MenuUtils.EnterNode_YouTube);
            DrawDebugMode(true);
            GUILayout.Space(DGUI.Properties.Space(2));
            DrawNodeName(false);
            m_infoMessageNotConnected.Draw(m_enterNode.ErrorNodeIsNotConnected, InspectorWidth);
            GUILayout.Space(DGUI.Properties.Space(8));
            DrawOutputSockets(BaseNode);
            GUILayout.Space(DGUI.Properties.Space());
            serializedObject.ApplyModifiedProperties();
            SendGraphEventNodeUpdated();
        }
    }
}