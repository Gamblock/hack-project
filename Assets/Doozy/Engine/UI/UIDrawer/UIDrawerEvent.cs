// Copyright (c) 2015 - 2020 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using UnityEngine.Events;

namespace Doozy.Engine.UI
{
    /// <inheritdoc />
    /// <summary> UnityEvent used to send UIDrawer references when an UIDrawerBehavior is triggered </summary>
    [Serializable]
    public class UIDrawerEvent : UnityEvent<UIDrawer> { }
}