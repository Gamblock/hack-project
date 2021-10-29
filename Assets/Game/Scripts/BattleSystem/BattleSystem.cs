using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public enum BattleStates
{
    Start,
    PlayerTurn,
    EnemyTurn,
    Won,
    Lost
}
public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerPosition;
    public Transform enemyPosition;
    public BattleStates state;
    public CanvasGroup tutorialCanvas;

    public CanvasGroup playerActionsUI;

    private BattleUnit playerUnit;
    private BattleUnit enemyUnit;
    private GameObject player;
    private GameObject enemy;
    private bool isFirstTurn = true;
    
    private void Start()
    {
        state = BattleStates.Start;
        SetUpBattle();
    }

    private void Update()
    {
       StateChecker();
    }

    public void SetUpBattle()
    {
       player = Instantiate(playerPrefab, playerPosition.position, playerPosition.rotation);
       enemy =  Instantiate(enemyPrefab, enemyPosition.position,enemyPosition.rotation);

        playerUnit = player.GetComponent<BattleUnit>();
        playerUnit.currentHp = playerUnit.hp;
        player.GetComponent<BattleUI>().SetHud(playerUnit);
        enemyUnit = enemy.GetComponent<BattleUnit>();
        enemyUnit.currentHp = enemyUnit.hp;
        enemy.GetComponent<BattleUI>().SetHud(enemyUnit);
       
        state = BattleStates.PlayerTurn;
        PlayerTurn();
    }

    public void PlayerTurn()
    {
        if (isFirstTurn)
        {
            isFirstTurn = false;
            ShowTutorialWindow("It's your turn choose an action", 5);
        }
        
    }

    public async void ShowTutorialWindow(string inputText,float showDuration)
    {
        tutorialCanvas.GetComponentInChildren<TextMeshProUGUI>().text = inputText;
        tutorialCanvas.alpha = 1;
        await Task.Delay(TimeSpan.FromSeconds(showDuration));
        tutorialCanvas.alpha = 0;
    }

    private void StateChecker()
    {
        if (enemyUnit.currentHp <= 0)
        {
            state = BattleStates.Won;
        }
        else if(playerUnit.currentHp <= 0)
        {
            state = BattleStates.Lost;
        }
        if (state == BattleStates.PlayerTurn)
        {
            playerActionsUI.interactable = true;
        }
        else
        {
            playerActionsUI.interactable = false;
        }
    }
    public void PlayerAttack()
    {
        enemyUnit.currentHp = enemyUnit.currentHp - (playerUnit.damage + playerUnit.strength - enemyUnit.armor);
       enemyUnit.gameObject.GetComponent<BattleUI>().SetHP(enemyUnit.currentHp);
       state = BattleStates.EnemyTurn;

    }
    public void EnemyAttack()
    {
        playerUnit.currentHp = playerUnit.currentHp - (enemyUnit.damage + enemyUnit.strength - playerUnit.armor);
        playerUnit.gameObject.GetComponent<BattleUI>().SetHP(playerUnit.currentHp);
    }
}
