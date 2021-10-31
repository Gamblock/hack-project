using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public struct ResourceInfluence
{
   public TypeEnums.ResourceTypes typeOfResource;
   public TypeEnums.StatTypes statToInfluence;
   public int influenceAmount;
}

[Serializable]
public class StatTypesToUse
{
   public TypeEnums.StatTypes statToUse;
  [ReadOnly]
   public int StatValue;
}

[Serializable]
public class StatUIElement
{
   public TextMeshProUGUI text;
   public TypeEnums.StatTypes statType;
}
public class StatsUI : MonoBehaviour
{
   public List<StatUIElement> uiElements;
   public List<ResourceInfluence> influences;
   public List<StatTypesToUse> statsTouse;
   public TextMeshProUGUI classText;
   public LoadoutSpawner loadoutSpawner;


  
  
   public void RandomizeStats(int median, int range, List<TypeEnums.ResourceTypes> resourcesUsed,CharacterInfoSO character)
   {
      foreach (var res in resourcesUsed)
      {
         foreach (var influence in influences)
         {
            if (influence.typeOfResource == res)
            {

               for (int i = 0; i < statsTouse.Count; i++)
               {
                  if (statsTouse[i].statToUse == influence.statToInfluence)
                  {
                     statsTouse[i].StatValue = Random.Range((median + influence.influenceAmount - range),
                        median + influence.influenceAmount + range);
                  }
               }
            }
         }
      }
      
      foreach (var stat in statsTouse)
     {
        if (stat.StatValue == 0)
        {
           
           stat.StatValue = Random.Range((median  - range),
              median + range);
           
        }
     }

      foreach (var stat in statsTouse)
      {
         foreach (var element in uiElements)
         {
            if (stat.statToUse == element.statType)
            {
               character.characterStats.Add(stat);
               element.text.text = stat.StatValue.ToString();
            }
         }
      }

      int index = 0;
      int largerValue = 0;
      
      for (int i = 0; i < statsTouse.Count; i++)
      {
         if (statsTouse[i].StatValue > largerValue)
         {
            largerValue = statsTouse[i].StatValue;
            index = i;
         }
      }

      Debug.Log(statsTouse[index].statToUse);
   
      if (statsTouse[index].statToUse == TypeEnums.StatTypes.armor)
      {
         character.classType = TypeEnums.ClassTypes.Tank;
         classText.text = TypeEnums.ClassTypes.Tank.ToString();
         loadoutSpawner.SpawnLoadOut(TypeEnums.ClassTypes.Tank);
      }
      if (statsTouse[index].statToUse == TypeEnums.StatTypes.health)
      {
         character.classType = TypeEnums.ClassTypes.Healer;
         classText.text = TypeEnums.ClassTypes.Healer.ToString();
         loadoutSpawner.SpawnLoadOut(TypeEnums.ClassTypes.Healer);
      }
      if (statsTouse[index].statToUse== TypeEnums.StatTypes.strength)
      {
         character.classType = TypeEnums.ClassTypes.DamageDealer;
         classText.text = TypeEnums.ClassTypes.DamageDealer.ToString();
         loadoutSpawner.SpawnLoadOut(TypeEnums.ClassTypes.DamageDealer);
      }
      
      
   }
}
