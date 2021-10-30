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
    public IntEventChannelSO onPlayerTakeDamage;
    public IntEventChannelSO onEnemyTakeDamage;

    public CanvasGroup playerActionsUI;

    private BattleUnit playerUnit;
    private BattleUnit enemyUnit;
    private GameObject player;
    private GameObject enemy;
    private PlayerController playerController;
    private EnemyController enemyController;
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

    private void OnEnable()
    {
        onPlayerTakeDamage.OnEventRaised += DamagePlayer;
        onEnemyTakeDamage.OnEventRaised += DamageEnemy;
    }

    private void OnDisable()
    {
        onPlayerTakeDamage.OnEventRaised -= DamagePlayer;
        onEnemyTakeDamage.OnEventRaised -= DamageEnemy;
        
    }

    public void SetUpBattle()
    {
       player = Instantiate(playerPrefab, playerPosition.position, playerPosition.rotation);
       enemy =  Instantiate(enemyPrefab, enemyPosition.position,enemyPosition.rotation);
       playerController = player.GetComponent<PlayerController>();
       enemyController = enemy.GetComponent<EnemyController>();
       playerController.Init(enemy.transform);
       enemyController.Init(player.transform);
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

        if (state == BattleStates.EnemyTurn)
        {
            EnemyAttack();
        }
    }
    public async void PlayerAttack()
    {
        player.GetComponent<PlayerController>().AttackMelee();
        await Task.Delay(TimeSpan.FromSeconds(2));
        state = BattleStates.EnemyTurn;
    }

    public void PlayerHealAttack()
    {
        player.GetComponent<PlayerController>().HealerAttackStart();
    }
    public async void PlayerHeal()
    {
        player.GetComponent<PlayerController>().HealerAbility();
        playerUnit.currentHp += 20;
        playerUnit.gameObject.GetComponent<BattleUI>().SetHP(playerUnit.currentHp);
        await Task.Delay(TimeSpan.FromSeconds(1));
        state = BattleStates.EnemyTurn;
    }
    public async void EnemyAttack()
    {
        enemyController.UseRandomAttack();

        await Task.Delay(TimeSpan.FromSeconds(2));
        state = BattleStates.PlayerTurn;
    }

    public void DamagePlayer(int damageValue)
    {
        playerUnit.currentHp = playerUnit.currentHp - damageValue;
        playerUnit.gameObject.GetComponent<BattleUI>().SetHP(playerUnit.currentHp);
        player.GetComponent<PlayerController>().TakeDamage();
    } 
    public void DamageEnemy(int damageValue)
    {
        enemyUnit.currentHp = enemyUnit.currentHp - damageValue;
        enemyUnit.gameObject.GetComponent<BattleUI>().SetHP(enemyUnit.currentHp);
        enemy.GetComponent<EnemyController>().TakeDamage();
    }
    
}
