using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Storage")]
public class Storage : ScriptableObject
{
    public static string waterResource = "WATER";
    public static string fireResource = "FIRE";
    public static string earthResource = "EARTH";
    public static string savedCharId = "CHARID";
    public IDSO idso;
    public int GetIntByKey(string key)
    {
      return  ES3.Load<int>(key);
    }
    public void SaveIntByKey(string key,int value)
    {
        ES3.Save(key,value);
    }
    
    public void SaveID(CharacterInfoSO characterInfoSo)
    {
        ES3.Save(savedCharId,characterInfoSo.id);
    }
    public void SaveCharacter(CharacterInfoSO characterInfoSo)
    {
        if (!ES3.KeyExists(characterInfoSo.id))
        {
            ES3.Save(characterInfoSo.id,characterInfoSo); 
            SaveID( characterInfoSo);
            idso.ID = characterInfoSo.id;
        }
    }

    public CharacterInfoSO GetCharacter(string id)
    {
       return ES3.Load<CharacterInfoSO>(id);
    }
    public int UpdateResourceValue(TypeEnums.ResourceTypes resourceType, int amountToAdd)
    {
        if (resourceType == TypeEnums.ResourceTypes.earth)
        {
            if (amountToAdd == 0)
            {
                return ES3.Load<int>(earthResource);
            }
            int value;
            if (ES3.KeyExists(earthResource))
            {
                value =  ES3.Load<int>(earthResource);
                value = value + amountToAdd;
            }
            else
            {
                value = amountToAdd;
            }
            ES3.Save(earthResource,value);
          return value;
        }
        
        if (resourceType == TypeEnums.ResourceTypes.fire)
        {
            if (amountToAdd == 0)
            {
                return ES3.Load<int>(fireResource);
            }
            int value;
            if (ES3.KeyExists(fireResource))
            {
                value =  ES3.Load<int>(fireResource);
                value = value + amountToAdd;  
            }
            else
            {
                value = amountToAdd;
            }
            
            ES3.Save(fireResource,value);
            return value;
        }
        if (resourceType == TypeEnums.ResourceTypes.water)
        {
            if (amountToAdd == 0)
            {
                return ES3.Load<int>(waterResource);
            }
            int value;
            if (ES3.KeyExists(waterResource))
            {
                value =  ES3.Load<int>(waterResource );
                value = value + amountToAdd;
            }
            else
            {
                value = amountToAdd;
            }
            ES3.Save(waterResource ,value);
            return value;
        }

        return 0;
    }
}
