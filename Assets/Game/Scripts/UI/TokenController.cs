using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TokenController : MonoBehaviour
{
    public TextMeshProUGUI tokenText;
    public ServerCommunicationManager serverManager;
    public ShopViewModel shopViewModel;

    
    private void OnEnable()
    {
        shopViewModel.OnTokenAmountChange += UpdateTokenAmount;
    }

    private void OnDisable()
    {
        shopViewModel.OnTokenAmountChange -= UpdateTokenAmount;
    }

    public void UpdateTokenAmount(int amountToadd)
    {
        int finalValue = PlayerPrefs.GetInt(serverManager.GetTokenKey());
        finalValue = finalValue + amountToadd;
        tokenText.text = finalValue.ToString();
        Debug.Log(finalValue);
        serverManager.SetTokenAmountOnServer(finalValue);
        PlayerPrefs.SetInt(serverManager.GetTokenKey(),finalValue);
        shopViewModel.SetAvailableTokenAmount(finalValue);
    }
    
    public void ExchangeTokens()
    {
        Application.OpenURL("https://binance-hack-frontend.herokuapp.com/tokenBalance");
    }

    public void SeeAllNFT()
    {
        Application.OpenURL("https://binance-hack-frontend.herokuapp.com/nftList");
    }
}
