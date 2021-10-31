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
   public async void AttackMelee()
    {
        playerAnimator.SetTrigger("Attack");
        await Task.Delay(TimeSpan.FromSeconds(0.5));
        slashParticles.Play();
        onEnemyDamaged.RaiseEvent(20);
    }
}
