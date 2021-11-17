using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    public Transform castingHandTransform;
    public ParticleSystem slashParticles;
    public Animator playerAnimator;
    public ParticleSystem takeDamageParticles;
    public ParticleSystem tankAbilityParticles;
    public ParticleSystem dpsAbilityParticles;
    public IntEventChannelSO onEnemyDamaged;
    public SetCharacterFromSO setter;
    public BattleUI battleUI;
    public BattleUnit unit;
    
    private Transform oponent;
    private TypeEnums.ClassTypes classType;
    private Projectile _projectile;

    public AbilityBase specialAbilityPrefab;
    public AbilityBase casterAttackPrefab;

    
    private AbilityBase specialAbility;
    private AbilityBase casterAttack;

    private void Start()
    {
        specialAbility = Instantiate(specialAbilityPrefab, transform.position, Quaternion.identity);
        casterAttack = Instantiate(casterAttackPrefab, transform.position, Quaternion.identity);
        casterAttack.GetAllVariables.castingHandTransform = castingHandTransform;
        casterAttack.GetAllVariables.targetTransform = oponent;
        battleUI.SetHud(unit);
    }

    public void Init(Transform opponentTransform)
    {
        oponent = opponentTransform;
    }

    public SetCharacterFromSO GetSetter()
    {
        return setter;
    }
    public void TakeDamage()
    {
        takeDamageParticles.Play();
        playerAnimator.SetTrigger("Damaged");
    }

    public void HealerAbility()
    {
        specialAbility.GetAllVariables.casterTransform = transform;
        specialAbility.GetAllVariables.casterAnimator = playerAnimator;
        specialAbility.Cast();
    }
    
    public void HealerAttack()
    {
        casterAttack.GetAllVariables.castingHandTransform = castingHandTransform;
        casterAttack.GetAllVariables.targetTransform = oponent;
        casterAttack.GetAllVariables.casterAnimator = playerAnimator;
        casterAttack.Cast();
    }
    
   public  void AttackMelee()
   {
       StartCoroutine(AttackMeleeDelay());
   }

   private IEnumerator AttackMeleeDelay()
   {
       playerAnimator.SetTrigger("Attack");
       yield return new WaitForSeconds(0.5f);
       slashParticles.Play();
       onEnemyDamaged.RaiseEvent(20); 
   }
   public void TankAbility()
   {
       playerAnimator.SetTrigger("Heal");
       tankAbilityParticles.Play();
   }
   public void DPSAbility()
   {
       Debug.Log("dps");
       playerAnimator.SetTrigger("Heal");
       dpsAbilityParticles.Play();
   }
}
