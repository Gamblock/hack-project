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
    public GameObject enemyPrefab;

    public Transform playerPosition;
    public Transform enemyPosition;
    public BattleStates state;
    public CanvasGroup winscreenCanvas;
    public IntEventChannelSO onPlayerTakeDamage;
    public IntEventChannelSO onEnemyTakeDamage;
    public IntEventChannelSO onPlayerHeal;
    public DamageNumber damageNumbers;
    public DamageNumber healingNumbers;
    public CanvasGroup playerActionsUI;
    public TokenController gloryHole;
    
    private BattleUnit playerUnit;
    private BattleUnit enemyUnit;
    private GameObject enemy;
    private PlayerController playerController;
    private EnemyController enemyController;
    private bool isFirstTurn = true;
    private bool enemyCanAttack;
    private bool tankAbilityUsed;
    private bool dpsAbilityUsed;
    private bool playerUsedAbility;
    
    private void Start()
    {
        state = BattleStates.Start;
    }

    private void Update()
    {
       StateChecker();
    }

    private void OnEnable()
    {
        onPlayerTakeDamage.OnEventRaised += DamagePlayer;
        onEnemyTakeDamage.OnEventRaised += DamageEnemy;
        onPlayerHeal.OnEventRaised += OnPlayerHeal;
    }

    private void OnDisable()
    {
        onPlayerTakeDamage.OnEventRaised -= DamagePlayer;
        onEnemyTakeDamage.OnEventRaised -= DamageEnemy;
        
    }

    public void InitPlayer(PlayerController player)
    {
        playerController = player;
    }
    public void SetUpBattle()
    {
        enemy =  Instantiate(enemyPrefab, enemyPosition.position,enemyPosition.rotation);
        enemyController = enemy.GetComponent<EnemyController>();
        playerController.Init(enemy.transform);
        enemyController.Init(playerController.transform);
        playerUnit = playerController.unit;
        playerUnit.currentHp = playerUnit.hp;
        enemyUnit = enemy.GetComponent<BattleUnit>();
        enemyUnit.currentHp = enemyUnit.hp;
        enemy.GetComponent<BattleUI>().SetHud(enemyUnit);
        state = BattleStates.PlayerTurn;
    }
    
    private void StateChecker()
    {
        if (enemyUnit.currentHp <= 0)
        {
            state = BattleStates.Won;
            winscreenCanvas.interactable = true;
            winscreenCanvas.alpha = 1;
            gloryHole.UpdateTokenAmount(100);
        }
        else if(playerUnit.currentHp <= 0)
        {
            state = BattleStates.Lost;
        }
        if (state == BattleStates.PlayerTurn && !playerUsedAbility)
        {
            playerActionsUI.interactable = true;
        }
        else if(state != BattleStates.PlayerTurn)
        {
            playerActionsUI.interactable = false;
            playerUsedAbility = false;
        }

        if (state == BattleStates.EnemyTurn && enemyCanAttack)
        {
            EnemyAttack();
        }
    }

    public void DisablePlayerActionsUi()
    {
        playerActionsUI.interactable = false;
    }
    public void TankAbility()
    {
        playerUsedAbility = true;
       playerController.TankAbility();
        state = BattleStates.EnemyTurn;
        enemyCanAttack = true;
        tankAbilityUsed = true;
    }
    public void dpsAbility()
    {
        playerUsedAbility = true;
        playerController.DPSAbility();
        state = BattleStates.EnemyTurn;
        enemyCanAttack = true;
        dpsAbilityUsed = true;
    }

    public void CastPlayerSpecialAbility()
    {
        playerController.HealerAbility();
    }
    public void PlayerAttackMelee()
    {
        playerUsedAbility = true;
        playerActionsUI.interactable = false;
        StartCoroutine(PlayerAttackDelay());
    }

    private IEnumerator PlayerAttackDelay()
    {
        playerController.GetComponent<PlayerController>().AttackMelee();
        yield return new WaitForSeconds(2);
    }
    public void PlayerHealAttack()
    {
        playerUsedAbility = true;
        playerActionsUI.interactable = false;
        playerController.HealerAttack();
    }
    public void OnPlayerHeal(int healValue)
    {
        playerUnit.currentHp += healValue;
        healingNumbers.CreateNew(healValue,playerController.transform.position);
        state = BattleStates.EnemyTurn;
        enemyCanAttack = true; 
    }
   
    public void EnemyAttack()
    {
        
        StartCoroutine(EnemyAttackDelay());
    }

    private IEnumerator EnemyAttackDelay()
    {
        enemyController.UseRandomAttack();
        enemyCanAttack = false;
        yield return new WaitForSeconds(2);
    }
    public  void DamagePlayer(int damageValue)
    {
        StartCoroutine(DamagePlayerDelay(damageValue));

    } 
    private IEnumerator  DamagePlayerDelay(int damageValue)
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
        playerController.TakeDamage();
        damageNumbers.CreateNew(damageValue, playerController.transform.position);
        yield return new WaitForSeconds(1);
        state = BattleStates.PlayerTurn;
    }
    public  void DamageEnemy(int damageValue)
    {
        StartCoroutine(DamageEnemyDelay(damageValue));
    }

    private IEnumerator DamageEnemyDelay(int damageValue)
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
        yield return new WaitForSeconds(1);
        state = BattleStates.EnemyTurn;
        enemyCanAttack = true;  
    }
    
}
