using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "CurrentSelectedResourcesSO")]
public class CurrentSelectedResourcesSO : ScriptableObject
{
   [ReadOnly]
   public List<TypeEnums.ResourceTypes> selectedResourcesList = new List<TypeEnums.ResourceTypes>();
}
