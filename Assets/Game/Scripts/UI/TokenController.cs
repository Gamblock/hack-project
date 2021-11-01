using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TokenController : MonoBehaviour
{
    public TextMeshProUGUI tokenText;
    public ServerCommunicationManager serverManager;
    
    
    public void UpdateTokenAmount(int amountToadd)
    {
        serverManager.GetUserByEmail(PlayerPrefs.GetString(serverManager.GetEmailKey()));
        int finalValue = PlayerPrefs.GetInt(serverManager.GetTokenKey());
        finalValue = finalValue + amountToadd;
        tokenText.text = finalValue.ToString();
        serverManager.SetTokenAmountOnServer(finalValue);
    }
    
    public void ExchangeTokens()
    {
        Application.OpenURL("https://binance-hack-frontend.herokuapp.com/tokenBalance");
    }
}
