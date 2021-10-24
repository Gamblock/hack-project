using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
[CreateAssetMenu(fileName = "CharacterInfo")]
public class CharacterInfoSO : ScriptableObject
{
    public List<int> objectIndexes;
    public List<StatTypesToUse> characterStats;
}
