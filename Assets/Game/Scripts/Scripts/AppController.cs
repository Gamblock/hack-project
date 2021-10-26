using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour
{
   public int StartResourceAmount = 10;
   public Storage storage;
   public ResourceController resourceController;
   private static string TutorialPassedKey = "TUTORIALPASSED";
   public IDSO idso;
   public SetCharacterFromSO setChar;
   public StatsUI statsUI;
   

   private void Start()
   {

      if (idso.ID.Length >= 5)
      {
         setChar.characterInfoSo = storage.GetCharacter(idso.ID);
         setChar.SetCharacter();
         statsUI.ShowStats( storage.GetCharacter(idso.ID));
      }
      if (storage.GetIntByKey(TutorialPassedKey) == 0)
      {
         storage.UpdateResourceValue(TypeEnums.ResourceTypes.earth, StartResourceAmount);
         storage.UpdateResourceValue(TypeEnums.ResourceTypes.water, StartResourceAmount);
         storage.UpdateResourceValue(TypeEnums.ResourceTypes.fire, StartResourceAmount);
         storage.SaveIntByKey(TutorialPassedKey,1);
      }
      resourceController.ShowResourceValues();
      
   }

  
}
