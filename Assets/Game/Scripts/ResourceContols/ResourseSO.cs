using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ResourceSO")]
public class ResourseSO : ScriptableObject
{
    public TypeEnums.ResourceTypes typeOfResource;
    public int resourcePrice;
}
