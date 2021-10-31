using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageNumbersPro
{
    [System.Serializable]
    public struct FadeSettings
    {
        public FadeSettings(Vector2 newScale)
        {
            fadeDuration = 0.2f;

            positionOffset = new Vector2(0.5f, 0);
            scaleOffset = Vector2.one;
            scale = newScale;
        }

        [Tooltip("The duration that the fade takes.")]
        public float fadeDuration;
        [Space(8)]
        [Tooltip("Moves TextA and TextB in opposite directions based on this offset.")]
        public Vector2 positionOffset;
        [Tooltip("Scales TextA and divides the scale of TextB by this scale.")]
        public Vector2 scaleOffset;
        [Tooltip("Scales both TextA and TextB by this scale.")]
        public Vector2 scale;
    }
}