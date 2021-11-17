using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public ServerCommunicationManager server;
    public TokenController tokenController;
    public ResourceContainerSO container;
    public TextMeshProUGUI waterAmount;
    public TextMeshProUGUI earthAmount;
    public TextMeshProUGUI fireAmount;

    private void Awake()
    {
        ClearResourceContainer();
    }

    private void ClearResourceContainer()
    {
        container.earthResourceAmount = 0;
        container.fireResourceAmount = 0;
        container.waterResourceAmount = 0; 
    }

    private async void ShowResourceValues()
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        server.UpdateResources(TypeEnums.ResourceTypes.water,0);
        waterAmount.text = container.waterResourceAmount.ToString();
        fireAmount.text = container.fireResourceAmount.ToString();
        earthAmount.text = container. earthResourceAmount.ToString();
    }
    private void OnApplicationQuit()
    {
        ClearResourceContainer();
    }

    public void BuyResource(TypeEnums.ResourceTypes resourceType, int price)
    {
        PlayerPrefs.SetInt(server.GetTokenKey(), 300);
        if (PlayerPrefs.GetInt(server.GetTokenKey()) >= price)
        {
            Debug.Log("Token Purchased");
            server.UpdateResources(resourceType,1);
        }
        else
        {
            Debug.Log("You have insufficient tokens");
        }
    }
    
}
