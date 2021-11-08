using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CharacterSelectionView")]
public class CharacterSelectionView : ScriptableObject
{
   public CharacterInfoSO activeCharacter;
   public RawImage characterImage;

   public void SetSelectedCharacter(CharacterInfoSO character,RawImage nftImage)
   {
      activeCharacter = character;
      characterImage = nftImage;
   }
}
