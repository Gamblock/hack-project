using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectionSetter : MonoBehaviour
{
   public SetupCardsFromCharacterSO cardSetterPrefab;
   public GridLayout cardGrid;


   public void SetupSelectableCardGrid(List<CharacterInfoSO> avaliableCahracters)
   {
      foreach (var character in avaliableCahracters)
      {
          SetupCardsFromCharacterSO cardSetterTemp = Instantiate(cardSetterPrefab, cardGrid.transform);
          cardSetterTemp.SetupCardFromServer();
      }
   }
}
