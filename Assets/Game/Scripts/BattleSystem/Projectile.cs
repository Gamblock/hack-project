using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   public float flySpeed = 10;
   public IntEventChannelSO onFireballHit;
   public ParticleSystem explosion;
   public float yOffset = 1.5f;
   private bool isFlying;
   private float leeway = 0.25f;
   private Transform targetTransform;
   private int damageToDeal;
  
   private bool castByPlayer;
   public void Shoot(Transform target, int damage, IntEventChannelSO damageChannel)
   {
      targetTransform = target;
      onFireballHit = damageChannel;
      damageToDeal = damage;
      isFlying = true;
   }

   private void Update()
   {
      if (isFlying)
      {
         transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetTransform.position.x,targetTransform.position.y + yOffset, targetTransform.position.z), flySpeed);
      }

      if ( isFlying && Vector3.Distance(transform.position, new Vector3(targetTransform.position.x,targetTransform.position.y + yOffset, targetTransform.position.z)) <= leeway )
      {
         HitOponent();
      }
   }

   public void HitOponent()
   {
      isFlying = false;
      Instantiate(explosion.gameObject, transform.position, Quaternion.identity);
      onFireballHit.RaiseEvent(damageToDeal);
      Destroy(gameObject);
   }
}


