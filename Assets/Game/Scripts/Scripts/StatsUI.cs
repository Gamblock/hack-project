using System;
using System.Collections;
using System.Collections.Generic;
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
  
   public int StatValue;
}
public class StatsUI : MonoBehaviour
{
   public CharacterInfoSO characterInfoSo;
   public List<ResourceInfluence> influences;
   public List<StatTypesToUse> statsTouse;
   public VerticalLayoutGroup statsUIGroup;
   public TextMeshProUGUI textPrefab;
   

   public void RandomizeStats(int median, int range, List<TypeEnums.ResourceTypes> resourcesUsed)
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
         characterInfoSo.characterStats.Add(stat);
         TextMeshProUGUI text = Instantiate(textPrefab, statsUIGroup.transform);
         text.text = stat.statToUse.ToString() + " " + stat.StatValue;
      }
   }
}
