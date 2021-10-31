using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Fireball : MonoBehaviour
{
   public float flySpeed = 10;
   public IntEventChannelSO onFireballHitPlayer;
   public IntEventChannelSO onFireballHitEnemy;
   public ParticleSystem explosion;
   private Transform enemy;
   private bool isFlying;
   private float leeway = 0.25f;
   private int damageToDeal;
   private bool castByPlayer;
   public void Shoot(Transform enemyTransform, int damage, bool isPlayer)
   {
      castByPlayer = isPlayer;
      enemy = enemyTransform;
      isFlying = true;
      damageToDeal = damage;
   }

   private void Update()
   {
      if (isFlying)
      {
         transform.position = Vector3.MoveTowards(transform.position, enemy.position, flySpeed);
      }

      if (Vector3.Distance(transform.position, enemy.position) <= leeway && isFlying)
      {
         HitOponent();
      }
   }

   public void HitOponent()
   {
      isFlying = false;
      if (!castByPlayer)
      {
         onFireballHitPlayer.RaiseEvent(damageToDeal);
      }
      else
      {
         onFireballHitEnemy.RaiseEvent(damageToDeal);
      }
     
      Instantiate(explosion.gameObject, transform.position, Quaternion.identity);
      Destroy(gameObject);
   }
}


