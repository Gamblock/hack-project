using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GOEvent : UnityEvent<GameObject>
{
    
}
public class GameObjectEventListener : MonoBehaviour
{
    [SerializeField] private GameObjectEventChannelSO _channel = default;

    public GOEvent OnEventRaised;

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

    private void Respond(GameObject value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
