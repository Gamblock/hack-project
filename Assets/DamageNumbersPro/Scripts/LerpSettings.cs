using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageNumbersPro
{
    [System.Serializable]
    public struct LerpSettings
    {
        public LerpSettings (float customDefault)
        {
            minX = -0.7f;
            maxX = 0.7f;
            minY = 1f;
            maxY = 2f;

            speed = 5f;
        }

        [Header("Offset:")]
        [Tooltip("Minimum of horizontal offset.")]
        public float minX;
        [Tooltip("Maximum of horizontal offset.")]
        public float maxX;
        [Tooltip("Minimum of vertical offset.")]
        public float minY;
        [Tooltip("Maximum of vertical offset.")]
        public float maxY;

        [Header("Speed:")]
        [Tooltip("Speed at which it moves to the offset position.")]
        public float speed;
    }
}