using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCharacterFromSO : MonoBehaviour
{
   public CharacterInfoSO characterInfoSo;
   private List<GameObject> allParts = new List<GameObject>();


   private void Start()
   {
      Transform[] childTransforms = this.transform.GetComponentsInChildren<Transform>();
      foreach (var tr in childTransforms)
      {
         allParts.Add(tr.gameObject);
      }

      foreach (var tr in childTransforms)
      {
         if (tr.gameObject.name.Contains("Chr_") || tr.gameObject.name.Contains("Helmet"))
         {
            tr.gameObject.SetActive(false);
         }
      }
   }

   public void SetCharacter()
   {
      
      foreach (var index in characterInfoSo.objectIndexes)
      {
        
         if (allParts[index] != null)
         {
            allParts[index].gameObject.SetActive(true);
         }
         
      }
   }
}
