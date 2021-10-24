﻿using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Events that have one int argument.
/// Example: An Achievement unlock event, where the int is the Achievement ID.
/// </summary>

[CreateAssetMenu(menuName = "Events/Bool Event Channel")]
public class BoolEventChannelSo : ScriptableObject
{
    public UnityAction<bool> OnEventRaised;
    public void RaiseEvent(bool value)
    {
        OnEventRaised.Invoke(value);
    }
}