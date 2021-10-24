using System;using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObservableVariable<T>
{
    [SerializeField] T _value;

    public event Action<T> OnValueChanged;

    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }
    
    public ObservableVariable(T value)
    {
        _value = value;
    }
}

public static class ObservableVariableHelper
{
    public static void ResetAllObservableVariables(object instance)
    {
        var thisType = instance.GetType();
        var fields = thisType.GetFields();
        foreach (var item in fields)
        {
            if (!item.FieldType.IsGenericType || 
                item.FieldType.GetGenericTypeDefinition() != typeof(ObservableVariable<>))
            {
                return;
            }

            var obj = item.GetValue(instance);
            var property = item.FieldType.GetProperty("Value");
            property.SetValue(obj, default);
        }
    }
}
