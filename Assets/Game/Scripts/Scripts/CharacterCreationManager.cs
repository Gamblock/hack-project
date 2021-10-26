using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PsychoticLab;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterCreationManager : MonoBehaviour
{
   
   public int numberOfSlots = 3;
   public CharacterRandomizer randomizer;
   [FormerlySerializedAs("stats")] public StatsUI statsUI;
   public int statRange;
   public int medianStatValue;
   public TextMeshProUGUI nameText;
   public NameGenerator nameGenerator;
   public VoidEventChannelSO onCharacterCreated;
   public Storage storage;
   public ResourceController resourceController;

   private List<TypeEnums.ResourceTypes> currentSelectedResources = new List<TypeEnums.ResourceTypes>();
   private bool characterIsCreated;

   private void Start()
   {
       resourceController.ShowResourceValues();
   }

   private void OnEnable()
   {
       onCharacterCreated.OnEventRaised += OnCharacterCreated;
   }

   private void OnDisable()
   {
       onCharacterCreated.OnEventRaised -= OnCharacterCreated;
   }

   public void OnCharacterCreated()
   {
       characterIsCreated = true;
   }
   public void ManageObjectList(bool addToList, TypeEnums.ResourceTypes resourceType)
   {
       if (!characterIsCreated)
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
               nameText.text = nameGenerator.GenerateRandomName(currentSelectedResources);
               storage.SaveCharacter(randomizer.characterInfoSo);
           } 
       }
   }
}
