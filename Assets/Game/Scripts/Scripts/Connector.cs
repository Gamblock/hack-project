using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Connector : MonoBehaviour
{
   private void Awake()
   {
      ShowEmail("gg");
   }

   public TextMeshProUGUI text;
   public void ShowEmail(string email)
   {
      text.text = email;
   }
}
