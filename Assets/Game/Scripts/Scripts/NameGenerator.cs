using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ResourceNameParts
{
    public TypeEnums.ResourceTypes resourceType;
    public List<string> nameParts;

   
}
[CreateAssetMenu(fileName = "NameGenerator")]
public class NameGenerator : ScriptableObject
{
    public CharacterInfoSO charachter;
    public string finalName;
    public List<ResourceNameParts> namePartsList;
    public string GenerateRandomName(List<TypeEnums.ResourceTypes> resourcesUsed)
    {

        finalName = null;
        foreach (var res in resourcesUsed)
        {
            foreach (var namepart in namePartsList)
            {
                if (namepart.resourceType == res)
                {
                    string temp =  namepart.nameParts[Random.Range(0,  namepart.nameParts.Count - 1)];
                    finalName += temp;
                }
            }
        }

        charachter.charNAme = finalName;
        return finalName;
       
    }

}
