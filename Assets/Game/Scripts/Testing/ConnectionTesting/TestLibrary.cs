using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Runtime.InteropServices;

public class TestLibrary : MonoBehaviour
{
   public ServerCommunicationManager serverManager;

   private void Start()
   {
      serverManager.GetUserByEmail("admin@admin.com");
     // serverManager.SetTokenAmountOnServer(100);
      Debug.Log(PlayerPrefs.GetInt("CASHMONEY"));
   }
}
