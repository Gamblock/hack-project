using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AppController : MonoBehaviour
{
   public CharacterInfoSO characterInfoSo;
   [FormerlySerializedAs("cardSetter")] public SetupCardsFromCharacterSO cardsSetter;
   public Button creatCharacterButton;
   public VoidEventChannelSO onServerDataNotFound;

   public void AllowCharacterCreation()
   {
       creatCharacterButton.interactable = true;
   }
   private void OnEnable()
   {
       onServerDataNotFound.OnEventRaised += AllowCharacterCreation;
//      cardSetter.SetupCardFromServer();
   }

   private void OnDisable()
   {
       onServerDataNotFound.OnEventRaised -= AllowCharacterCreation;
   }
}
