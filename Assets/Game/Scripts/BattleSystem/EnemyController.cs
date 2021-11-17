using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
   [FormerlySerializedAs("fireballPrefab")] public Projectile projectilePrefab;
   public Animator enemyAnimator;
   public Transform castingHandTransform;
   public int fireballDamage = 15;
   public ParticleSystem spike;
   public int spikeDamage = 20;
   public IntEventChannelSO onPlayerDamaged;
   public float spikeYOffset = -1.7f;
   public ParticleSystem takeDamageParticles;
    
   private Transform oponent;
   private Projectile _projectile;

   public void Init(Transform opponentTransform)
   {
       oponent = opponentTransform;
   }

   public void CastFireball()
   {
       enemyAnimator.SetTrigger("Fireball");
   }

   public void CastLightning()
   {
       enemyAnimator.SetTrigger("Spike");
   }
   public void Spike()
   {
       GameObject spikeGO = Instantiate(spike.gameObject, new Vector3(oponent.position.x, oponent.position.y + spikeYOffset, oponent.position.z), Quaternion.identity);
       onPlayerDamaged.RaiseEvent(spikeDamage);
   }
   public void EnemyFireballAttack()
   {
       _projectile = Instantiate(projectilePrefab.gameObject, castingHandTransform.position, Quaternion.identity).GetComponent<Projectile>();
       _projectile.Shoot(oponent,fireballDamage,onPlayerDamaged);
   }

   public void UseRandomAttack()
   {
       int i = Random.Range(0, 2);
       Debug.Log(i);
       if (i == 0)
       {
           CastFireball();
       }

       if (i == 1)
       {
           CastLightning();
       }
   }
   public void TakeDamage()
   {
       takeDamageParticles.Play();
       enemyAnimator.SetTrigger("Damaged");
   }
}
