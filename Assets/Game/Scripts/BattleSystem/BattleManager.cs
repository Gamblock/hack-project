using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public ServerCommunicationManager server;
    public TokenController tokenController;
    public PlayerController playerController;
    public BattleSystemUISetup battleUI;
    public Transform playerPosition;
    public BattleSystem battler;
    

    private PlayerController controller;

    private void Start()
    {
        Init();
    }

    
    public void Init()
    {
        controller = Instantiate(playerController, playerPosition.position,Quaternion.identity);
        battler.InitPlayer(controller);
        battler.SetUpBattle();
        battleUI.SetUIButtons(controller.GetCurrentCharacter(server));
    }
    
}
