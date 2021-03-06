using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "CharacterInfo")]
public class CharacterInfoSO : ScriptableObject
{
    [Sirenix.OdinInspector.ReadOnly]
    public string id = Guid.NewGuid().ToString();
    public List<int> objectIndexes;
    public List<StatTypesToUse> characterStats;
    public string charNAme;
    public TypeEnums.ClassTypes classType;
    public Texture2D cardTexture;

    public void ClearTemplate()
    {
        id = Guid.NewGuid().ToString();
        objectIndexes.Clear();
        characterStats.Clear();
        charNAme = "";
        cardTexture = null;
    }
    
}


