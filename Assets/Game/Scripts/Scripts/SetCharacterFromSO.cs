using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SetCharacterFromSO : MonoBehaviour
{
   public CharacterInfoSO characterInfoSo;
   public ServerCommunicationManager serverManager;
   public BattleSystemUISetup battleUI;
   
   private List<GameObject> allParts = new List<GameObject>();
   private List<GameObject> activePartsToSave = new List<GameObject>();
   private int presetIndex;

   private void Awake()
   {
      Transform[] childTransforms = this.transform.GetComponentsInChildren<Transform>();
      foreach (var tr in childTransforms)
      {
         allParts.Add(tr.gameObject);
      }
      Debug.Log(childTransforms.Length);
      foreach (var tr in childTransforms)
      {
         if (tr.gameObject.name.Contains("Chr_") || tr.gameObject.name.Contains("Helmet"))
         {
            tr.gameObject.SetActive(false);
         }
      }
   }

   private void Start()
   {
      SetCharacter();
   }


   public void SaveActiveParts(CharacterInfoSO charInfoSO)
   {   
      charInfoSO.objectIndexes.Clear();
      for (int i = 0; i < allParts.Count; i++)
      {
         if (allParts[i].activeSelf && allParts[i].gameObject.name.Contains("Chr_"))
         {
            charInfoSO.objectIndexes.Add(i);
         }
      }
   }
   public void SetSpecifiedCharacter(CharacterInfoSO charInfoSO)
   {
      
      foreach (var index in charInfoSO.objectIndexes)
      {
        
         if (allParts[index] != null)
         {
            allParts[index].gameObject.SetActive(true);
         }
         
      }
   }

   private IEnumerator SettingCharacter()
   {
      yield return new WaitForSeconds(0.5f);
      serverManager.GetCharacterFromServer(characterInfoSo);
      Debug.Log(characterInfoSo.objectIndexes.Count);
      battleUI.SetUIButtons(characterInfoSo);
      foreach (var index in characterInfoSo.objectIndexes)
      {
        
         if (allParts[index] != null)
         {
            allParts[index].gameObject.SetActive(true);
         }
         
      }
   }
   public void SetCharacter()
   {
      StartCoroutine(SettingCharacter());
   }

}


