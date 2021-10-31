using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageNumbersPro
{
    [System.Serializable]
    public struct CombinationSettings
    {
        public CombinationSettings(float customDefault)
        {
            absorberType = AbsorberType.OLDEST;
            combinationGroup = "";
            maxDistance = 10f;

            bonusLifetime = 1f;
            delay = 0.2f;

            absorberSpeed = 0;
            targetSpeed = 5f;

            absorbTime = 0.4f;
            targetFadeOutSpeed = 1f;
            targetScaleDownSpeed = 1f;

            instantGain = false;

            absorberScaleGain = new Vector2(1.5f, 1.5f);
            absorberScaleFade = 15;
        }

        [Header("Main:")]
        [Tooltip("If set to oldest, the oldest number will absorb new numbers.  If set to newest, the newest number will absorb existing numbers.")]
        public AbsorberType absorberType;
        [Tooltip("Only numbers of the same combination group will combine with each other.")]
        public string combinationGroup;
        [Tooltip("The maximum distance at which numbers will combine.")]
        public float maxDistance;
        [Tooltip("If true, the absorber will instantly gain the numbers of the target.  Should be used when combination is very fast.")]
        public bool instantGain;
        [Tooltip("Delay after spawning that a number can be absorbed.")]
        public float delay;

        [Header("Lifetime:")]
        [Tooltip("The lifetime of the absorber is reset and also increased by this bonus lifetime.")]
        public float bonusLifetime;

        [Header("Movement:")]
        [Tooltip("Speed of the absorber while absorbing.")]
        public float absorberSpeed;
        [Tooltip("Speed of the target while being absorbed.")]
        public float targetSpeed;
        [Tooltip("Time at which the target is absorbed and destroyed.")]
        public float absorbTime;

        [Header("Fading:")]
        [Tooltip("Fades out the target while it is being absorbed.  Keep this at 1 to 100% fade it out by the time it is absorbed.")]
        public float targetFadeOutSpeed;
        [Tooltip("Scales down the target while it is being absorbed.")]
        public float targetScaleDownSpeed;

        [Header("Scale:")]
        [Tooltip("Absorber gains a scale up when it absorbs the target.")]
        public Vector2 absorberScaleGain;
        [Tooltip("Speed at which the scale up fades out.")]
        public float absorberScaleFade;
    }

    [System.Serializable]
    public enum AbsorberType
    {
        OLDEST
        ,
        NEWEST
    }
}