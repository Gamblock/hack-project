using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Int Event Channel")]
public class IntEventChannelSO : ScriptableObject
{
    public UnityAction<int> OnEventRaised;
    public void RaiseEvent(int value)
    {
        OnEventRaised.Invoke(value);
    }
}