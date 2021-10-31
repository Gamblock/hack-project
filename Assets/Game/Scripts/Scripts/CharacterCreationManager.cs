using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsychoticLab;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterCreationManager : MonoBehaviour
{
   
   public int numberOfSlots = 3;
   public CharacterRandomizer randomizer;
   [FormerlySerializedAs("stats")] public StatsUI statsUI;
   public int statRange;
   public int medianStatValue;
   public TextMeshProUGUI nameText;
   public NameGenerator nameGenerator;
   public CanvasGroup errorTextGroup;
   public CanvasGroup characterCreationMenu;
   public CharacterInfoSO character;
   public Button continueButton;
   public CanvasGroup createdCharacterUI;
   
   public ServerCommunicationManager serverManager;
   public CurrentSelectedResourcesSO currentSelectedResourcesSo;
   private bool characterIsCreated;


   private void Awake()
   {
       currentSelectedResourcesSo.selectedResourcesList.Clear();
       character.ClearTemplate();
   }

   private void Update()
   {
       if (currentSelectedResourcesSo.selectedResourcesList.Count == numberOfSlots)
       {
           continueButton.interactable = true;
       }
       else
       {
           continueButton.interactable = false;
       }
   }

   public async void CreateCharacter()
   {
       randomizer.gameObject.SetActive(true);
       await Task.Delay(TimeSpan.FromSeconds(0.05));
       if (currentSelectedResourcesSo.selectedResourcesList.Count == numberOfSlots)
       {
           statsUI.RandomizeStats(medianStatValue,statRange, currentSelectedResourcesSo.selectedResourcesList, character);
           randomizer.Randomize();
           nameText.text = nameGenerator.GenerateRandomName(currentSelectedResourcesSo.selectedResourcesList);
           characterCreationMenu.alpha = 0;
           characterCreationMenu.interactable = false;
           createdCharacterUI.alpha = 1;
           await Task.Delay(TimeSpan.FromSeconds(0.05));
           ScreenCapturer.TakeScreenShot_Static(400,540);
       }
       else
       {
           errorTextGroup.alpha = 1;
           errorTextGroup.GetComponentInChildren<TextMeshProUGUI>().text = "You need to select 3 resource types";
           await Task.Delay(TimeSpan.FromSeconds(3));
           errorTextGroup.alpha = 0;
       }
   }
}
