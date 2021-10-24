using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// To use a generic UnityEvent type you must override the generic type.
/// </summary>
[System.Serializable]
public class Vec3Event : UnityEvent<Vector3>
{

}

/// <summary>
/// A flexible handler for int events in the form of a MonoBehaviour. Responses can be connected directly from the Unity Inspector.
/// </summary>
public class Vector3EventListener : MonoBehaviour
{
    [SerializeField] private Vector3EventChannelSO _channel = default;

    public Vec3Event OnEventRaised;

    private void OnEnable()
    {
        if (_channel != null)
            _channel.OnEventRaised += Respond;
    }

    private void OnDisable()
    {
        if (_channel != null)
            _channel.OnEventRaised -= Respond;
    }

    private void Respond(Vector3 value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}