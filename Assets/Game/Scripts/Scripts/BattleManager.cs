using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
  public IDSO idso;
  public SetCharacterFromSO setChar;
  public Storage storage;

  private void Start()
  {
    setChar.characterInfoSo = storage.GetCharacter(idso.ID);
    setChar.SetCharacter();
  }
}
