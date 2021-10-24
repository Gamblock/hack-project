using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Vector3 Event Channel")]
public class Vector3EventChannelSO : ScriptableObject
{
    public UnityAction<Vector3> OnEventRaised;
    public void RaiseEvent(Vector3 value)
    {
        OnEventRaised.Invoke(value);
    }
    
}
