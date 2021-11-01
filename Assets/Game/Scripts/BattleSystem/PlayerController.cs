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
    public Fireball fireball;
    public ParticleSystem slashParticles;
    public Animator playerAnimator;
    public ParticleSystem takeDamageParticles;
    public ParticleSystem healParticles;
    public ParticleSystem tankAbilityParticles;
    public ParticleSystem dpsAbilityParticles;
    public IntEventChannelSO onEnemyDamaged;
    public VoidEventChannelSO onPlayerTurnFinished;
    private Transform oponent;
    private TypeEnums.ClassTypes classType;
    private Fireball _fireball;

    private void Update()
    {
        
    }

    public void Init(Transform opponentTransform)
    {
        oponent = opponentTransform;
    }

    public void TakeDamage()
    {
        takeDamageParticles.Play();
        playerAnimator.SetTrigger("Damaged");
    }

    public void HealerAbility()
    {
        healParticles.Play();
        playerAnimator.SetTrigger("Heal");
    }


    public void HealerAttack()
    {
        _fireball = Instantiate(fireball.gameObject, castingHandTransform.position, Quaternion.identity).GetComponent<Fireball>();
        _fireball.Shoot(oponent,20,true);
    }
    public void HealerAttackStart()
    {
        Debug.Log("healer");
        playerAnimator.SetTrigger("HealerAttack");
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
