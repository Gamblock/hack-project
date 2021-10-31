using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppController : MonoBehaviour
{
   public CharacterInfoSO characterInfoSo;
   public SetupCardFromCharacterSO cardSetter;
   public Button creatCharacterButton;
   public VoidEventChannelSO onServerDataNotFound;

   public void AllowCharacterCreation()
   {
       Debug.Log("nodatat");
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
