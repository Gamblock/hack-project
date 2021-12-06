using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySelfCast : AbilityBase
{
    
    public string triggerName;
    public ParticleSystem spellParticles;
    public IntEventChannelSO changedValueChannel;
    public float delayBeforeParticlesPlay;
    public int influenceValue;
    

    public override void Cast()
    {
        StartCoroutine(CastSpell());
    }

    private IEnumerator CastSpell()
    {
        
        spellParticles.transform.position = GetAllVariables.casterTransform.position;
        GetAllVariables.casterAnimator.SetTrigger(triggerName);
        yield return new WaitForSeconds(delayBeforeParticlesPlay);
        spellParticles.Play();
        if (influenceValue != 0)
        {
            changedValueChannel.RaiseEvent(influenceValue);
        }
    }
}
