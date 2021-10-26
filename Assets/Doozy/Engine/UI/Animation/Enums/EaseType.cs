// Copyright (c) 2015 - 2020 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

namespace Doozy.Engine.UI.Animation
{
    /// <summary> Describes the types of ease available for any animation </summary>
    public enum EaseType
    {
        /// <summary>
        /// Use a predefined ease curve to specify the rate of change of a parameter over time (also see: https://easings.net/)
        /// </summary>
        Ease = 0,
        
        /// <summary>
        /// Use a custom AnimationCurve to specify the rate of change of a parameter over time
        /// </summary>
        AnimationCurve = 1
    }
}