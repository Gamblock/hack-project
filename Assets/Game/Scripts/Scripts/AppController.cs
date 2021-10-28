using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour
{
   public CharacterInfoSO characterInfoSo;
   public SetupCardFromCharacterSO cardSetter;

   private void Start()
   {
      
   }

   private void OnEnable()
   {
      
      if (characterInfoSo.objectIndexes.Count > 0)
      {
         Debug.Log("enab");
         cardSetter.SetUpCard(characterInfoSo);
      }
   }
}
