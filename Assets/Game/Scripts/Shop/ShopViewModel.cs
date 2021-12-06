using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShopViewModel")]
public class ShopViewModel : ScriptableObject
{
    public event Action<int> OnTokenAmountChange = i => { };

    public ObservableVariable<int> avaliableTokenAmount;

    public void SetAvailableTokenAmount(int avaliableTokens)
    {
        avaliableTokenAmount.Value = avaliableTokens;
    }
    public void ChangeTokenAmount(int value)
    {
        OnTokenAmountChange.Invoke(value);
    }

    
}
