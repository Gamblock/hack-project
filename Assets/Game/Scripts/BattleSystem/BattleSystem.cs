using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DamageNumbersPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public CanvasGroup winscreenCanvas;
    public IntEventChannelSO onPlayerTakeDamage;
    public IntEventChannelSO onEnemyTakeDamage;
    public DamageNumber damageNumbers;
    public DamageNumber healingNumbers;
    public CanvasGroup playerActionsUI;

    private BattleUnit playerUnit;
    private BattleUnit enemyUnit;
    private GameObject enemy;
    private PlayerController playerController;
    private EnemyController enemyController;
    private bool isFirstTurn = true;
    private bool enemyCanAttack;
    private bool tankAbilityUsed;
    private bool dpsAbilityUsed;
    
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
        enemy =  Instantiate(enemyPrefab, enemyPosition.position,enemyPosition.rotation); 
        playerController = playerPrefab.GetComponent<PlayerController>();
        enemyController = enemy.GetComponent<EnemyController>();
        playerController.Init(enemy.transform);
        enemyController.Init(playerPrefab.transform);
        playerUnit = playerPrefab.GetComponent<BattleUnit>();
        playerUnit.currentHp = playerUnit.hp;
        playerPrefab.GetComponent<BattleUI>().SetHud(playerUnit);
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
        /*
        tutorialCanvas.GetComponentInChildren<TextMeshProUGUI>().text = inputText;
        tutorialCanvas.alpha = 1;
        await Task.Delay(TimeSpan.FromSeconds(showDuration));
        tutorialCanvas.alpha = 0;
        */
    }

    private void StateChecker()
    {
        if (enemyUnit.currentHp <= 0)
        {
            state = BattleStates.Won;
            winscreenCanvas.interactable = true;
            winscreenCanvas.alpha = 1;
        }
        else if(playerUnit.currentHp <= 0)
        {
            state = BattleStates.Lost;
        }
        if (state == BattleStates.PlayerTurn)
        {
            foreach (var button in playerActionsUI.GetComponentsInChildren<Button>())
            {
                if (button.gameObject.activeSelf)
                {
                    button.interactable = true; 
                }
               
            }
            playerActionsUI.interactable = true;
        }
        else
        {
            playerActionsUI.interactable = false;
        }

        if (state == BattleStates.EnemyTurn && enemyCanAttack)
        {
            EnemyAttack();
        }
    }

    public void TankAbility()
    {
        playerPrefab.GetComponent<PlayerController>().TankAbility();
        state = BattleStates.EnemyTurn;
        enemyCanAttack = true;
        tankAbilityUsed = true;
    }
    public void dpsAbility()
    {
        playerPrefab.GetComponent<PlayerController>().DPSAbility();
        state = BattleStates.EnemyTurn;
        enemyCanAttack = true;
        dpsAbilityUsed = true;
    }
    public async void PlayerAttack()
    {
        playerPrefab.GetComponent<PlayerController>().AttackMelee();
        await Task.Delay(TimeSpan.FromSeconds(2));
    }

    public void PlayerHealAttack()
    {
        playerPrefab.GetComponent<PlayerController>().HealerAttackStart();
    }
    public async void PlayerHeal()
    {
        playerPrefab.GetComponent<PlayerController>().HealerAbility();
        playerUnit.currentHp += 20;
        playerUnit.gameObject.GetComponent<BattleUI>().SetHP(playerUnit.currentHp);
        healingNumbers.CreateNew(20,playerPrefab.transform.position);
        await Task.Delay(TimeSpan.FromSeconds(1));
        state = BattleStates.EnemyTurn;
        enemyCanAttack = true;
    }
    public async void EnemyAttack()
    {
        enemyController.UseRandomAttack();
        enemyCanAttack = false;
        await Task.Delay(TimeSpan.FromSeconds(2));
    }

    public async void DamagePlayer(int damageValue)
    {
        if (tankAbilityUsed)
        {
            damageValue = damageValue / 2;
            DamageEnemy(damageValue);
            tankAbilityUsed = false;
            state = BattleStates.PlayerTurn;
        }
        playerUnit.currentHp = playerUnit.currentHp - damageValue;
        playerUnit.gameObject.GetComponent<BattleUI>().SetHP(playerUnit.currentHp);
        playerPrefab.GetComponent<PlayerController>().TakeDamage();
        damageNumbers.CreateNew(damageValue, playerPrefab.transform.position);
        await Task.Delay(TimeSpan.FromSeconds(1));
        state = BattleStates.PlayerTurn;
        
    } 
    public async void DamageEnemy(int damageValue)
    {
        if (dpsAbilityUsed)
        {
            damageValue = damageValue * 2 + 5;
            dpsAbilityUsed = false;
        }
        enemyUnit.currentHp = enemyUnit.currentHp - damageValue;
        enemyUnit.gameObject.GetComponent<BattleUI>().SetHP(enemyUnit.currentHp);
        enemy.GetComponent<EnemyController>().TakeDamage();
        damageNumbers.CreateNew(damageValue, enemy.transform.position);
        await Task.Delay(TimeSpan.FromSeconds(1));
        state = BattleStates.EnemyTurn;
        enemyCanAttack = true;
    }
    
}
