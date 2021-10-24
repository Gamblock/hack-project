using System;
using System.Collections;
using System.Collections.Generic;
using PsychoticLab;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterCreationManager : MonoBehaviour
{
   
   public int numberOfSlots = 3;
   public CharacterRandomizer randomizer;
   [FormerlySerializedAs("stats")] public StatsUI statsUI;
   public int statRange;
   public int medianStatValue;

   private List<TypeEnums.ResourceTypes> currentSelectedResources = new List<TypeEnums.ResourceTypes>();

   public void ManageObjectList(bool addToList, TypeEnums.ResourceTypes resourceType)
   {
       if (addToList)
       {
           currentSelectedResources.Add(resourceType);
       }
       else
       {
           currentSelectedResources.Remove(resourceType);
       }

       if (numberOfSlots <= currentSelectedResources.Count)
       {
           statsUI.RandomizeStats(medianStatValue,statRange, currentSelectedResources);
           randomizer.Randomize();
       }
       
   }
}
