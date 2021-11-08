using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbility : AbilityBase
{
    public string triggerName;
    public Projectile projectile;
    private Projectile castedProjectile;
    public int damageValue;
    public IntEventChannelSO changedValueChannel;

   
    public override void Cast()
    {
        GetAllVariables.casterAnimator.SetTrigger(triggerName);
        castedProjectile = Instantiate(projectile, GetAllVariables.castingHandTransform.position, Quaternion.identity);
        castedProjectile.onFireballHit = changedValueChannel;
        castedProjectile.Shoot(GetAllVariables.targetTransform,damageValue, GetAllVariables.targetTransform.GetComponent<BattleUnit>().takeDamageChannel);
        
    }
}
