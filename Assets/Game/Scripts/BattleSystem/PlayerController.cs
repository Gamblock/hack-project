using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using MoreMountains.Feedbacks;
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
    public List<GameObject> dpsLoadout;
    public List<GameObject> tankLoadout;
    public List<GameObject> healerLoadout;
    public MMFeedbacks testFeedBack;
    
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

    public CharacterInfoSO GetCurrentCharacter(ServerCommunicationManager serverManager)
    {
        setter.serverManager = serverManager;
        CharacterInfoSO tempChar = setter.SetCharacter();
        if (tempChar.classType == TypeEnums.ClassTypes.Healer)
        {
            foreach (var gearPiece in healerLoadout)
            {
                gearPiece.SetActive(true);
            }
        }
        if (tempChar.classType == TypeEnums.ClassTypes.Tank)
        {
            foreach (var gearPiece in tankLoadout)
            {
                gearPiece.SetActive(true);
            }
        } 
        if (tempChar.classType == TypeEnums.ClassTypes.DamageDealer)
        {
            foreach (var gearPiece in dpsLoadout)
            {
                gearPiece.SetActive(true);
            }
        }

        return tempChar;
    }
    public void TakeDamage(int damageValue)
    {
        takeDamageParticles.Play();
        playerAnimator.SetTrigger("Damaged");
        unit.currentHp -= damageValue;
        battleUI.SetHP(unit.currentHp);
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
       testFeedBack.PlayFeedbacks();
       onEnemyDamaged.RaiseEvent(20); 
   }
   public void TankAbility()
   {
       playerAnimator.SetTrigger("Heal");
       tankAbilityParticles.Play();
   }
   public void DPSAbility()
   {
       playerAnimator.SetTrigger("Heal");
       dpsAbilityParticles.Play();
   }
}
