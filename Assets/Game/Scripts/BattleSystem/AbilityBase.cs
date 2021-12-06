using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct AllVariables
{
    public Transform targetTransform;
    public Transform casterTransform;
    public Transform castingHandTransform;
    public Animator casterAnimator;
}
[CreateAssetMenu(fileName = "Ability")]
public abstract class AbilityBase : MonoBehaviour
{
    
    public AllVariables GetAllVariables;
    public virtual void Cast(){}

}
